using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Kuzzle.Protocol {
  public class MessageEventArgs : EventArgs {
    public JObject Message { get; set; }

    public MessageEventArgs(string msg) {
      Message = JObject.Parse(msg);
    }
  }

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
    /// Dispatch the specified Kuzzle API response event
    /// </summary>
    /// <param name="message">Kuzzle API response.</param>
    protected void OnMessage(MessageEventArgs message) {
      MessageEvent?.Invoke(this, message);
    }

    public event EventHandler<MessageEventArgs> MessageEvent;
  }
}
