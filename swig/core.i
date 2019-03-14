%include <std_shared_ptr.i>

%rename(TokenValidity) token_validity;
%rename(AckResponse) ack_response;
%rename(queueTTL) queue_ttl;
%rename(Options, match="class") options;
%rename(QueryOptions) s_query_options;
%rename(JsonObject) json_object;
%rename(JsonResult) json_result;
%rename(LoginResult) login_result;
%rename(BoolResult) bool_result;
%rename(Statistics) statistics;
%rename(AllStatisticsResult) all_statistics_result;
%rename(StatisticsResult) statistics_result;
%rename(CollectionsList) collection_entry;
%rename(CollectionsListResult) collection_entry_result;
%rename(StringArrayResult) string_array_result;
%rename(KuzzleResponse) kuzzle_response;
%rename(KuzzleRequest) kuzzle_request;
%rename(ShardsResult) shards_result;
%rename(DateResult) date_result;
%rename(UserData) user_data;
%rename(RoomOptions) room_options;
%rename(SearchFilters) search_filters;
%rename(NotificationResult) notification_result;
%rename(NotificationContent) notification_content;
%rename(NotificationListener) NotificationListenerClass;
%rename(SubscribeToSelf) subscribe_to_self;

%rename(Mapping, match="class") mapping;
%rename(_auth, match="class") auth;
%rename(_kuzzle, match="class") kuzzle;
%rename(_realtime, match="class") realtime;
%rename(_collection, match="class") collection;
%rename(_document, match="class") document;
%rename(_server, match="class") server;

%rename(delete) delete_;

%ignore s_options;
%ignore *::error;
%ignore *::status;
%ignore *::stack;

// Internal use: ignore it to prevent a warning
%ignore kuzzleio::User::operator=;

// Proper C# Exception overloads are located in exceptions.i
%ignore KuzzleException;
%ignore BadRequestException;
%ignore ForbiddenException;
%ignore GatewayTimeoutException;
%ignore InternalException;
%ignore NotFoundException;
%ignore PartialException;
%ignore PreconditionException;
%ignore ServiceUnavailableException;
%ignore SizeLimitException;
%ignore UnauthorizedException;

// do not wrap: used to communicate between the go, cgo and c++ layers
%ignore _c_emit_event;

%feature("director") EventListener;
%feature("director") SubscribeListener;
%feature("director") NotificationListenerClass;

%{
#include "websocket.cpp"
#include "search_result.cpp"
#include "user.cpp"
#include "user_right.cpp"

#include "collection.cpp"
#include "auth.cpp"
#include "index.cpp"
#include "server.cpp"
#include "document.cpp"
#include "default_constructors.cpp"
#include <functional>
%}

%shared_ptr(kuzzleio::notification_result);
%shared_ptr(kuzzleio::SearchResult);

%typemap(csdirectorin) std::shared_ptr<kuzzleio::notification_result>
  "new NotificationResult($iminput, true)"

%inline {
  namespace kuzzleio {
    class NotificationListenerClass {
      protected:
        NotificationListener * _listener;

      public:
        NotificationListener* listener() const noexcept {
         return _listener;
        }

        virtual void onMessage(
            std::shared_ptr<kuzzleio::notification_result>) = 0;

        NotificationListenerClass() :
          _listener(
            new std::function<void(std::shared_ptr<notification_result>)>(
              [this](std::shared_ptr<kuzzleio::notification_result> res) {
                this->onMessage(res);
            }))
          {}

        virtual ~NotificationListenerClass() {
          delete _listener;
        };
    };
  }
}

// Realtime controller extension
%csmethodmodifiers kuzzleio::Realtime::subscribe "private";
%csmethodmodifiers kuzzleio::Realtime::unsubscribe "private";
%rename("_subscribe", fullname=1) kuzzleio::Realtime::subscribe;
%rename("_unsubscribe", fullname=1) kuzzleio::Realtime::unsubscribe;

%extend kuzzleio::Realtime {
  std::string subscribe(
      const std::string& index,
      const std::string& collection,
      const std::string& body,
      NotificationListenerClass & cb,
      const room_options& options) {
    return $self->subscribe(index, collection, body, cb.listener(), options);
  }

  std::string subscribe(
      const std::string& index,
      const std::string& collection,
      const std::string& body,
      NotificationListenerClass & cb) {
    return $self->subscribe(index, collection, body, cb.listener());
  }
}

%typemap(cscode) kuzzleio::Realtime %{
  protected System.Collections.Concurrent.ConcurrentDictionary<
      string,
      System.Collections.Concurrent.ConcurrentBag<NotificationListener>
      > _listeners = new System.Collections.Concurrent.ConcurrentDictionary<
          string,
          System.Collections.Concurrent.ConcurrentBag<NotificationListener>
          >();

  internal void addListener(string roomId, NotificationListener listener) {
    if (!_listeners.ContainsKey(roomId)) {
      _listeners.TryAdd(
        roomId,
        new System.Collections.Concurrent.ConcurrentBag<NotificationListener>());
    }

    _listeners[roomId].Add(listener);
  }

  public string subscribe(string index, string collection, string body,
                           NotificationListener cb, room_options options) {
    string roomId = this._subscribe(index, collection, body, cb, options);

    this.addListener(roomId, cb);

    return roomId;
  }

  public string subscribe(string index, string collection, string body,
                           NotificationListener cb) {
    string roomId = this._subscribe(index, collection, body, cb);

    this.addListener(roomId, cb);

    return roomId;
  }

  public void unsubscribe(string roomId, QueryOptions options) {
    this._listeners.TryRemove(roomId, out _);
    this._unsubscribe(roomId, options);
  }

  public void unsubscribe(string roomId) {
    this._listeners.TryRemove(roomId, out _);
    this._unsubscribe(roomId);
  }
%}

// Make Kuzzle controllers permanent, allowing them to store data as they
// see fit
%immutable kuzzleio::Kuzzle::auth;
%immutable kuzzleio::Kuzzle::collection;
%immutable kuzzleio::Kuzzle::document;
%immutable kuzzleio::Kuzzle::index;
%immutable kuzzleio::Kuzzle::realtime;
%immutable kuzzleio::Kuzzle::server;

%rename(_native_auth) kuzzleio::Kuzzle::auth;
%rename(_native_collection) kuzzleio::Kuzzle::collection;
%rename(_native_document) kuzzleio::Kuzzle::document;
%rename(_native_index) kuzzleio::Kuzzle::index;
%rename(_native_realtime) kuzzleio::Kuzzle::realtime;
%rename(_native_server) kuzzleio::Kuzzle::server;

%csmethodmodifiers kuzzleio::Kuzzle::auth "protected";
%csmethodmodifiers kuzzleio::Kuzzle::collection "protected";
%csmethodmodifiers kuzzleio::Kuzzle::document "protected";
%csmethodmodifiers kuzzleio::Kuzzle::index "protected";
%csmethodmodifiers kuzzleio::Kuzzle::realtime "protected";
%csmethodmodifiers kuzzleio::Kuzzle::server "protected";

%typemap(cscode) kuzzleio::Kuzzle %{
  // prevent premature GC collection
  protected Protocol _protocol;

  // singletons
  protected Auth _auth = null;
  protected Collection _collection = null;
  protected Document _document = null;
  protected Index _index = null;
  protected Realtime _realtime = null;
  protected Server _server = null;

  public Auth auth {
    get {
      if (_auth == null) _auth = _native_auth;
      return _auth;
    }
  }

  public Collection collection {
    get {
      if (_collection == null) _collection = _native_collection;
      return _collection;
    }
  }

  public Document document {
    get {
      if (_document == null) _document = _native_document;
      return _document;
    }
  }

  public Index index {
    get {
      if (_index == null) _index = _native_index;
      return _index;
    }
  }

  public Realtime realtime {
    get {
      if (_realtime == null) _realtime = _native_realtime;
      return _realtime;
    }
  }

  public Server server {
    get {
      if (_server == null) _server = _native_server;
      return _server;
    }
  }
%}

// Add a C# reference to prevent premature garbage collection and resulting use
// of dangling C++ pointer.
%typemap(csconstruct, excode=SWIGEXCODE) Kuzzle %{: this($imcall, true)
  {$excode$directorconnect
    // Ensure that the GC doesn't collect any Protocol instance set from C#
    _protocol = protocol;
  }
%}


// Add a C# reference to prevent premature garbage collection and resulting use
// of dangling C++ pointer.
%typemap(cscode) kuzzleio::Kuzzle %{
  protected Protocol _protocol;
%}

%typemap(csconstruct, excode=SWIGEXCODE) Kuzzle %{: this($imcall, true)
  {$excode$directorconnect
    // Ensure that the GC doesn't collect any Protocol instance set from C#
    _protocol = protocol;
  }
%}


%{
#include "kuzzle.cpp"
#include "realtime.cpp"
#define SWIG_FILE_WITH_INIT
%}

%include "exceptions.i"
%include "stl.i"
%include "kcore.i"
%include "std_string.i"
%include "std_vector.i"
%include "typemaps.i"

%extend kuzzleio::kuzzle_response {
    ~kuzzle_response() {
        kuzzle_free_kuzzle_response($self);
    }
}

%include "options.cpp"
%include "event_emitter.cpp"
%include "websocket.cpp"
%include "kuzzle.cpp"
%include "search_result.cpp"
%include "default_constructors.cpp"
%include "user.cpp"
%include "user_right.cpp"

%include "collection.cpp"
%include "search_result.cpp"
%include "document.cpp"
%include "realtime.cpp"
%include "auth.cpp"
%include "index.cpp"
%include "server.cpp"
