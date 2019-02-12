using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using StoryWriting_n01304390.Models;
using StoryWriting_n01304390.Models.ViewModels;

namespace StoryWriting_n01304390.Controllers
{
    public class StoryContentController : Controller
    {
        private StoryWritingDbContext database = new StoryWritingDbContext();

        // Should be no default view for StoryContent, redirect to GetList view for Story
        public ActionResult Index()
        {
            return RedirectToAction("GetList", "Story");
        }

        // Sends a viewmodel to the AddContent view for StoryContent and returns the view
        // A viewmodel is used because the view needs information about both the story the content is being 
        // added to, and a list of all the writers in the database
        public ActionResult AddContent(int? id)
        {
            if ((id == null) || (database.Stories.Find(id) == null))
            {
                return HttpNotFound();
            }

            AddOrEditViewModel addContentView = new AddOrEditViewModel();
            addContentView.Story = database.Stories.Find(id);
            addContentView.Writers = database.Writers.ToList();

            return View(addContentView);
        }

        // This runs after the user submits a form to add the story content in the AddContent view
        // Add the story content and redirect back to the ViewStory view for the Story specified by id
        [HttpPost]
        public ActionResult Create(int? id, string new_story_content, string new_content_writer)
        {
            if ((id == null) || (database.Stories.Find(id) == null))
            {
                return HttpNotFound();
            }

            string queryString = "INSERT INTO storycontents (Content, ContentWriter_WriterID, Story_StoryID) VALUES (@content, @writer, @story)";

            SqlParameter[] queryParams = new SqlParameter[3];
            queryParams[0] = new SqlParameter("@content", new_story_content);
            queryParams[1] = new SqlParameter("@writer", new_content_writer);
            queryParams[2] = new SqlParameter("@story", id);

            database.Database.ExecuteSqlCommand(queryString, queryParams);

            return RedirectToAction("ViewStory/" + id, "Story");
        }

        // Sends a viewmodel to the EditContent view for StoryContent and returns the view
        // A viewmodel is used because the view needs information about both the story the content is being 
        // edited from, and a list of all the writers in the database
        public ActionResult EditContent(int? id)
        {
            if ((id == null) || (database.StoryContents.Find(id) == null))
            {
                return HttpNotFound();
            }

            AddOrEditViewModel editContentView = new AddOrEditViewModel();
            editContentView.StoryContent = database.StoryContents.Find(id);
            editContentView.Writers = database.Writers.ToList();

            return View(editContentView);
        }

        // This runs after the user submits a form to edit the story content in the EditContent view
        // Edit the story content and redirect back to the ViewStory view for the Story specified by 
        // the id of the story being edited
        [HttpPost]
        public ActionResult Update(int? id, string new_story_content, string new_content_writer)
        {
            if ((id == null) || (database.StoryContents.Find(id) == null))
            {
                return HttpNotFound();
            }

            string query = "UPDATE storycontents SET Content=@content, ContentWriter_WriterID=@writer WHERE StoryContentID=@contentid";

            SqlParameter[] queryParams = new SqlParameter[3];
            queryParams[0] = new SqlParameter("@content", new_story_content);
            queryParams[1] = new SqlParameter("@writer", new_content_writer);
            queryParams[2] = new SqlParameter("@contentid", id);

            database.Database.ExecuteSqlCommand(query, queryParams);

            return RedirectToAction("ViewStory/" + database.StoryContents.Find(id).Story.StoryID, "Story");
        }

        // Delete the specific story content corresponding to id
        // Returns the ViewStory view for Story corresponding to the story the content was linked to
        public ActionResult DeleteContent(int? id)
        {
            if ((id == null) || (database.StoryContents.Find(id) == null))
            {
                return HttpNotFound();
            }

            var storyID = database.StoryContents.Find(id).Story.StoryID;

            string queryString = "DELETE FROM storycontents WHERE StoryContentID=@scontentid";
            SqlParameter param = new SqlParameter("@scontentid", id);

            database.Database.ExecuteSqlCommand(queryString, param);

            return RedirectToAction("ViewStory/" + storyID, "Story");
        }
    }
}