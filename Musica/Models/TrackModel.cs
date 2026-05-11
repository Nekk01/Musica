using System.ComponentModel.DataAnnotations;

namespace Musica;

public class TrackModel
{
    public int ID { get; set; }
    public string title { get; set; }
    public DateTime release_dateP { get; set; } = DateTime.Now;
    public int artist_profile_id { get; set; } 
    public string artist_name { get; set;}
    public int genre_id { get; set; } 
    public string file_url { get; set; }
    public string cover_url { get; set; }
}