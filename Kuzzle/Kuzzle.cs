using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuzzleSdk.Protocol;
using KuzzleSdk.API;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using System;

namespace KuzzleSdk {
  /// <summary>
  /// Kuzzle API interface.
  /// </summary>
  public interface IKuzzleApi {
    /// <summary>
    /// Gets or sets the authentication token.
    /// </summary>
    /// <value>The authentication token.</value>
    string Jwt { get; set; }

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
    void DispatchTokenExpired();

    /// <summary>
    /// Occurs when an unhandled response is received.
    /// </summary>
    event EventHandler<Response> UnhandledResponse;

    /// <summary>
    /// Occurs when the authentication token has expired
    /// </summary>
    event Action TokenExpired;
  }

  /// <summary>
  /// Main entry point for this SDK.
  /// </summary>
  public sealed class Kuzzle : IKuzzleApi {
    private AbstractProtocol networkProtocol;

    private readonly Dictionary<string, TaskCompletionSource<Response>>
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
    public event EventHandler<Response> UnhandledResponse;

    /// <summary>
    /// Token expiration event
    /// </summary>
    public event Action TokenExpired;

    public void DispatchTokenExpired() {
      Jwt = null;
      TokenExpired?.Invoke();
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
    /// Exposes actions from the "realtime" Kuzzle API controller
    /// </summary>
    public RealtimeController Realtime { get; private set; }

    /// <summary>
    /// Exposes actions from the "server" Kuzzle API controller
    /// </summary>
    public ServerController Server { get; private set; }

    /// <summary>
    /// Authentication token
    /// </summary>
    public string Jwt { get; set; }

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

        Jwt = null;
        networkProtocol = value;
      }
    }

    /// <summary>
    /// Handles the ResponseEvent event from the network protocol
    /// </summary>
    /// <param name="sender">Network Protocol instance</param>
    /// <param name="payload">raw API Response</param>
    private void ResponsesListener(object sender, string payload) {
      Response response = Response.FromString(payload);

      if (requests.ContainsKey(response.Room)) {
        if (response.Error != null) {
          if (response.Error.Message == "Token expired") {
            DispatchTokenExpired();
          }

          requests[response.RequestId].SetException(
            new Exceptions.ApiErrorException(response));
        } else {
          requests[response.RequestId].SetResult(response);
        }

        lock (requests) {
          requests.Remove(response.RequestId);
        }
      } else {
        UnhandledResponse?.Invoke(this, response);
      }
    }

    private void StateChangeListener(object sender, ProtocolState state) {
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
    public Kuzzle(AbstractProtocol networkProtocol) {
      NetworkProtocol = networkProtocol;
      NetworkProtocol.ResponseEvent += ResponsesListener;
      NetworkProtocol.StateChanged += StateChangeListener;

      // Initializes the controllers
      Auth = new AuthController(this);
      Collection = new CollectionController(this);
      Document = new DocumentController(this);
      Realtime = new RealtimeController(this);
      Server = new ServerController(this);

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
    public async Task ConnectAsync() {
      await NetworkProtocol.ConnectAsync();
    }

    /// <summary>
    /// Disconnect this instance.
    /// </summary>
    public void Disconnect() {
      NetworkProtocol.Disconnect();
    }

    /// <summary>
    /// Sends an API request to Kuzzle and returns the corresponding API
    /// response.
    /// </summary>
    /// <returns>API response</returns>
    /// <param name="query">Kuzzle API query</param>
    public Task<Response> QueryAsync(JObject query) {
      if (NetworkProtocol.State != ProtocolState.Open) {
        throw new Exceptions.NotConnectedException();
      }

      if (Jwt != null) {
        query["jwt"] = Jwt;
      }

      string requestId = Guid.NewGuid().ToString();
      query["requestId"] = requestId;

      // Injecting SDK version + instance ID
      if (query["volatile"] == null) {
        query["volatile"] = new JObject();
      }

      query["volatile"]["sdkVersion"] = Version;
      query["volatile"]["sdkInstanceId"] = InstanceId;

      NetworkProtocol.Send(query);

      lock (requests) {
        requests[requestId] = new TaskCompletionSource<Response>();
      }

      return requests[requestId].Task;
    }
  }
}
