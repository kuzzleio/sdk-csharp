namespace KuzzleSdk.Protocol {
  /// <summary>
  /// Network protocol state.
  /// </summary>
  public enum ProtocolState {
    /// <summary>
    /// The network protocol accepts new requests.
    /// </summary>
    Open,

    /// <summary>
    /// The network protocol accepts new requests.
    /// Those are queued until it reconnects.
    /// </summary>
    Reconnecting,

    /// <summary>
    /// The network protocol does not accept requests.
    /// </summary>
    Closed
  }
}
