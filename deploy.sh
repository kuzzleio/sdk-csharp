#!/bin/bash

set -ex

NUSPEC_FILE="${TRAVIS_BUILD_DIR}/Kuzzle/Kuzzle.nuspec"
NUPKG_FILE="${TRAVIS_BUILD_DIR}/kuzzlesdk.nupkg"

# Current distributions have nuget v4 only but we need at least a v5 to
# match the latest nuspec specifications
# But first we have some configuring to do to allow nuget to update itself
# through SSL
yes | sudo certmgr -ssl -m https://go.microsoft.com
yes | sudo certmgr -ssl -m https://nugetgallery.blob.core.windows.net
yes | sudo certmgr -ssl -m https://nuget.org
nuget update -self

nuget pack ${NUSPEC_FILE} -OutputDirectory ${TRAVIS_BUILD_DIR} -OutputFileNamesWithoutVersion

nuget push ${NUPKG_FILE} -ApiKey ${NUGET_API_KEY} -Source nuget.org
