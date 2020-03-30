using System;
namespace KuzzleSdk.Enums.CollectionController {
  /// <summary>
  /// Controls the kind of collections to be returned by collection:list
  /// </summary>
  public enum TypeFilter {
    /// <summary>
    /// Return all collections (stored and real-time)
    /// </summary>
    All,
    /// <summary>
    /// Return only stored collections
    /// </summary>
    Stored,
    /// <summary>
    /// Return only real-time collections
    /// </summary>
    Realtime,
  }
}
