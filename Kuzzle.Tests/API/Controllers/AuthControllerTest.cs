using Xunit;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using KuzzleSdk.Exceptions;
using Moq;
using System;
using System.Collections.Generic;

namespace Kuzzle.Tests.API.Controllers {
  public class AuthControllerTest {
    private readonly AuthController _authController;
    private readonly KuzzleApiMock _api;

    public static IEnumerable<object[]> GenerateTimeSpans() {
      yield return new object[] { null };
      yield return new object[] { new TimeSpan(1, 2, 3) };
    }

    public AuthControllerTest() {
      _api = new KuzzleApiMock();
      _authController = new AuthController(_api.MockedObject);
    }

    [Fact]
    public async void CheckTokenAsyncTest() {
      string token = "foobar";

      _api.SetResult(@"{result: {foo: 123}}");

      JObject result = await _authController.CheckTokenAsync(token);

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "checkToken" },
        { "body", new JObject{ { "token", token } } }
      });

      Assert.Equal<JObject>(
        new JObject { { "foo", 123 } },
        result,
        new JTokenEqualityComparer());

      _api.Mock.VerifySet(
        a => a.AuthenticationToken = It.IsAny<string>(),
        Times.Never);
    }

    [Fact]
    public async void CreateMyCredentialsAsyncTest() {
      var credentials = new JObject { { "credentials", "content" } };
      _api.SetResult(@"{result: {foo: 123}}");

      JObject result = await _authController.CreateMyCredentialsAsync(
        "foostrategy",
        credentials);

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "createMyCredentials" },
        { "strategy", "foostrategy"},
        { "body", credentials }
      });

      Assert.Equal<JObject>(
        new JObject { { "foo", 123 } },
        result,
        new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void CredentialsExistAsyncTest(bool value) {
      _api.SetResult(new JObject { { "result", value } });

      bool result = await _authController.CredentialsExistAsync("foostrategy");

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "credentialsExist" },
        { "strategy", "foostrategy"}
      });

      Assert.Equal(result, value);
    }

    [Fact]
    public async void DeleteMyCredentialsAsyncTest() {
      _api.SetResult(@"{result: {foo: 123}}");

      await _authController.DeleteMyCredentialsAsync("foo");

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "deleteMyCredentials" },
        { "strategy", "foo"}
      });
    }

    [Fact]
    public async void GetCurrentUserAsyncTest() {
      _api.SetResult(@"{result: {foo: 123}}");

      JObject result = await _authController.GetCurrentUserAsync();

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "getCurrentUser" }
      });

      Assert.Equal<JObject>(
        new JObject { { "foo", 123 } },
        result,
        new JTokenEqualityComparer());
    }

    [Fact]
    public async void GetMyCredentialsAsyncTest() {
      _api.SetResult(@"{result: {foo: 123}}");

      JObject result = await _authController.GetMyCredentialsAsync("foo");

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "getMyCredentials" },
        { "strategy", "foo" }
      });

      Assert.Equal<JObject>(
        new JObject { { "foo", 123 } },
        result,
        new JTokenEqualityComparer());
    }

    [Fact]
    public async void GetMyRightsAsyncTest() {
      JArray expected = JArray.Parse("['foo', 'bar', 'baz']");
      _api.SetResult(new JObject {
        { "result", new JObject {
          { "hits", expected } } } });

      JArray result = await _authController.GetMyRightsAsync();

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "getMyRights" }
      });

      Assert.Equal<JArray>(
        expected,
        result,
        new JTokenEqualityComparer());
    }

    [Fact]
    public async void GetStrategiesAsyncTest() {
      JArray expected = JArray.Parse("['foo', 'bar', 'baz']");
      _api.SetResult(new JObject { { "result", expected } });

      JArray result = await _authController.GetStrategiesAsync();

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "getStrategies" }
      });

      Assert.Equal<JArray>(
        expected,
        result,
        new JTokenEqualityComparer());
    }

    [Theory]
    [
      MemberData(
        nameof(AuthControllerTest.GenerateTimeSpans),
        MemberType = typeof(AuthControllerTest))
    ]
    public async void LoginAsyncTestSuccess(TimeSpan? expiresIn) {
      var credentials = new JObject { { "fake", "credentials" } };
      var expected = JObject.Parse(@"{
        _id: '<kuid>',
        jwt: '<encrypted token>',
        expiresAt: 1321085955000,
        ttl: 360000
      }");

      _api.SetResult(new JObject { { "result", expected } });

      JObject result = await _authController.LoginAsync(
        "foostrategy",
        credentials,
        expiresIn);

      var expectedQuery = new JObject {
        { "controller", "auth" },
        { "action", "login" },
        { "strategy", "foostrategy" },
        { "body", credentials },
      };

      if (expiresIn != null) {
        expectedQuery["expiresIn"] = $"{Math.Floor((double)expiresIn?.TotalSeconds)}s";
      }

      _api.Verify(expectedQuery);

      Assert.Equal<JObject>(
        expected,
        result,
        new JTokenEqualityComparer());

      _api.Mock.VerifySet(a => a.AuthenticationToken = "<encrypted token>");
    }

    [Fact]
    public async void LoginAsyncTestFailure() {
      _api.SetError();

      await Assert.ThrowsAsync<ApiErrorException>(
        () => _authController.LoginAsync("foostrategy", new JObject()));

      _api.Mock.VerifySet(
        a => a.AuthenticationToken = It.IsAny<string>(),
        Times.Never);
    }

    [Fact]
    public async void LogoutAsyncTest() {
      _api.SetResult(new JObject());

      await _authController.LogoutAsync();

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "logout" }
      });
    }

    [Theory]
    [
      MemberData(
        nameof(AuthControllerTest.GenerateTimeSpans),
        MemberType = typeof(AuthControllerTest))
    ]
    public async void RefreshTokenTestSuccess(TimeSpan? expiresIn) {
      var expected = JObject.Parse(@"{
        _id: '<kuid>',
        jwt: '<encrypted token>',
        expiresAt: 1321085955000,
        ttl: 360000
      }");

      _api.SetResult(new JObject { { "result", expected } });

      JObject result = await _authController.RefreshTokenAsync(expiresIn);

      var expectedQuery = new JObject {
        { "controller", "auth" },
        { "action", "refreshToken" },
      };

      if (expiresIn != null) {
        expectedQuery["expiresIn"] = $"{Math.Floor((double)expiresIn?.TotalSeconds)}s";
      }

      _api.Verify(expectedQuery);

      Assert.Equal<JObject>(
        expected,
        result,
        new JTokenEqualityComparer());

      _api.Mock.VerifySet(a => a.AuthenticationToken = "<encrypted token>");
    }

    [Fact]
    public async void RefreshTokenTestFailure() {
      _api.SetError();

      await Assert.ThrowsAsync<ApiErrorException>(
        () => _authController.RefreshTokenAsync());

      _api.Mock.VerifySet(
        a => a.AuthenticationToken = It.IsAny<string>(),
        Times.Never());
    }

    [Fact]
    public async void UpdateMyCredentialsAsyncTest() {
      var credentials = new JObject { { "credentials", "content" } };
      _api.SetResult(@"{result: {foo: 123}}");

      JObject result = await _authController.UpdateMyCredentialsAsync(
        "foostrategy",
        credentials);

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "updateMyCredentials" },
        { "strategy", "foostrategy"},
        { "body", credentials }
      });

      Assert.Equal<JObject>(
        new JObject { { "foo", 123 } },
        result,
        new JTokenEqualityComparer());
    }

    [Fact]
    public async void UpdateSelfAsyncTest() {
      var content = new JObject { { "user", "content" } };
      _api.SetResult(@"{result: {foo: 123}}");

      JObject result = await _authController.UpdateSelfAsync(content);

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "updateSelf" },
        { "body", content }
      });

      Assert.Equal<JObject>(
        new JObject { { "foo", 123 } },
        result,
        new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void ValidateMyCredentialsAsyncTest(bool value) {
      var credentials = new JObject { { "credentials", "content" } };
      _api.SetResult(new JObject { { "result", value } });

      bool result = await _authController.ValidateMyCredentialsAsync(
        "foostrategy",
        credentials);

      _api.Verify(new JObject {
        { "controller", "auth" },
        { "action", "validateMyCredentials" },
        { "strategy", "foostrategy" },
        { "body", credentials }
      });

      Assert.Equal(value, result);
    }
  }
}
