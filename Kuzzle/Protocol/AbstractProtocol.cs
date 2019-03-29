using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kuzzle.Protocol {
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
    //public abstract void ConnectAsync();

    /// <summary>
    /// Disconnect this instance.
    /// </summary>
    //public abstract void Disconnect();

    /// <summary>
    /// Send the specified payload to Kuzzle.
    /// </summary>
    /// <param name="payload">Payload data to send across the network</param>
    public abstract void Send(string payload);

    /// <summary>
    /// Dispatch the specified Kuzzle API response event
    /// </summary>
    /// <param name="responseEvent">Kuzzle API response.</param>
    protected void OnResponseReceived(ResponseEventArgs responseEvent) {
      ResponseReceived?.Invoke(this, responseEvent);
    }

    public class ResponseEventArgs : EventArgs {
      public string Response { get; set; }
    }

    public event EventHandler<ResponseEventArgs> ResponseReceived;

    /*
    // Public API
    public delegate void ResponseDelegate(Response response);
    public delegate void NotificationDelegate(Notification notification);

    internal void QueueResponse(string id, ResponseDelegate rdel) {
      requests[id] = rdel;
    }

    internal void Dispatch(string data) {
    }

    // Fields
    protected ConcurrentDictionary<string, List<NotificationDelegate>>
      subscriptions = new ConcurrentDictionary<string, List<NotificationDelegate>>();
    protected ConcurrentDictionary<string, ResponseDelegate> requests =
      new ConcurrentDictionary<string, ResponseDelegate>();

    // Internal methods
    internal void Subscribe(string roomId, NotificationDelegate ndel) {
      if (!subscriptions.ContainsKey(roomId)) {
        subscriptions[roomId] = new List<NotificationDelegate>();
      }

      subscriptions[roomId].Add(ndel);
    }

    internal bool Unsubscribe(string roomId, NotificationDelegate ndel) {
      bool removed = false;

      if (subscriptions.ContainsKey(roomId)) {
        removed = subscriptions[roomId].Remove(ndel);

        if (removed && subscriptions[roomId].Count == 0) {
          subscriptions.TryRemove(roomId, out _);
        }
      }

      return removed;
    }
  }
  */
  }
