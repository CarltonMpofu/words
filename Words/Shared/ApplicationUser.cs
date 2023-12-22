using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Words.Shared
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }


        // Navigation property for user's words
        [JsonIgnore]
        public List<UserWord> UserWords { get; set; }
    }
}
