#!/bin/bash

set -e

BLUE='\033[0;34m'
LBLUE='\033[1;34m'
NC='\033[0m'

echo "Align all submodules to the same branch as the current repository."
echo "You can specify an other branch by passing it as an argument:"
echo ""
echo "By default, align submodule branches to this repository current branch name."
echo "You can also use a specific branch name instead, by passing it as an argument:"
echo "  ./align-submodules.sh 1-dev"
echo ""

if [ -z "$1" ]; then
  git_branch=$(git branch | grep \* | cut -d ' ' -f2)
else
  git_branch="$1"
fi

current_path=$(pwd)

sdk_paths="sdk-cpp sdk-c go/src/github.com/kuzzleio/sdk-go"

echo -e "${BLUE}Align submodules on branch $git_branch $NC"

for sdk_path in $sdk_paths;
do
  echo -e "${LBLUE}Align $sdk_path $NC"
  cd $sdk_path
  git fetch origin $git_branch
  git checkout $git_branch
  git pull origin $git_branch
done

cd $pwd
