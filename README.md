# LazyFetcher
[![Build Status](https://dev.azure.com/rhdpin/rhdpin/_apis/build/status/rhdpin.lazyfetcher?branchName=master)](https://dev.azure.com/rhdpin/rhdpin/_build/latest?definitionId=1&branchName=master)

A command-line interface for [LazyMan](https://github.com/StevensNJD4/LazyMan). 

* Choose a stream to download from list of feeds
* Download the latest game of given team
* Get the stream URL to be used by a video player

The application ([.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.0)) is basic with many fixed settings and supports only NHL feeds. See also similar (Rust made) application, [LazyStream](https://github.com/tarkah/lazystream). 

## Requirements 
* OS: Windows (x86/x64) / MacOS / Linux (x64/ARMv7)
* [Streamlink](https://github.com/streamlink/streamlink)
* Application expects that by default that hosts file has been configured for needed redirection. Alternatively a parameter can be used to use [go-mlbam-proxy](https://github.com/jwallet/go-mlbam-proxy) instead.

## Installation
### Method 1: Download released binaries and install dependencies
Download the compiled binary files from [Releases](https://github.com/rhdpin/lazyfetcher/releases). Then you can install the [Streamlink](https://github.com/streamlink/streamlink) according to its installation instructions. 

If hosts file is not edited, [go-mlbam-proxy](https://github.com/jwallet/go-mlbam-proxy) is needed. The executable (mlbamproxy.exe) can also be copied from LazyMan installation. Both Streamlink (and go-mlbam-proxy if needed) must be either in same directory with LazyFetch executable, or in directory specified by PATH environment variable.
### Method 2: Docker
Docker method is good because you get all done with one command. Currently the image size is big though (200-300MB). Currently it supports only 'hosts' editing approach, but the good thing is that it does not interfere with hosts file on your Docker host machine, but only the container.
1. Install Docker on your host
2. Run the container with command like: 
`docker run -it --rm -v /mnt/download:/app/download --network=host --add-host targethostname:hostipaddress rhdpin/lazyfetcher:linux-x64 -c -p /app/download`

Parameter `-v` binds a folder from host to container. In example command host folder is `/mnt/download` and it's mapped to `/app/download` in container. Parameter `--add-host` adds redirection to `hosts` file of the container, so replace `targethostname` and `hostipaddress` with same values which you use in `hosts` file with LazyMan. 

`rhdpin/lazyfetcher:linux-x64` is the image name to be used. Replace `linux-x64` with `linux-arm32v7` to get a image for ARMv7 device like Raspberry Pi. Rest of the parameters are for the LazyFetcher itself.

## Usage
```
$ ./LazyFetcher --help
LazyFetcher 1.0.5

  -c, --choose       Choose the feed from list of found feeds.

  -t, --team         Get latest game for team (three letter abbreviation. E.g. WPG).

  -b, --bitrate      (Default: best) Specify bitrate of stream to be downloaded. Use verbose mode to see available bitrates.

  -d, --days         (Default: 2) Specify how many days back to search games

  -e, --date         (Default: current date) Specify date to search games from (in format yyyy-MM-dd, e.g. 2019-12-22

  -l, --league       (Default: NHL) Set league. Only NHL supported currently.

  -o, --overwrite    Overwrite file if it already exists (instead of skipping the download).

  -p, --path         (Default: current directory) Set target download path.

  -u, --url          Get only URL of the stream but don't download.

  -v, --verbose      Use verbose mode to get more detailed output.

  -x, --use-proxy    Use proxy for redirection (required if 'hosts' file has not been edited).

  --help             Display this help screen.

  --version          Display version information.
```

Choose the feed from list of found feeds and download it using proxy instead of editing hosts file
```
$ ./LazyFetcher -x -c -p /mnt/download
LazyFetcher 1.0.5

 1: 2019-12-15 PHI@WPG home (TSN3)
 2: 2019-12-15 PHI@WPG away (NBCS-PH+)
 3: 2019-12-15 MIN@CHI home (NBCS-CH)
 4: 2019-12-15 MIN@CHI away (FS-N)
 5: 2019-12-15 LAK@DET home (FS-D)
 6: 2019-12-15 LAK@DET away (FS-W)
 7: 2019-12-15 VAN@VGK home (ATT-RM)
 8: 2019-12-15 VAN@VGK national (SN)
 9: 2019-12-16 OTT@FLA home (FS-F)
10: 2019-12-16 OTT@FLA away (TSN5)
11: 2019-12-16 NSH@NYR home (MSG)
12: 2019-12-16 NSH@NYR away (FS-TN)
13: 2019-12-16 WSH@CBJ home (FSOH)
14: 2019-12-16 WSH@CBJ away (NBCS-WA)
15: 2019-12-16 COL@STL home (FSMW)
16: 2019-12-16 COL@STL away (ALT)
17: 2019-12-16 EDM@DAL home (FSSW+)
18: 2019-12-16 EDM@DAL away (SNW)

Choose feed (q to quit): 11

Downloading feed: 2019-12-16 NSH@NYR (home,MSG)
Writing stream to file: 167 MB (11.8 MB/s)
```
Get latest game of your favorite team with hosts file edited. It tries to get feed of chosen team (away/home) if available, otherwise it uses first feed found.
```
$ ./LazyFetcher -t DAL -p /mnt/download
LazyFetcher 1.0.5

Fetching latest feed for 'DAL'...
Feed found: 2019-12-16 EDM@DAL (home,FSSW+)
Writing stream to file: 343 MB (8.2 MB/s)
```
## Releases
All release packages contain all needed files, so no additional installation of .NET Core runtime is needed. 

After extracting the files on Linux, run `chmod +x LazyFetcher` to make the program executable.