namespace Musica.Models
{
    public class RecentTrackModel
    {
        public int TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = "/Images/userDefault.png";
        public DateTime ReleaseDate { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public int ArtistProfileId { get; set; }
        public long PlayCount { get; set; }
    }

    public class TimeSinceLastTrackModel
    {
        public int TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string TimeSinceRelease { get; set; } = string.Empty;
        public int DaysSinceRelease { get; set; }
        public int HoursSinceRelease { get; set; }
        public int MinutesSinceRelease { get; set; }
        public int SecondsSinceRelease { get; set; }
        public bool HasTracks { get; set; }
    }
}