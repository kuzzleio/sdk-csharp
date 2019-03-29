using System.Collections.Concurrent;

namespace Kuzzle.Protocol {
  abstract class Protocol {
    public string Endpoint { get; protected set; }

    protected Protocol(string endpoint) {

    }

    public abstract void Connect();
    public abstract void Disconnect();
    public abstract void Send(string id, string query, ResponseDelegate response);

    public delegate void ResponseDelegate(Response r);
  }
}
