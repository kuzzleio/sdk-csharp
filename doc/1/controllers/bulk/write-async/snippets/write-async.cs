try {
    JObject content = JObject.Parse(@"{
      kuzzle_info: {
        author: '<kuid>',
        createdAt: 1481816934209,
        updatedAt: null,
        updater: null,
        active: true,
        deletedAt: null
      }
    }");
    await kuzzle.Bulk.WriteAsync("nyc-open-data", "yellow-taxi", content);
    Console.WriteLine("Success");
} catch (Exception e) {
    Console.WriteLine(e);
}