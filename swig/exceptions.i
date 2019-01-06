%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t badRequestExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL BadRequestExceptionRegisterCallback(CSharpExceptionCallback_t badRequestCallback) {
    badRequestExceptionCallback = badRequestCallback;
  }

  static void SWIG_CSharpSetPendingExceptionBadRequest(const char *msg) {
    badRequestExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class BadRequestExceptionHelper {
    public delegate void BadRequestExceptionDelegate(string message);

    static BadRequestExceptionDelegate badRequestDelegate =
                                   new BadRequestExceptionDelegate(SetPendingBadRequestException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="BadRequestExceptionRegisterCallback")]
    public static extern
           void BadRequestExceptionRegisterCallback(BadRequestExceptionDelegate badRequestCallback);

    static void SetPendingBadRequestException(string message) {
      SWIGPendingException.Set(new BadRequestException(message));
    }

    static BadRequestExceptionHelper() {
      BadRequestExceptionRegisterCallback(badRequestDelegate);
    }
  }
  static BadRequestExceptionHelper badRequestExceptionHelper = new BadRequestExceptionHelper();
%}

%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t forbiddenExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL ForbiddenExceptionRegisterCallback(CSharpExceptionCallback_t forbiddenCallback) {
    forbiddenExceptionCallback = forbiddenCallback;
  }

  static void SWIG_CSharpSetPendingExceptionForbidden(const char *msg) {
    forbiddenExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class ForbiddenExceptionHelper {
    public delegate void ForbiddenExceptionDelegate(string message);

    static ForbiddenExceptionDelegate forbiddenDelegate =
                                   new ForbiddenExceptionDelegate(SetPendingForbiddenException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="ForbiddenExceptionRegisterCallback")]
    public static extern
           void ForbiddenExceptionRegisterCallback(ForbiddenExceptionDelegate forbiddenCallback);

    static void SetPendingForbiddenException(string message) {
      SWIGPendingException.Set(new ForbiddenException(message));
    }

    static ForbiddenExceptionHelper() {
      ForbiddenExceptionRegisterCallback(forbiddenDelegate);
    }
  }
  static ForbiddenExceptionHelper forbiddenExceptionHelper = new ForbiddenExceptionHelper();
%}

%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t gatewayTimeoutExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL GatewayTimeoutExceptionRegisterCallback(CSharpExceptionCallback_t gatewayTimeoutCallback) {
    gatewayTimeoutExceptionCallback = gatewayTimeoutCallback;
  }

  static void SWIG_CSharpSetPendingExceptionGatewayTimeout(const char *msg) {
    gatewayTimeoutExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class GatewayTimeoutExceptionHelper {
    public delegate void GatewayTimeoutExceptionDelegate(string message);

    static GatewayTimeoutExceptionDelegate gatewayTimeoutDelegate =
                                   new GatewayTimeoutExceptionDelegate(SetPendingGatewayTimeoutException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="GatewayTimeoutExceptionRegisterCallback")]
    public static extern
           void GatewayTimeoutExceptionRegisterCallback(GatewayTimeoutExceptionDelegate gatewayTimeoutCallback);

    static void SetPendingGatewayTimeoutException(string message) {
      SWIGPendingException.Set(new GatewayTimeoutException(message));
    }

    static GatewayTimeoutExceptionHelper() {
      GatewayTimeoutExceptionRegisterCallback(gatewayTimeoutDelegate);
    }
  }
  static GatewayTimeoutExceptionHelper gatewayTimeoutExceptionHelper = new GatewayTimeoutExceptionHelper();
%}

%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t internalExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL InternalExceptionRegisterCallback(CSharpExceptionCallback_t internalCallback) {
    internalExceptionCallback = internalCallback;
  }

  static void SWIG_CSharpSetPendingExceptionInternal(const char *msg) {
    internalExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class InternalExceptionHelper {
    public delegate void InternalExceptionDelegate(string message);

    static InternalExceptionDelegate internalDelegate =
                                   new InternalExceptionDelegate(SetPendingInternalException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="InternalExceptionRegisterCallback")]
    public static extern
           void InternalExceptionRegisterCallback(InternalExceptionDelegate internalCallback);

    static void SetPendingInternalException(string message) {
      SWIGPendingException.Set(new InternalException(message));
    }

    static InternalExceptionHelper() {
      InternalExceptionRegisterCallback(internalDelegate);
    }
  }
  static InternalExceptionHelper internalExceptionHelper = new InternalExceptionHelper();
%}

%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t notFoundExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL NotFoundExceptionRegisterCallback(CSharpExceptionCallback_t notFoundCallback) {
    notFoundExceptionCallback = notFoundCallback;
  }

  static void SWIG_CSharpSetPendingExceptionNotFound(const char *msg) {
    notFoundExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class NotFoundExceptionHelper {
    public delegate void NotFoundExceptionDelegate(string message);

    static NotFoundExceptionDelegate notFoundDelegate =
                                   new NotFoundExceptionDelegate(SetPendingNotFoundException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="NotFoundExceptionRegisterCallback")]
    public static extern
           void NotFoundExceptionRegisterCallback(NotFoundExceptionDelegate notFoundCallback);

    static void SetPendingNotFoundException(string message) {
      SWIGPendingException.Set(new NotFoundException(message));
    }

    static NotFoundExceptionHelper() {
      NotFoundExceptionRegisterCallback(notFoundDelegate);
    }
  }
  static NotFoundExceptionHelper notFoundExceptionHelper = new NotFoundExceptionHelper();
%}

%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t partialExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL PartialExceptionRegisterCallback(CSharpExceptionCallback_t partialCallback) {
    partialExceptionCallback = partialCallback;
  }

  static void SWIG_CSharpSetPendingExceptionPartial(const char *msg) {
    partialExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class PartialExceptionHelper {
    public delegate void PartialExceptionDelegate(string message);

    static PartialExceptionDelegate partialDelegate =
                                   new PartialExceptionDelegate(SetPendingPartialException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="PartialExceptionRegisterCallback")]
    public static extern
           void PartialExceptionRegisterCallback(PartialExceptionDelegate partialCallback);

    static void SetPendingPartialException(string message) {
      SWIGPendingException.Set(new PartialException(message));
    }

    static PartialExceptionHelper() {
      PartialExceptionRegisterCallback(partialDelegate);
    }
  }
  static PartialExceptionHelper partialExceptionHelper = new PartialExceptionHelper();
%}
%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t preconditionExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL PreconditionExceptionRegisterCallback(CSharpExceptionCallback_t preconditionCallback) {
    preconditionExceptionCallback = preconditionCallback;
  }

  static void SWIG_CSharpSetPendingExceptionPrecondition(const char *msg) {
    preconditionExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class PreconditionExceptionHelper {
    public delegate void PreconditionExceptionDelegate(string message);

    static PreconditionExceptionDelegate preconditionDelegate =
                                   new PreconditionExceptionDelegate(SetPendingPreconditionException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="PreconditionExceptionRegisterCallback")]
    public static extern
           void PreconditionExceptionRegisterCallback(PreconditionExceptionDelegate preconditionCallback);

    static void SetPendingPreconditionException(string message) {
      SWIGPendingException.Set(new PreconditionException(message));
    }

    static PreconditionExceptionHelper() {
      PreconditionExceptionRegisterCallback(preconditionDelegate);
    }
  }
  static PreconditionExceptionHelper preconditionExceptionHelper = new PreconditionExceptionHelper();
%}

%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t serviceUnavailableExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL ServiceUnavailableExceptionRegisterCallback(CSharpExceptionCallback_t serviceUnavailableCallback) {
    serviceUnavailableExceptionCallback = serviceUnavailableCallback;
  }

  static void SWIG_CSharpSetPendingExceptionServiceUnavailable(const char *msg) {
    serviceUnavailableExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class ServiceUnavailableExceptionHelper {
    public delegate void ServiceUnavailableExceptionDelegate(string message);

    static ServiceUnavailableExceptionDelegate serviceUnavailableDelegate =
                                   new ServiceUnavailableExceptionDelegate(SetPendingServiceUnavailableException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="ServiceUnavailableExceptionRegisterCallback")]
    public static extern
           void ServiceUnavailableExceptionRegisterCallback(ServiceUnavailableExceptionDelegate serviceUnavailableCallback);

    static void SetPendingServiceUnavailableException(string message) {
      SWIGPendingException.Set(new ServiceUnavailableException(message));
    }

    static ServiceUnavailableExceptionHelper() {
      ServiceUnavailableExceptionRegisterCallback(serviceUnavailableDelegate);
    }
  }
  static ServiceUnavailableExceptionHelper serviceUnavailableExceptionHelper = new ServiceUnavailableExceptionHelper();
%}

%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t sizeLimitExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL SizeLimitExceptionRegisterCallback(CSharpExceptionCallback_t sizeLimitCallback) {
    sizeLimitExceptionCallback = sizeLimitCallback;
  }

  static void SWIG_CSharpSetPendingExceptionSizeLimit(const char *msg) {
    sizeLimitExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class SizeLimitExceptionHelper {
    public delegate void SizeLimitExceptionDelegate(string message);

    static SizeLimitExceptionDelegate sizeLimitDelegate =
                                   new SizeLimitExceptionDelegate(SetPendingSizeLimitException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="SizeLimitExceptionRegisterCallback")]
    public static extern
           void SizeLimitExceptionRegisterCallback(SizeLimitExceptionDelegate sizeLimitCallback);

    static void SetPendingSizeLimitException(string message) {
      SWIGPendingException.Set(new SizeLimitException(message));
    }

    static SizeLimitExceptionHelper() {
      SizeLimitExceptionRegisterCallback(sizeLimitDelegate);
    }
  }
  static SizeLimitExceptionHelper sizeLimitExceptionHelper = new SizeLimitExceptionHelper();
%}

%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t unauthorizedExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL UnauthorizedExceptionRegisterCallback(CSharpExceptionCallback_t unauthorizedCallback) {
    unauthorizedExceptionCallback = unauthorizedCallback;
  }

  static void SWIG_CSharpSetPendingExceptionUnauthorized(const char *msg) {
    unauthorizedExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class UnauthorizedExceptionHelper {
    public delegate void UnauthorizedExceptionDelegate(string message);

    static UnauthorizedExceptionDelegate unauthorizedDelegate =
                                   new UnauthorizedExceptionDelegate(SetPendingUnauthorizedException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="UnauthorizedExceptionRegisterCallback")]
    public static extern
           void UnauthorizedExceptionRegisterCallback(UnauthorizedExceptionDelegate unauthorizedCallback);

    static void SetPendingUnauthorizedException(string message) {
      SWIGPendingException.Set(new UnauthorizedException(message));
    }

    static UnauthorizedExceptionHelper() {
      UnauthorizedExceptionRegisterCallback(unauthorizedDelegate);
    }
  }
  static UnauthorizedExceptionHelper unauthorizedExceptionHelper = new UnauthorizedExceptionHelper();
%}

%insert(runtime) %{
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);

  CSharpExceptionCallback_t kuzzleExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL KuzzleExceptionRegisterCallback(CSharpExceptionCallback_t kuzzleCallback) {
    kuzzleExceptionCallback = kuzzleCallback;
  }

  static void SWIG_CSharpSetPendingExceptionKuzzle(const char *msg) {
    kuzzleExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class KuzzleExceptionHelper {
    public delegate void KuzzleExceptionDelegate(string message);

    static KuzzleExceptionDelegate kuzzleDelegate =
                                   new KuzzleExceptionDelegate(SetPendingKuzzleException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="KuzzleExceptionRegisterCallback")]
    public static extern
           void KuzzleExceptionRegisterCallback(KuzzleExceptionDelegate kuzzleCallback);

    static void SetPendingKuzzleException(string message) {
      SWIGPendingException.Set(new KuzzleException(message));
    }

    static KuzzleExceptionHelper() {
      KuzzleExceptionRegisterCallback(kuzzleDelegate);
    }
  }
  static KuzzleExceptionHelper kuzzleExceptionHelper = new KuzzleExceptionHelper();
%}

%exception {
  try {
    $action
  } catch (kuzzleio::BadRequestException e) {
    SWIG_CSharpSetPendingExceptionBadRequest(e.what());
  } catch (kuzzleio::ForbiddenException e) {
    SWIG_CSharpSetPendingExceptionForbidden(e.what());
  } catch (kuzzleio::InternalException e) {
    SWIG_CSharpSetPendingExceptionInternal(e.what());
  } catch (kuzzleio::NotFoundException e) {
    SWIG_CSharpSetPendingExceptionNotFound(e.what());
  } catch (kuzzleio::PartialException e) {
    SWIG_CSharpSetPendingExceptionPartial(e.what());
  } catch (kuzzleio::PreconditionException e) {
    SWIG_CSharpSetPendingExceptionPrecondition(e.what());
  } catch (kuzzleio::ServiceUnavailableException e) {
    SWIG_CSharpSetPendingExceptionServiceUnavailable(e.what());
  } catch (kuzzleio::SizeLimitException e) {
    SWIG_CSharpSetPendingExceptionSizeLimit(e.what());
  } catch (kuzzleio::UnauthorizedException e) {
    SWIG_CSharpSetPendingExceptionUnauthorized(e.what());
  } catch (kuzzleio::KuzzleException e) {
    SWIG_CSharpSetPendingExceptionKuzzle(e.what());
  }
}

%typemap(csbase) kuzzleio::KuzzleException "System.ApplicationException";
