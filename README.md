# LazyFetcher

Download [LazyMan](https://github.com/StevensNJD4/LazyMan) streams from command line. 

* Choose a stream to download from list of feeds
* Download the latest game of given team
* Get the stream URL to be used by a video player

The application is still very basic with fixed settings. See also similar (Rust made) application, [LazyStream](https://github.com/tarkah/lazystream), which is more configurable, but doesn't support downloading yet. 

## Requirements 
* OS: Windows (x86/x64)/ MacOS / Linux (x64/ARM)
* .NET Core 3.0 runtime (and SDK if also building)
* [Streamlink](https://github.com/streamlink/streamlink)
* [go-mlbam-proxy](https://github.com/jwallet/go-mlbam-proxy)

## Installation
You can install the [Streamlink](https://github.com/streamlink/streamlink) according to its installation instructions. [go-mlbam-proxy](https://github.com/jwallet/go-mlbam-proxy) executable (mlbamproxy.exe) can also be copied from LazyMan installation. Both Streamlink and and go-mlbam-proxy must be either in same directory with LazyFetch executable, or in directory specified by PATH environment variable.

## Usage
Get latest game of your favorite team. It tries to get feed of chosen team (away/home) if available, otherwise it uses first feed found.
```
$./LazyFetcher -t CAR -p /mnt/download
LazyFetcher 1.0.0.0

Fetching latest feed for 'car'...
Feed found: 2019-12-12 CAR@VAN (away,FS-CR)

[cli][info] Found matching plugin hls for URL hlsvariant://http://foundurl.m3u8 name_key=bitrate verify=False
[cli][info] Available streams: 670k (worst), 1000k, 1450k, 2140k, 2900k, 4100k, 6600k (best)
[cli][info] Opening stream: 6600k (hls)
[download][..12-12-CAR@VAN-FS-CR.mp4] Written 235.5 MB (32s @ 6.5 MB/s) 
```
Choose the feed from list of found feeds
```
$./LazyFetcher -c
LazyFetcher 1.0.0.0

 1: 2019-12-12 NSH@BUF home (MSG-B)
 2: 2019-12-12 NSH@BUF away (FS-TN)
 3: 2019-12-12 BOS@TBL home (SUN)
 4: 2019-12-12 BOS@TBL away (NESN)
 5: 2019-12-12 NYI@FLA home (FS-F)
 6: 2019-12-12 NYI@FLA away (MSG+)
 7: 2019-12-12 CBJ@PIT home (ATT-PT)
 8: 2019-12-12 CBJ@PIT away (FSOH)
 9: 2019-12-12 WPG@DET home (FS-D+)
10: 2019-12-12 WPG@DET away (TSN3)
11: 2019-12-12 VGK@STL home (FSMW)
12: 2019-12-12 VGK@STL away (ATT-RM)
13: 2019-12-12 EDM@MIN home (FS-N)
14: 2019-12-12 EDM@MIN away (SNOL)
15: 2019-12-12 TOR@CGY home (SNW)
16: 2019-12-12 TOR@CGY away (TSN4)
17: 2019-12-12 CHI@ARI home (FSAZ)
18: 2019-12-12 CHI@ARI away (NBCS-CH)
19: 2019-12-12 CAR@VAN home (SNP)
20: 2019-12-12 CAR@VAN away (FS-CR)
21: 2019-12-12 LAK@ANA home (PRIME)
22: 2019-12-12 LAK@ANA away (FS-W)
23: 2019-12-12 NYR@SJS home (NBCS-CA)
24: 2019-12-12 NYR@SJS away (MSG)
25: 2019-12-13 VGK@DAL home (FSSW)
26: 2019-12-13 VGK@DAL away (ATT-RM)
27: 2019-12-13 NJD@COL home (ALT)
28: 2019-12-13 NJD@COL away (MSG+)

Choose feed (q to quit): 
```
## Releases
No CI set up yet. If you don't want build app yourself, you can contact me. I can then provide binaries for your platform.