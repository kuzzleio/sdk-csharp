using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kuzzle.Protocol;
using Kuzzle.API;
using Kuzzle.API.Controllers;
using Newtonsoft.Json.Linq;
using System;

namespace Kuzzle {
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

    public Auth Auth { get; private set; }
    public Document Document { get; private set; }
    public Realtime Realtime { get; private set; }
    public Server Server { get; private set; }

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
      Auth = new Auth(this);
      Document = new Document(this);
      Realtime = new Realtime(this);
      Server = new Server(this);

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
