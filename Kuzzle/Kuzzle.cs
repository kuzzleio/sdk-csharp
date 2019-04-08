using System.Collections.Generic;
using System.Threading.Tasks;
using Kuzzle.Protocol;
using Kuzzle.API;
using Kuzzle.API.Controllers;
using Newtonsoft.Json.Linq;

namespace Kuzzle {
  public sealed class Kuzzle {
    private AbstractProtocol networkProtocol;

    private readonly Dictionary<string, TaskCompletionSource<Response>>
        requests = new Dictionary<string, TaskCompletionSource<Response>>();

    public Auth Auth { get; private set; }
    public Document Document { get; private set; }
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
          networkProtocol.ResponseEvent -= ResponseHandler;
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
    private void ResponseHandler(object sender, string payload) {
      Response response = Response.FromString(payload);

      if (requests.ContainsKey(response.RequestId)) {
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
        // if a response is unknown, then it's probably a real-time notification
      }
    }

    /// <summary>
    /// Initialize a new instance of the <see cref="T:Kuzzle.Kuzzle"/> class.
    /// </summary>
    /// <param name="networkProtocol">Network protocol.</param>
    public Kuzzle(AbstractProtocol networkProtocol) {
      NetworkProtocol = networkProtocol;
      NetworkProtocol.ResponseEvent += ResponseHandler;

      // Initializes the controllers
      Auth = new Auth(this);
      Document = new Document(this);
      Server = new Server(this);
    }

    ~Kuzzle() {
      NetworkProtocol.ResponseEvent -= ResponseHandler;
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
      if (Jwt != null) {
        query["jwt"] = Jwt;
      }

      string requestId = System.Guid.NewGuid().ToString();
      query["requestId"] = requestId;

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
