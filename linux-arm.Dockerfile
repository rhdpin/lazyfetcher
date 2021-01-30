# Dockerfile for building ARMv7 (like Raspberry Pi) based image
#
# No support for proxy now, so 'hosts' file addition is defined when running the container
# Build example: docker build -t lazyfetcher-linux-arm -f linux-arm.Dockerfile .
# Usage example: docker run -it --rm -v /host/dir:/container/dir --network=host --add-host targethostname:ipaddress lazyfetcher-linux-arm -c -p /container/dir
#                Replace 'targethostname' and 'ipaddress' with values you'd put to hosts file normally

FROM mcr.microsoft.com/dotnet/sdk:3.1-bionic-arm32v7 as build
WORKDIR /app

COPY src/*.csproj ./src/
WORKDIR /app/src
RUN dotnet restore

WORKDIR /app/
COPY src/. ./src/
WORKDIR /app/src
RUN dotnet publish -r linux-arm -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:3.1-bionic-arm32v7 as runtime
WORKDIR /app
RUN apt update && apt -y install streamlink
COPY --from=build /app/src/out ./
ENTRYPOINT ["dotnet", "LazyFetcher.dll"]