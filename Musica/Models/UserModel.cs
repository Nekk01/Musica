using System;

namespace Musica;

public class UserModel
{
        public string display_name{ get; set;}
        public string bio { get; set;}
        public string avatar_url { get; set;}
        public DateTime created_at { get; set;}
        public int role_id { get; set;}
}
