using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuzzleSdk.Protocol;
using KuzzleSdk.API;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using KuzzleSdk.API.Offline;
using KuzzleSdk.EventHandler;

[assembly: InternalsVisibleTo("Kuzzle.Tests")]

// Allow using Moq on internal objects/interfaces
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace KuzzleSdk {
  /// <summary>
  /// Kuzzle API interface.
  /// </summary>
  public interface IKuzzleApi {
    /// <summary>
    /// Gets or sets the authentication token.
    /// </summary>
    /// <value>The authentication token.</value>
    string AuthenticationToken { get; set; }

    AbstractKuzzleEventHandler EventHandler { get; }

    /// <summary>
    /// Gets the instance identifier.
    /// </summary>
    /// <value>The instance identifier.</value>
    string InstanceId { get; }

    /// <summary>
    /// Gets the network protocol.
    /// </summary>
    /// <value>The network protocol.</value>
    AbstractProtocol NetworkProtocol { get; }

    /// <summary>
    /// Sends a query to Kuzzle's API
    /// </summary>
    /// <returns>The query response.</returns>
    /// <param name="query">Kuzzle API query</param>
    Task<Response> QueryAsync(JObject query);

    /// <summary>
    /// Dispatches a TokenExpired event.
    /// </summary>
    [Obsolete("use IKuzzleApi.EventHandler.DispatchTokenExpired instead", false)]
    void DispatchTokenExpired();

    /// <summary>
    /// Occurs when an unhandled response is received.
    /// </summary>
    [Obsolete("use IKuzzleApi.EventHandler.UnhandledResponse event instead", false)]
    event EventHandler<Response> UnhandledResponse;

    /// <summary>
    /// Occurs when the authentication token has expired
    /// </summary>
    event Action TokenExpired;
  }

  internal interface IKuzzle {
    string AuthenticationToken { get; set; }

    IAuthController GetAuth();
    IRealtimeController GetRealtime();
    AbstractKuzzleEventHandler GetEventHandler();

    TaskCompletionSource<Response> GetRequestById(string requestId);
    }

  /// <summary>
  /// Main entry point for this SDK.
  /// </summary>
  public sealed class Kuzzle : IKuzzleApi, IKuzzle {
    private AbstractProtocol networkProtocol;

    internal readonly Dictionary<string, TaskCompletionSource<Response>>
        requests = new Dictionary<string, TaskCompletionSource<Response>>();

    // General informations

    /// <summary>
    /// This SDK Version
    /// </summary>
    public readonly string Version;

    /// <summary>
    /// Instance unique identifier.
    /// </summary>
    public string InstanceId { get; }

    // Emitter for all responses not directly linked to a user request
    // (i.e. all real-time notifications)
    [Obsolete(@"The UnhandledResponse event from Kuzzle is deprecated, use UnhandledResponse event from Kuzzle.EventHandler instead", false)]
    public event EventHandler<Response> UnhandledResponse {
      add {
        EventHandler.UnhandledResponse += value;
      }

      remove {
        EventHandler.UnhandledResponse -= value;
      }
    }

    /// <summary>
    /// Token expiration event
    /// </summary>
    [Obsolete(@"The TokenExpired event from Kuzzle is deprecated, use TokenExpired event from Kuzzle.EventHandler instead", false)]
    public event Action TokenExpired {
      add {
        EventHandler.TokenExpired += value;
      }

      remove {
        EventHandler.TokenExpired -= value;
      }
    }

    [Obsolete(@"DispatchTokenExpired from Kuzzle is deprecated, use DispatchTokenExpired from Kuzzle.EventHandler instead", false)]
    public void DispatchTokenExpired() {
      EventHandler.DispatchTokenExpired();
    }

    /// <summary>
    /// Exposes actions from the "auth" Kuzzle API controller
    /// </summary>
    public AuthController Auth { get; private set; }

    /// <summary>
    /// Exposes actions from the "collection" Kuzzle API controller
    /// </summary>
    public CollectionController Collection { get; private set; }

    /// <summary>
    /// Exposes actions from the "document" Kuzzle API controller
    /// </summary>
    public DocumentController Document { get; private set; }
  
    /// <summary>
    /// Exposes actions from the "index" Kuzzle API controller
    /// </summary>
    public IndexController Index { get; private set; }

    /// <summary>
    /// Exposes actions from the "realtime" Kuzzle API controller
    /// </summary>
    public RealtimeController Realtime { get; private set; }

    /// <summary>
    /// Exposes actions from the "server" Kuzzle API controller
    /// </summary>
    public ServerController Server { get; private set; }

    /// <summary>
    /// Exposes actions from the "bulk" Kuzzle API controller
    /// </summary>
    public BulkController Bulk { get; private set; }

    /// <summary>
    /// Exposes actions from the "admin" Kuzzle API controller
    /// </summary>
    public AdminController Admin { get; private set; }

    /// <summary>
    /// Exposes the event handler
    /// </summary>
    public AbstractKuzzleEventHandler EventHandler { get; private set; }

    /// <summary>
    /// Exposes actions from the OfflineManager
    /// </summary>
    public OfflineManager Offline { get; private set; }

    /// <summary>
    /// The maximum amount of elements that the queue can contains.
    /// If set to -1, the size is unlimited.
    /// </summary>
    public int MaxQueueSize {
      get { return Offline.MaxQueueSize; }
      set { Offline.MaxQueueSize = value; }
    }

    /// <summary>
    /// The minimum duration of a Token after refresh.
    /// If set to -1 the SDK does not refresh the token automaticaly.
    /// </summary>
    public int RefreshedTokenDuration {
      get { return Offline.RefreshedTokenDuration; }
      set { Offline.RefreshedTokenDuration = value; }
    }

    /// <summary>
    /// The minimum duration of a Token before being automaticaly refreshed.
    /// If set to -1 the SDK does not refresh the token automaticaly.
    /// </summary>
    public int MinTokenDuration {
      get { return Offline.MinTokenDuration; }
      set { Offline.MinTokenDuration = value; }
    }

    /// <summary>
    /// The maximum delay between two requests to be replayed
    /// </summary>
    public int MaxRequestDelay {
      get { return Offline.MaxRequestDelay; }
      set { Offline.MaxRequestDelay = value; }
    }

    public Func<JObject, bool> QueueFilter {
      get { return Offline.QueueFilter; }
      set { Offline.QueueFilter = value; }
    }

    /// <summary>
    /// Queue requests when network is down,
    /// and automatically replay them when the SDK successfully reconnects.
    /// </summary>
    public bool AutoRecover {
      get { return Offline.AutoRecover; }
      set { Offline.AutoRecover = value; }
    }

    /// <summary>
    /// Authentication token
    /// </summary>
    public string AuthenticationToken { get; set; }

    /// <summary>
    /// Authentication token (deprecated, use AuthenticationToken instead)
    /// </summary>
    [Obsolete(
      "The Jwt property is deprecated, use AuthencationToken instead", false)]
    string Jwt {
      get { return AuthenticationToken; }
      set { AuthenticationToken = value; }
    }

    /// <summary>
    /// Network Protocol
    /// </summary>
    public AbstractProtocol NetworkProtocol {
      get {
        return networkProtocol;
      }
      set {
        if (networkProtocol != null) {
          networkProtocol.ResponseEvent -= ResponsesListener;
          NetworkProtocol.StateChanged -= StateChangeListener;
        }

        AuthenticationToken = null;
        networkProtocol = value;
      }
    }


    /// <summary>
    /// Handles the ResponseEvent event from the network protocol
    /// </summary>
    /// <param name="sender">Network Protocol instance</param>
    /// <param name="payload">raw API Response</param>
    internal void ResponsesListener(object sender, string payload) {
      Response response = Response.FromString(payload);

      if (requests.ContainsKey(response.Room)) {
        if (response.Error != null) {
          if (response.Error.Message == "Token expired") {
            EventHandler.DispatchTokenExpired();
          }

          requests[response.RequestId].SetException(
            new Exceptions.ApiErrorException(response));
        } else {
          requests[response.RequestId].SetResult(response);
        }

        lock (requests) {
          requests.Remove(response.RequestId);
        }

        Offline?.QueryReplayer?.Remove((obj) => obj["requestId"].ToString() == response.RequestId);

      } else {
        EventHandler.DispatchUnhandledResponse(response);
      }
    }

    internal void StateChangeListener(object sender, ProtocolState state) {
      // If not connected anymore: close tasks and clean up the requests buffer
      if (state == ProtocolState.Closed) {
        lock (requests) {
          foreach (var task in requests.Values) {
            task.SetException(new Exceptions.ConnectionLostException());
          }

          requests.Clear();
        }
      }
    }


    /// <summary>
    /// Initialize a new instance of the <see cref="T:Kuzzle.Kuzzle"/> class.
    /// </summary>
    /// <param name="networkProtocol">Network protocol.</param>
    public Kuzzle(
      AbstractProtocol networkProtocol,
      int refreshedTokenDuration = 3600000,
      int minTokenDuration = 3600000,
      int maxQueueSize = -1,
      int maxRequestDelay = 1000,
      Func<JObject, bool> queueFiler = null
    ) {
      NetworkProtocol = networkProtocol;
      NetworkProtocol.ResponseEvent += ResponsesListener;
      NetworkProtocol.StateChanged += StateChangeListener;

      EventHandler = new KuzzleEventHandler(this);

      // Initializes the controllers
      Auth = new AuthController(this);
      Collection = new CollectionController(this);
      Document = new DocumentController(this);
      Index = new IndexController(this);
      Realtime = new RealtimeController(this);
      Server = new ServerController(this);
      Bulk = new BulkController(this);
      Admin = new AdminController(this);

      Offline = new OfflineManager(networkProtocol, this) {
        RefreshedTokenDuration = refreshedTokenDuration,
        MinTokenDuration = minTokenDuration,
        MaxQueueSize = maxQueueSize,
        MaxRequestDelay = maxRequestDelay,
        QueueFilter = queueFiler
      };

      // Initializes instance unique properties
      Version = typeof(Kuzzle)
        .GetTypeInfo()
        .Assembly
        .GetName()
        .Version
        .ToString();
      InstanceId = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations
    /// before the <see cref="T:KuzzleSdk.Kuzzle"/>
    /// is reclaimed by garbage collection.
    /// </summary>
    ~Kuzzle() {
      NetworkProtocol.ResponseEvent -= ResponsesListener;
      NetworkProtocol.StateChanged -= StateChangeListener;
    }

    /// <summary>
    /// Establish a network connection
    /// </summary>
    public async Task ConnectAsync(CancellationToken cancellationToken) {
      await NetworkProtocol.ConnectAsync(cancellationToken);
    }

    /// <summary>
    /// Disconnect this instance.
    /// </summary>
    public void Disconnect() {
      NetworkProtocol.Disconnect();
    }

    TaskCompletionSource<Response> IKuzzle.GetRequestById(string requestId) {
      return requests[requestId];
    }

    /// <summary>
    /// Sends an API request to Kuzzle and returns the corresponding API
    /// response.
    /// </summary>
    /// <returns>API response</returns>
    /// <param name="query">Kuzzle API query</param>
    public Task<Response> QueryAsync(JObject query) {
      if (query == null) {
        throw new Exceptions.InternalException("You must provide a query", 400);
      }

      if (NetworkProtocol.State == ProtocolState.Closed) {
        throw new Exceptions.NotConnectedException();
      }

      if (query["waitForRefresh"] != null) {
        if (query["waitForRefresh"].ToObject<bool>()) {
          query.Add("refresh", "wait_for");
        }
        query.Remove("waitForRefresh");
      }

      if (AuthenticationToken != null) {
        query["jwt"] = AuthenticationToken;
      }

      string requestId = Guid.NewGuid().ToString();
      query["requestId"] = requestId;

      if (query["volatile"] == null) {
        query["volatile"] = new JObject();
      } else if (!(query["volatile"] is JObject)) {
        throw new Exceptions.InternalException("Volatile data must be a JObject", 400);
      }

      query["volatile"]["sdkVersion"] = Version;
      query["volatile"]["sdkInstanceId"] = InstanceId;

      if (NetworkProtocol.State == ProtocolState.Open) {
        NetworkProtocol.Send(query);
      } else if (NetworkProtocol.State == ProtocolState.Reconnecting) {
        Offline.QueryReplayer.Enqueue(query);
      }

      lock (requests) {
        requests[requestId] = new TaskCompletionSource<Response>();
      }

      return requests[requestId].Task;
    }

    IAuthController IKuzzle.GetAuth() {
      return Auth;
    }

    IRealtimeController IKuzzle.GetRealtime() {
      return Realtime;
    }

    AbstractKuzzleEventHandler IKuzzle.GetEventHandler() {
      return EventHandler;
    }
  }
}
