using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoryWriting_n01304390.Models.ViewModels
{
    // This model is used to send information regarding multiple different types models to the view
    public class AddOrEditViewModel
    {
        public AddOrEditViewModel()
        {
        }

        public virtual Story Story { get; set; }

        public virtual StoryContent StoryContent { get; set; }

        public IEnumerable<Writer> Writers { get; set; }
    }
}