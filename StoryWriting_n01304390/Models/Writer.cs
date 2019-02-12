using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StoryWriting_n01304390.Models
{
    // The model for a single writer
    // Has a writer id, first name, last name, and username
    // Links to multiple stories and multiple story contents
    public class Writer
    {
        [Key]
        public int WriterID { get; set; }

        [Required, StringLength(255), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, StringLength(255), Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, StringLength(255), Display(Name = "Username")]
        public string Username { get; set; }

        public virtual ICollection<Story> Stories { get; set; }

        public virtual ICollection<StoryContent> StoryContents { get; set; }
    }
}