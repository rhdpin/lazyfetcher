FROM mcr.microsoft.com/dotnet/core/sdk:3.0-bionic AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY src/*.csproj ./src/
WORKDIR /app/src
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY src/. ./src/
WORKDIR /app/src
RUN dotnet publish -c Release -o out

# test application -- see: dotnet-docker-unit-testing.md
#FROM build AS testrunner
#WORKDIR /app/tests
#COPY tests/. .
#ENTRYPOINT ["dotnet", "test", "--logger:trx"]

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