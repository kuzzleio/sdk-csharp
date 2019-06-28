FROM amd64/debian:stretch-slim

LABEL io.kuzzle.vendor="Kuzzle <support@kuzzle.io>"
LABEL description="Runner for csharp documentation snippets"
LABEL name="kuzzleio/doc-runner:csharp"

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

RUN apt-get update && apt-get install -y \
  make \
  pkg-config \
  build-essential \
  git \
  apt-transport-https \
  dirmngr \
  gnupg \
  ca-certificates \
  curl

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF \
  && echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | tee /etc/apt/sources.list.d/mono-official-stable.list \
  && apt-get update \
  && apt-get install -y mono-devel \
  && curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > /etc/apt/trusted.gpg.d/microsoft.asc.gpg \
  && curl -o /etc/apt/sources.list.d/microsoft-prod.list https://packages.microsoft.com/config/debian/9/prod.list \
  && apt-get update \
  && apt-get install -y dotnet-sdk-2.1 \
  && dotnet tool install -g dotnet-script

ENV PATH $PATH:/root/.dotnet/tools

WORKDIR /app
