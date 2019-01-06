%typemap(throws, canthrow=1) kuzzleio::KuzzleException {
  SWIG_CSharpSetPendingExceptionArgument(SWIG_CSharpArgumentException, $1.what(), NULL);
  return $null;
}

%typemap(csbase) kuzzleio::KuzzleException "System.Exception";
