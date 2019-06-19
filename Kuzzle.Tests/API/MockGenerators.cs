using System.Collections.Generic;
using KuzzleSdk.API.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kuzzle.Tests.API {
  public static class MockGenerators {
    public static IEnumerable<object[]> GenerateMappings() {
      yield return new object[] { null };
      yield return new object[] { new JObject { { "foo", "bar" } } };
    }

    public static IEnumerable<object[]> GenerateSearchOptions() {
      yield return new object[] { null };
      yield return new object[] {
        JsonConvert.DeserializeObject<SearchOptions>(@"{
          from: null,
          size: null,
          scroll: '1d'
        }")
      };
      yield return new object[] {
        JsonConvert.DeserializeObject<SearchOptions>(@"{
          from: -10,
          size: 42,
          type: '1d'
        }")
      };
      yield return new object[] {
        JsonConvert.DeserializeObject<SearchOptions>(@"{
          from: 12,
          size: null,
          type: null
        }")
      };
    }
  }
}
