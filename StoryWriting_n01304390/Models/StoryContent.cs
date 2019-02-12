using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StoryWriting_n01304390.Models
{
    // The model for a single story content
    // Has a story content id and the content
    // Links to exactly one writer and one story
    public class StoryContent
    {
        [Key]
        public int StoryContentID { get; set; }

        [Required, StringLength(255), Display(Name = "Story Content")]
        public string Content { get; set; }

        public virtual Writer ContentWriter { get; set; }

        public virtual Story Story { get; set; }
    }
}