try {
  string id = await kuzzle.Document.DeleteAsync(
    "nyc-open-data",
    "yellow-taxi",
    "some-id");

  Console.WriteLine($"Document {id} successfully deleted");
} catch (KuzzleException e) {
  Console.Error.WriteLine(e);
}
