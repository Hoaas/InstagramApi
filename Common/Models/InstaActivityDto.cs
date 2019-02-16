using System.Collections.Generic;

namespace Common.Models
{
    public class InstaActivityDto
    {
        public InstaUserDto User { get; set; }

        public string Text { get; set; }

        public List<string> Urls { get; set; } = new List<string>();
    }
}
