using KuzzleSdk.API.Controllers;
using Xunit;
using Newtonsoft.Json.Linq;
using System;
using KuzzleSdk.API.Options;
using KuzzleSdk.API;

namespace Kuzzle.Tests.API.Controllers
{
    public class RealtimeControllerTest
    {
        private readonly RealtimeController _realtimeController;
        private readonly KuzzleApiMock _api;

        public RealtimeControllerTest()
        {
            _api = new KuzzleApiMock();
            _realtimeController = new RealtimeController(_api.MockedObject);
        }

        private void MockNotificationHandler(Response notification)
        {
            Console.WriteLine(notification.Result);
        }

        [Fact]
        public async void CountAsyncTest()
        {
            string roomId = "A113";
            Int32 count = 3;
            _api.SetResult(new JObject { {
                "result", new JObject { {
                    "count", count } }
                } }
            );

            Int32 res = await _realtimeController.CountAsync(roomId);

            _api.Verify(new JObject {
                { "controller", "realtime" },
                { "action", "count" },
                { "body", new JObject {{ "roomId", roomId }}}
            });
            Assert.Equal<Int32>(count, res);
        }

        [Fact]
        public async void PublishAsyncTest()
        {
            string index = "an_index";
            string collection = "to";
            JObject message = new JObject { { "infinity", "and beyond" } };

            await _realtimeController.PublishAsync(index, collection, message);

            _api.Verify(new JObject{
                {"index" , index },
                {"collection" , collection },
                {"controller" , "realtime" },
                {"action" ,"publish" },
                {"body" , message },
            });
        }

        [Theory]
        [ClassData(typeof(SubscribeOptionGenerator))]
        public async void SubscribeAsyncTest(SubscribeOptions options)
        {
            string roomId = "A113";
            string channel = "a_channel";
            _api.SetResult(new JObject { {
                "result", new JObject {
                    { "roomId", roomId },
                    { "channel", channel } }
                } }
            );
            string index = "an_index";
            string collection = "a_collection";
            JObject filters = new JObject { { "studio", "pixar" } };
            JObject expectedQuery = new JObject {
                {"index", index},
                {"collection",collection},
                {"controller", "realtime"},
                {"action", "subscribe"},
                {"body", filters}
            };
            if (options != null)
            {
                expectedQuery.Merge(JObject.FromObject(options));
            }

            string res = await _realtimeController.SubscribeAsync(index, collection, filters, MockNotificationHandler, options);

            _api.Verify(expectedQuery);
            Assert.Equal(roomId, res);
        }

        [Fact]
        public async void UnsubscribeAsyncTest()
        {
            //First we need to subscribe to "a_collection"
            string index = "an_index";
            string collection = "a_collection";
            JObject filters = new JObject { { "studio", "pixar" } };
            string roomId = "A113";
            string channel = "a_channel";
            _api.SetResult(new JObject { {
                "result", new JObject {
                    { "roomId", roomId },
                    { "channel", channel } }
                } }
            );
            await _realtimeController.SubscribeAsync(index, collection, filters, MockNotificationHandler);

            //Then we can test if unsubcription is working
            _api.SetResult(new JObject {{
                "result",  new JObject {{ "roomId", roomId }}
                }}
            );

            await _realtimeController.UnsubscribeAsync(roomId);

            _api.Verify(new JObject {
                {"controller", "realtime"},
                {"action", "unsubscribe"},
                {"body", new JObject {{ "roomId", roomId}}}
            });
        }
    }

    public class SubscribeOptionGenerator : TheoryData<SubscribeOptions>
    {
        public SubscribeOptionGenerator()
        {
            SubscribeOptions s = new SubscribeOptions();
            Add(s);

            SubscribeOptions s1 = new SubscribeOptions();
            s1.Volatile = new JObject { { "reason", "test purpose" } };
            Add(s1);

            Add(null);
        }
    }
}