using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StoryWriting_n01304390.Models
{
    // The model for a single story
    // Has a story id, title, description, creator, a flag specifying whether or not it has a feature image, and the image type
    // A story can link to one creator but multiple story contents
    public class Story
    {
        [Key]
        public int StoryID { get; set; }

        [Required, StringLength(255), Display(Name = "Story Title")]
        public string StoryTitle { get; set; }

        [Required, StringLength(255), Display(Name = "Story Description")]
        public string StoryDescription { get; set; }

        public virtual Writer StoryCreator { get; set; }

        public virtual ICollection<StoryContent> StoryContents { get; set; }

        public bool HasFeatureImage { get; set; }

        public string ImageType { get; set; }
    }
}