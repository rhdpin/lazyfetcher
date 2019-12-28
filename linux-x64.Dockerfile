# Dockerfile for building x64 based Linux image
#
# No support for proxy now, so 'hosts' file addition is defined when running the container
# Build example: docker build -t lazyfetcher-linux-x64 -f linux-x64.Dockerfile .
# Usage example: docker run -it --rm -v /host/dir:/container/dir --add-host targethostname:ipaddress lazyfetcher-linux-x64 -c -p /container/dir
#                Replace 'targethostname' and 'ipaddress' with values you'd put to hosts file normally

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-bionic AS build
WORKDIR /app

COPY src/*.csproj ./src/
WORKDIR /app/src
RUN dotnet restore

WORKDIR /app/
COPY src/. ./src/
WORKDIR /app/src
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:3.0-alpine AS runtime
WORKDIR /app
RUN apk add --update \
    python3 \    
	python3-dev \
    py3-pip \
    gcc musl-dev --no-cache \ 
	&& pip3 install streamlink \ 
	&& apk del gcc musl-dev --no-cache \ 
	&& rm -Rf /tmp/* 
COPY --from=build /app/src/out ./
ENTRYPOINT ["dotnet", "LazyFetcher.dll"]