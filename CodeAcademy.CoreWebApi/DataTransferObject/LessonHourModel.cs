using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataTransferObject
{
    public class LessonHourModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        [Required]
        [Range(0,23)]
        public byte BeginHour { get; set; }

        [Required]
        [Range(0,59)]
        public byte BeginMinute { get; set; }

        [Required]
        [Range(0,23)]
        public byte EndHour { get; set; }

        [Required]
        [Range(0,59)]
        public byte EndMinute { get; set; }
    }
}
