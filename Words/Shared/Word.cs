using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Words.Shared
{
    public class Word
    {
        public int Id { get; set; }

        [Required]
        public string Term { get; set; } = string.Empty;

        [Required]
        public string Definition { get; set; } = string.Empty;
    }
}
