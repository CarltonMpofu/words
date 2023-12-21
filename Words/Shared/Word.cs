using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Words.Shared
{
    public class Word
    {
        public int Id { get; set; }

        public string Term { get; set; } = string.Empty;


        public string Definition { get; set; } = string.Empty;
    }
}
