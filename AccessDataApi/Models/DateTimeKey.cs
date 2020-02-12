using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccessDataApi.Models
{
    public class DateTimeKey
    {
        [Key]
        public string DateTimeKeyId { get; set; }

        [Required]
        public DateTime date { get; set; }

        public virtual ICollection<BookApp> bookApps { get; set; }

        public DateTimeKey()
        {
            bookApps = new List<BookApp>();
        }
    }
}
