using Xunit;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using System;

namespace Kuzzle.Tests.API.Controllers
{
    public class ServerControllerTest
    {
        private readonly ServerController _serverController;
        private readonly KuzzleApiMock _api;

        public ServerControllerTest()
        {
            _api = new KuzzleApiMock();
            _serverController = new ServerController(_api.MockedObject);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void AdminExistsTest(bool returnValue)
        {
            _api.SetResult(new JObject { {
                "result",  new JObject {
                    { "exists", returnValue }
                } }
            });

            bool res = await _serverController.AdminExistsAsync();

            _api.Verify(new JObject{
                { "controller", "server" },
                { "action", "adminExists"}
            });

            Assert.Equal(returnValue, res);
        }


        [Fact]
        public async void NowTest()
        {
            _api.SetResult(@"{result: {now: 1111111111111}}");

            Int64 res = await _serverController.NowAsync();

            _api.Verify(new JObject{
                {"controller", "server"},
                {"action", "now"}
            });
            Assert.Equal<Int64>(1111111111111, res);
        }
    
        [Fact]
        public async void InfoTest(){
            JObject serverInfo = new JObject {{ "kuzzle", "installed"}};
            _api.SetResult(new JObject {{ "result", new JObject {{ "serverInfo", serverInfo }} }});

            JObject res = await _serverController.InfoAsync();

            _api.Verify(new JObject{ 
                {"controller", "server"}, 
                {"action", "info"}
            });
            Assert.Equal<JObject>(serverInfo, res);
        }

        [Fact]
        public async void ConfigTest(){
            _api.SetResult(@"{result: {limits: 'none'}}");

            JObject res = await _serverController.GetConfigAsync();

            _api.Verify(new JObject{ 
                {"controller", "server"}, 
                {"action", "getConfig"}
            });
            Assert.Equal<JObject>(
                new JObject {{ "limits", "none"}},
                res
            );
        }

        [Fact]
        public async void GetStatsTest(){
            _api.SetResult(@"{ result: { cake: 'lie'} }");
            long start = 0; 
            long end = 1;

            JObject res = await _serverController.GetStatsAsync(start, end);

            _api.Verify(new JObject {
                { "controller", "server" }, 
                { "action", "getStats" },
                { "startTime", start },
                { "stopTime", end }
            }); 
            Assert.Equal<JObject>(
                new JObject {{ "cake", "lie" }}, 
                res
            );
        }

        [Fact]
        public async void GetAllStatsTest(){
            _api.SetResult(@"{ result: { cake: 'lie'} }");

            JObject res = await _serverController.GetAllStatsAsync();

            _api.Verify(new JObject {
                { "controller", "server" }, 
                { "action", "getAllStats" }
            }); 
            Assert.Equal<JObject>(
                new JObject {{ "cake", "lie" }}, 
                res
            );
        }

        [Fact]
        public async void GetLastStatsTest(){
            _api.SetResult(@"{ result: { cake: 'lie'} }");

            JObject res = await _serverController.GetLastStatsAsync();

            _api.Verify(new JObject {
                { "controller", "server" }, 
                { "action", "getLastStats" }
            }); 
            Assert.Equal<JObject>(
                new JObject {{ "cake", "lie" }}, 
                res
            );
        }
    }
}