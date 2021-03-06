﻿using Xunit;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;

namespace Kuzzle.Tests.API.Controllers {
  public class AdminControllerTest {
    private readonly AdminController _adminController;
    private readonly KuzzleApiMock _api;

    public AdminControllerTest() {
      _api = new KuzzleApiMock();
      _adminController = new AdminController(_api.MockedObject);
    }


    [Fact]
    public async void DumpAsyncTestSuccess() {

      _api.SetResult(@"{result: {acknowledge: true}}");

      await _adminController.DumpAsync();

      _api.Verify(new JObject {
        {"controller", "admin"},
        {"action", "dump"}
      });

    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void LoadFixturesAsyncTestSuccess(bool waitForRefresh) {

      _api.SetResult(@"{result: {acknowledge: true}}");

      await _adminController.LoadFixturesAsync(
        JObject.Parse(@"{foo: [{create: {_id: 'bar'}}, {field: 'value', field2: true}]}"),
        waitForRefresh);

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "loadFixtures"},
        {"waitForRefresh", waitForRefresh},
        {"body", new JObject {
          {"index-name", new JObject{
            {"foo", new JArray {
              new JObject{{"create", new JObject {{ "_id", "bar" }} } },
              new JObject{{"field", "value"}, {"field2", true} }
            }}
          }}
        }}
      };

      _api.Verify(verifyQuery);

    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void LoadMappingsAsyncTestSuccess(bool waitForRefresh) {

      _api.SetResult(@"{result: {acknowledge: true}}");

      await _adminController.LoadMappingsAsync(
        JObject.Parse(@"{foo: {properties: {field1: {}, field2: {}}}}"),
        waitForRefresh);

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "loadMappings"},
        {"waitForRefresh", waitForRefresh},
        {"body", new JObject {
          {"index-name", new JObject{
            {"foo", new JObject {
              {"properties", new JObject{
                {"field1", new JObject{ }},
                {"field2", new JObject{ }}
              }}
            }}
          }}
        }}
      };

      _api.Verify(verifyQuery);

    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void LoadSecuritiesAsyncTestSuccess(bool waitForRefresh) {

      _api.SetResult(@"{result: {acknowledge: true}}");

      await _adminController.LoadSecuritiesAsync(
        JObject.Parse(@"{roles: {foobar: {foo: 'bar', bar: 'foo'}}}"),
        waitForRefresh);

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "loadSecurities"},
        {"waitForRefresh", waitForRefresh},
        {"body", new JObject {
          {"roles", new JObject{
            {"foobar", new JObject {
              {"foo", "bar"},
              {"bar", "foo"}
            }}
          }}
        }}
      };

      _api.Verify(verifyQuery);

    }

    [Fact]
    public async void ResetCacheAsyncTestSuccess() {

      _api.SetResult(@"{result: {acknowledge: true}}");

      await _adminController.ResetCacheAsync("foobar");

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "resetCache"},
        {"database", "foobar"}
      };

      _api.Verify(verifyQuery);

    }

    [Fact]
    public async void ResetDatabaseAsyncTestSuccess() {

      _api.SetResult(@"{result: {acknowledge: true}}");

      await _adminController.ResetDatabaseAsync();

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "resetDatabase"},
      };

      _api.Verify(verifyQuery);

    }

    [Fact]
    public async void ResetKuzzleDataAsyncTestSuccess() {

      _api.SetResult(@"{result: {acknowledge: true}}");

      await _adminController.ResetKuzzleDataAsync();

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "resetKuzzleData"},
      };

      _api.Verify(verifyQuery);

    }

    [Fact]
    public async void ResetSecurityAsyncTestSuccess() {

      _api.SetResult(@"{result: {acknowledge: true}}");

      await _adminController.ResetSecurityAsync();

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "resetSecurity"},
      };

      _api.Verify(verifyQuery);

    }

    [Fact]
    public async void ShutdownAsyncTestSuccess() {

      _api.SetResult(@"{result: {acknowledge: true}}");

      await _adminController.ShutdownAsync();

      JObject verifyQuery = new JObject {
        {"controller", "admin"},
        {"action", "shutdown"},
      };

      _api.Verify(verifyQuery);

    }

  }
}
