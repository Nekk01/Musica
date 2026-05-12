using System;

namespace Musica;

public class PlaylistModel
{
    public int id { get; set;}
    public int owner_user_id { get; set; }
    public string name { get; set; }
    public DateTime created_at { get; set;}
}