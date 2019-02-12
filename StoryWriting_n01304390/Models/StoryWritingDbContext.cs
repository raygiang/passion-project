using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StoryWriting_n01304390.Models
{
    public class StoryWritingDbContext : DbContext
    {
        // File used to represent the database we are working with
        public StoryWritingDbContext()
        {
        }

        public DbSet<Story> Stories { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<StoryContent> StoryContents { get; set; }
    }
}