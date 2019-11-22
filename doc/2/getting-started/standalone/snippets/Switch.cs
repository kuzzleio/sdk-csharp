using KuzzleSdk;
using KuzzleSdk.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace getting_started_csharp
{
  class Init
  {
    static async Task Main(string[] args)
    {
      Console.WriteLine(args[0]);

      switch (args[0])
      {
        case "init":
          await Init();
          break;

        case "subscribe":
          await Subscribe();
          break;

        case "create":
          await Create();
          break;
      }
    }

    static async Task Init() {
      return;
    }
    static async Task Subscribe() {
      return;
    }
    static async Task Create() {
      return;
    }
  }
}
