using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk.Protocol {
  /// <summary>
  /// Abstract class laying the groundwork of network protocol communication
  /// between this SDK and Kuzzle backends.
  /// 
  /// Inherit from this class if you want to add new network capabilities to
  /// this SDK.
  /// </summary>
  public abstract class AbstractProtocol {
    /// <summary>
    /// Current connection state
    /// </summary>
    /// <value>The state.</value>
    public ProtocolState State { get; protected set; }

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
      ResponseEvent?.Invoke(this, payload);
    }

    /// <summary>
    /// To be triggered whenever a message is received from Kuzzle
    /// </summary>
    public event EventHandler<string> ResponseEvent;

    /// <summary>
    /// Dispatch a state changed event
    /// </summary>
    protected void DispatchStateChange(ProtocolState state) {
      StateChanged?.Invoke(this, state);
    }

    /// <summary>
    /// To be triggered whenever a message is received from Kuzzle
    /// </summary>
    public event EventHandler<ProtocolState> StateChanged;
  }
}
