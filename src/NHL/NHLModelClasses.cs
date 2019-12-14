using System;
using System.Collections.Generic;

namespace LazyFetcher.NHL
{
    public class Status
    {
        public string AbstractGameState { get; set; }
        public string CodedGameState { get; set; }
        public string DetailedState { get; set; }
        public string StatusCode { get; set; }
        public bool StartTimeTBD { get; set; }
    }

    public class LeagueRecord
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ot { get; set; }
        public string Type { get; set; }
    }

    public class TimeZone
    {
        public string Id { get; set; }
        public int Offset { get; set; }
        public string Tz { get; set; }
    }

    public class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string City { get; set; }
        public TimeZone TimeZone { get; set; }
    }

    public class Division
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameShort { get; set; }
        public string Link { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Conference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class Franchise
    {
        public int FranchiseId { get; set; }
        public string TeamName { get; set; }
        public string Link { get; set; }
    }

    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public Venue Venue { get; set; }
        public string Abbreviation { get; set; }
        public string TeamName { get; set; }
        public string TocationName { get; set; }
        public string FirstYearOfPlay { get; set; }
        public Division Division { get; set; }
        public Conference Conference { get; set; }
        public Franchise Franchise { get; set; }
        public string ShortName { get; set; }
        public string OfficialSiteUrl { get; set; }
        public int FranchiseId { get; set; }
        public bool Active { get; set; }
    }

    public class Away
    {
        public LeagueRecord LeagueRecord { get; set; }
        public int Score { get; set; }
        public Team Team { get; set; }
    }

    public class LeagueRecord2
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ot { get; set; }
        public string Type { get; set; }
    }

    public class TimeZone2
    {
        public string Id { get; set; }
        public int Offset { get; set; }
        public string Tz { get; set; }
    }

    public class Venue2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string City { get; set; }
        public TimeZone2 TimeZone { get; set; }
    }

    public class Division2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameShort { get; set; }
        public string Link { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Conference2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class Franchise2
    {
        public int FranchiseId { get; set; }
        public string TeamName { get; set; }
        public string Link { get; set; }
    }

    public class Team2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public Venue2 Venue { get; set; }
        public string Abbreviation { get; set; }
        public string TeamName { get; set; }
        public string LocationName { get; set; }
        public string FirstYearOfPlay { get; set; }
        public Division2 Division { get; set; }
        public Conference2 Conference { get; set; }
        public Franchise2 Franchise { get; set; }
        public string ShortName { get; set; }
        public string OfficialSiteUrl { get; set; }
        public int FranchiseId { get; set; }
        public bool Active { get; set; }
    }

    public class Home
    {
        public LeagueRecord2 LeagueRecord { get; set; }
        public int Score { get; set; }
        public Team2 Team { get; set; }
    }

    public class Teams
    {
        public Away Away { get; set; }
        public Home Home { get; set; }
    }

    public class Home2
    {
        public int Goals { get; set; }
        public int ShotsOnGoal { get; set; }
        public string RinkSide { get; set; }
    }

    public class Away2
    {
        public int Goals { get; set; }
        public int ShotsOnGoal { get; set; }
        public string RinkSide { get; set; }
    }

    public class Period
    {
        public string PeriodType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Num { get; set; }
        public string OrdinalNum { get; set; }
        public Home2 Home { get; set; }
        public Away2 Away { get; set; }
    }

    public class Away3
    {
        public int Scores { get; set; }
        public int Attempts { get; set; }
    }

    public class Home3
    {
        public int Scores { get; set; }
        public int Attempts { get; set; }
    }

    public class ShootoutInfo
    {
        public Away3 Away { get; set; }
        public Home3 Home { get; set; }
    }

    public class Team3
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class Home4
    {
        public Team3 Team { get; set; }
        public int Goals { get; set; }
        public int ShotsOnGoal { get; set; }
        public bool GoaliePulled { get; set; }
        public int NumSkaters { get; set; }
        public bool PowerPlay { get; set; }
    }

    public class Team4
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class Away4
    {
        public Team4 Team { get; set; }
        public int Goals { get; set; }
        public int ShotsOnGoal { get; set; }
        public bool GoaliePulled { get; set; }
        public int NumSkaters { get; set; }
        public bool PowerPlay { get; set; }
    }

    public class Teams2
    {
        public Home4 Home { get; set; }
        public Away4 Away { get; set; }
    }

    public class IntermissionInfo
    {
        public int IntermissionTimeRemaining { get; set; }
        public int IntermissionTimeElapsed { get; set; }
        public bool InIntermission { get; set; }
    }

    public class PowerPlayInfo
    {
        public int SituationTimeRemaining { get; set; }
        public int SituationTimeElapsed { get; set; }
        public bool InSituation { get; set; }
    }

    public class Linescore
    {
        public int CurrentPeriod { get; set; }
        public string CurrentPeriodOrdinal { get; set; }
        public string CurrentPeriodTimeRemaining { get; set; }
        public List<Period> Periods { get; set; }
        public ShootoutInfo ShootoutInfo { get; set; }
        public Teams2 Teams { get; set; }
        public string PowerPlayStrength { get; set; }
        public bool HasShootout { get; set; }
        public IntermissionInfo IntermissionInfo { get; set; }
        public PowerPlayInfo PowerPlayInfo { get; set; }
    }

    public class Venue3
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class Broadcast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Site { get; set; }
        public string Language { get; set; }
    }

    public class Editorial
    {
    }

    public class Epg
    {
        public string Title { get; set; }
        public string Platform { get; set; }
        public List<Feed1> Items { get; set; }
        public string TopicList { get; set; }
    }

    public class Feed1
    {
        public string Guid { get; set; }
        public string MediaState { get; set; }

        public string MediaPlaybackId { get; set; }

        public string MediaFeedType { get; set; }

        public string CallLetters { get; set; }

        public string EventId { get; set; }

        public string Language { get; set; }

        public bool FreeGame { get; set; }
        public string FeedName { get; set; }
        public bool GamePlus { get; set; }

    }

    public class Media
    {
        public List<Epg> Epg { get; set; }
    }

    public class Highlights
    {
    }

    public class Content
    {
        public string Link { get; set; }
        public Editorial Editorial { get; set; }
        public Media Media { get; set; }
        public Highlights Highlights { get; set; }
    }

    public class Game
    {
        public int GamePk { get; set; }
        public string Link { get; set; }
        public string GameType { get; set; }
        public string Season { get; set; }
        public DateTime GameDate { get; set; }
        public Status Status { get; set; }
        public Teams Teams { get; set; }
        public Linescore Linescore { get; set; }
        public Venue3 Venue { get; set; }
        public List<Broadcast> Broadcasts { get; set; }
        public Content Content { get; set; }
    }

    public class DateObj
    {
        public string Date { get; set; }
        public int TotalItems { get; set; }
        public int TotalEvents { get; set; }
        public int TotalGames { get; set; }
        public int TotalMatches { get; set; }
        public List<Game> Games { get; set; }
        public List<object> Events { get; set; }
        public List<object> Matches { get; set; }
    }

    public class RootObject
    {
        public string Copyright { get; set; }
        public int TotalItems { get; set; }
        public int TotalEvents { get; set; }
        public int TotalGames { get; set; }
        public int TotalMatches { get; set; }
        public int Wait { get; set; }
        public List<DateObj> Dates { get; set; }
    }
}
