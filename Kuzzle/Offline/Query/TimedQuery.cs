using System;
using Newtonsoft.Json.Linq;

namespace KuzzleSdk {
  public class TimedQuery : IEquatable<TimedQuery>, IComparable<TimedQuery> {

    public string UUID { get; private set; }
    private JObject query;
    private Int64 time;

    public JObject Query {
      get { return query; }
      private set { query = value; }
    }

    public Int64 Time {
      get { return time; }
      private set { time = value; }
    }

    public TimedQuery(JObject query, string uuid = null) : this(query, DateTime.Now.Ticks, uuid) {
    }

    public TimedQuery(JObject query, Int64 time, string uuid = null) {
      Query = query;
      Time = time;
      UUID = uuid ?? Guid.NewGuid().ToString();
    }

    public override int GetHashCode() {
      return UUID.GetHashCode();
    }

    public override bool Equals(object obj) {
      if (obj == null) return false;
      TimedQuery timedQuery = obj as TimedQuery;
      if (timedQuery == null) return false;
      return Equals(timedQuery);
    }

    public bool Equals(TimedQuery other) {
      if (other == null) return false;
      return this.UUID.Equals(other.UUID);
    }

    public int CompareTo(TimedQuery other) {
      if (other == null)
        return 1;
      else
        return this.Time.CompareTo(other.Time);
    }

    public override string ToString() {
      return "{uuid: \""+ this.UUID + "\", time: " + this.Time + ", query: " + this.Query.ToString() + "}";
    }
  }
}
