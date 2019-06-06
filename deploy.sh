#!/bin/bash

set -x

nuget pack Kuzzle/Kuzzle.nuspec -OutputFileNamesWithoutVersion

nuget push kuzzlesdk.nupkg -ApiKey ${NUGET_API_KEY} -Source nuget.org
