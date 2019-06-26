using KuzzleSdk.API.Controllers;
using Xunit;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using KuzzleSdk.API.Options;
using KuzzleSdk.API;
using Moq;

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
        [
            MemberData(nameof(SubscribeOptionsData))
        ]
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
            Mock<RealtimeController.NotificationHandler> notificationHandlerMock = new Mock<RealtimeController.NotificationHandler>();

            string res = await _realtimeController.SubscribeAsync(index, collection, filters, notificationHandlerMock.Object, options);

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
            Mock<RealtimeController.NotificationHandler> notificationHandlerMock = new Mock<RealtimeController.NotificationHandler>();

            await _realtimeController.SubscribeAsync(index, collection, filters, notificationHandlerMock.Object);

            //Then we test that Notification Handler is not called after the unsubscription. 
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

            Response notif = Response.FromString("{room: 'a_channel'}");
            _api.Mock.Raise(m => m.UnhandledResponse += null, this, notif);
            notificationHandlerMock.Verify(m => m.Invoke(notif), Times.Never);
        }

        [Fact]
        public void NotificationHandlerTokenExpiredTest()
        {
            _api.Mock.Raise(m => m.UnhandledResponse += null, this, Response.FromString(@"{type: 'TokenExpired'}"));

            _api.Mock.Verify(m => m.DispatchTokenExpired(), Times.Once());
        }

        [Fact]
        public async void NotificationHandlerTest()
        {
            //First we subscribe to a collection
            string index = "an_index";
            string collection = "a_collection";
            JObject filters = new JObject { { "studio", "pixar" } };
            _api.SetResult(new JObject { {
                "result", new JObject {
                    { "roomId", "A113"},
                    { "channel", "a_channel"} }
                } }
            );
            Mock<RealtimeController.NotificationHandler> notificationHandlerMock = new Mock<RealtimeController.NotificationHandler>();
            await _realtimeController.SubscribeAsync(index, collection, filters, notificationHandlerMock.Object);

            //Then we trigger a notification
            Response notif = Response.FromString("{room: 'a_channel'}");
            _api.Mock.Raise(m => m.UnhandledResponse += null, this, notif);

            //Then we can check that the handler has been called
            notificationHandlerMock.Verify(m => m.Invoke(notif), Times.AtLeastOnce);
        }

        public static IEnumerable<object[]> SubscribeOptionsData()
        {
            yield return new object[] { null };
            yield return new object[] { new SubscribeOptions() };
            yield return new object[] {
                JsonConvert.DeserializeObject<SubscribeOptions>(@"{
                    scope: 'all',
                    users: 'all',
                    volatile: {
                        reason: 'test purpose'
                    }
                }")
            };
        }
    }
}