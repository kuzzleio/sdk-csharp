using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuzzleSdk.Protocol;
using KuzzleSdk.API;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using System;

namespace KuzzleSdk {
  public sealed class Kuzzle {
    private AbstractProtocol networkProtocol;

    private readonly Dictionary<string, TaskCompletionSource<Response>>
        requests = new Dictionary<string, TaskCompletionSource<Response>>();

    // General informations
    public readonly string Version;
    public readonly string InstanceId;

    // Emitter for all responses not directly linked to a user request
    // (i.e. all real-time notifications)
    internal event EventHandler<Response> UnhandledResponse;

    // Emitter for token expiration events
    public event Action TokenExpired;

    internal void TokenHasExpired() {
      Jwt = null;
      TokenExpired?.Invoke();
    }

    public AuthController Auth { get; private set; }
    public DocumentController Document { get; private set; }
    public RealtimeController Realtime { get; private set; }
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
            TokenHasExpired();
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
      // If not connected anymore: clean up
      if (state == ProtocolState.Closed) {
        requests.Clear();
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
      Document = new DocumentController(this);
      Realtime = new RealtimeController(this);
      Server = new ServerController(this);

      // Initializes instance unique properties
      Version = typeof(Kuzzle).GetTypeInfo().Assembly.GetName().Version.ToString();
      InstanceId = Guid.NewGuid().ToString();
    }

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
    public Task<Response> Query(JObject query) {
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

      NetworkProtocol.SendAsync(query).Wait();

      TaskCompletionSource<Response> response =
          new TaskCompletionSource<Response>();

      lock (requests) {
        requests[requestId] = response;
      }

      return response.Task;
    }
  }
}
