using System.Runtime.Serialization;
using System.Json;

namespace Kuzzle {
  [DataContract]
  public struct Response {
    [DataContract]
    public struct ErrorResponse {
      [DataMember]
      public int status;

      [DataMember]
      public string message;
    }

    public JsonObject result;

    [DataMember]
    public ErrorResponse error;

    [DataMember]
    public string requestId;

    [DataMember]
    public int status;

    [DataMember]
    public string controller;

    [DataMember]
    public string action;

    [DataMember]
    public string index;

    [DataMember]
    public string collection;
  }
}
