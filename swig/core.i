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
%rename(User, match="class") user;
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

%ignore *::error;
%ignore *::status;
%ignore *::stack;

%feature("director") NotificationListenerClass;
%feature("director") EventListener;
%feature("director") SubscribeListener;

%{
#include "websocket.cpp"
#include "search_result.cpp"
#include "collection.cpp"
#include "auth.cpp"
#include "index.cpp"
#include "server.cpp"
#include "document.cpp"
#include "default_constructors.cpp"
#include <functional>
%}

%ignore getListener;
%ignore getListeners;

%{
#include "kuzzle.cpp"
#include "realtime.cpp"
#define SWIG_FILE_WITH_INIT
%}

%inline {
  namespace kuzzleio {
    class NotificationListenerClass {
      public:
        virtual void onMessage(kuzzleio::notification_result*) = 0;
        virtual ~NotificationListenerClass();
    };
  }
}

%extend kuzzleio::Realtime {  
  std::string subscribe(const std::string& index, const std::string& collection, const std::string& body, NotificationListenerClass* cb, room_options& options) {
    NotificationListener* listener = new std::function<void(kuzzleio::notification_result*)>([&](kuzzleio::notification_result* res) {
      cb->onMessage(res);
    });
    return $self->subscribe(index, collection, body, listener, options);
  }

  std::string subscribe(const std::string& index, const std::string& collection, const std::string& body, NotificationListenerClass* cb) {
    NotificationListener* listener = new std::function<void(kuzzleio::notification_result*)>([&](kuzzleio::notification_result* res) {
      cb->onMessage(res);
    });
    return $self->subscribe(index, collection, body, listener);
  }
}

%include "typemap.i"

%include "stl.i"
%include "kcore.i"

%extend options {
    options() {
        options *o = kuzzle_new_options();
        return o;
    }

    ~options() {
        free($self);
    }
}

%extend kuzzleio::kuzzle_response {
    ~kuzzle_response() {
        kuzzle_free_kuzzle_response($self);
    }
}


%include "websocket.cpp"
%include "kuzzle.cpp"
%include "collection.cpp"
%include "search_result.cpp"
%include "document.cpp"
%include "realtime.cpp"
%include "auth.cpp"
%include "index.cpp"
%include "server.cpp"
%include "default_constructors.cpp"
