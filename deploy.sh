#!/bin/bash

set -e

function getXmlValue {
  file=$1
  value=$2

  cat $file | grep "<${value}>" | perl -pe "s{.*<${value}>(.*)</${value}>.*}{\$1}"
}

function getNugetFilename {
  file=$1

  echo "$(getXmlValue $file id).$(getXmlValue $file version).nupkg"
}

NUSPEC_FILE="${TRAVIS_BUILD_DIR}/Kuzzle/Kuzzle.nuspec"
NUPKG_FILE="${TRAVIS_BUILD_DIR}/$(getNugetFilename ${NUSPEC_FILE})"

nuget pack ${NUSPEC_FILE} -OutputDirectory ${TRAVIS_BUILD_DIR}

nuget push ${NUPKG_FILE} -ApiKey ${NUGET_API_KEY} -Source nuget.org
