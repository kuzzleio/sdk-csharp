#!/bin/bash

set -e

NUSPEC_FILE="${TRAVIS_BUILD_DIR}/Kuzzle/Kuzzle.nuspec"
NUPKG_FILE="${TRAVIS_BUILD_DIR}/kuzzlesdk.nupkg"

# current distributions have nuget v4 only but we need at least a v5 to
# match the latest nuspec specifications
nuget update

nuget pack ${NUSPEC_FILE} -OutputDirectory ${TRAVIS_BUILD_DIR} -OutputFileNamesWithoutVersion

nuget push ${NUPKG_FILE} -ApiKey ${NUGET_API_KEY} -Source nuget.org
