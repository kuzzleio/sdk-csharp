using System;
using System.Threading.Tasks;
using KuzzleSdk;
using KuzzleSdk.Exceptions;
using KuzzleSdk.Protocol;
using Moq;
using Newtonsoft.Json.Linq;

namespace Kuzzle.Tests.Protocol {
  public class ProtocolMock {
    public Mock<AbstractProtocol> Mock { get; }

    public AbstractProtocol MockedObject {
      get { return Mock.Object; }
    }

    public ProtocolMock() {
      Mock = new Mock<AbstractProtocol>();
    }
  }
}
