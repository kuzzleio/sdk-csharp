#!/bin/bash

set -x

nuget pack $TRAVIS_BUILD_DIR/Kuzzle/Kuzzle.nuspec -OutputFileNamesWithoutVersion -OutputDirectory $TRAVIS_BUILD_DIR

nuget push kuzzlesdk.nupkg -ApiKey ${NUGET_API_KEY} -Source nuget.org
