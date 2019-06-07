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
sudo nuget update -self

# And now, we need to clear the NuGet cache created by the update command
# above (see https://github.com/NuGet/Home/issues/6537)
sudo chmod -R 777 /tmp/NuGetScratch/*

nuget pack ${NUSPEC_FILE} -OutputDirectory ${TRAVIS_BUILD_DIR} -OutputFileNamesWithoutVersion

nuget push ${NUPKG_FILE} -ApiKey ${NUGET_API_KEY} -Source nuget.org
