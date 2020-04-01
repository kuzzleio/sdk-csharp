using System;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk {
  /// <summary>
  /// Implement time-limited queries
  /// </summary>
  public class TimedQuery : IEquatable<TimedQuery>, IComparable<TimedQuery> {
    /// <summary>
    /// Query unique ID
    /// </summary>
    public string UUID { get; private set; }

    private JObject query;
    private Int64 time;

    /// <summary>
    /// Kuzzle API query
    /// </summary>
    public JObject Query {
      get { return query; }
      private set { query = value; }
    }

    /// <summary>
    /// Query starting timestamp
    /// </summary>
    public Int64 Time {
      get { return time; }
      private set { time = value; }
    }

    /// <summary>
    /// Default constructor: set the starting time to now
    /// </summary>
    public TimedQuery(JObject query, string uuid = null)
      : this(query, DateTime.Now.Ticks, uuid)
    {
    }

    /// <summary>
    /// Constructor with a preset starting time
    /// </summary>
    public TimedQuery(JObject query, Int64 time, string uuid = null) {
      Query = query;
      Time = time;
      UUID = uuid ?? Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Returns this query unique ID hash code
    /// </summary>
    public override int GetHashCode() {
      return UUID.GetHashCode();
    }

    /// <summary>
    /// Generic timed-query equality comparator
    /// </summary>
    public override bool Equals(object obj) {
      if (obj == null) return false;
      TimedQuery timedQuery = obj as TimedQuery;
      if (timedQuery == null) return false;
      return Equals(timedQuery);
    }

    /// <summary>
    /// Timed query equality comparator
    /// </summary>
    public bool Equals(TimedQuery other) {
      if (other == null) return false;
      return this.UUID.Equals(other.UUID);
    }

    /// <summary>
    /// Compare queries starting timestamps
    /// </summary>
    public int CompareTo(TimedQuery other) {
      if (other == null)
        return 1;
      else
        return this.Time.CompareTo(other.Time);
    }

    /// <summary>
    /// Stringifier function
    /// </summary>
    public override string ToString() {
      return "{uuid: \""+ this.UUID + "\", time: " + this.Time + ", query: " + this.Query.ToString() + "}";
    }
  }
}
