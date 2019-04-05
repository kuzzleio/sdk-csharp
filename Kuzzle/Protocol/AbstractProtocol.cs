using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Kuzzle.Protocol {
  public delegate void ResponseReceiver(object sender, ApiResponse response);

  /// <summary>
  /// Abstract class laying the groundwork of network protocol communication
  /// between this SDK and Kuzzle backends.
  /// 
  /// Inherit from this class if you want to add new network capabilities to
  /// this SDK.
  /// </summary>
  public abstract class AbstractProtocol {
    /// <summary>
    /// Connect this instance.
    /// </summary>
    public abstract Task ConnectAsync();

    /// <summary>
    /// Disconnect this instance.
    /// </summary>
    public abstract void Disconnect();

    /// <summary>
    /// Send the specified payload to Kuzzle.
    /// </summary>
    /// <param name="payload">Payload data to send across the network</param>
    public abstract Task SendAsync(JObject payload);

    /// <summary>
    /// Dispatch a message received from a Kuzzle server
    /// </summary>
    /// <param name="payload">Kuzzle API response.</param>
    protected void DispatchResponse(string payload) {
      ResponseEvent?.Invoke(this, ApiResponse.FromString(payload));
    }

    public event EventHandler<ApiResponse> ResponseEvent;
  }
}
