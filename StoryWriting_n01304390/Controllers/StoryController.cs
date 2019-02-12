using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using StoryWriting_n01304390.Models;
using StoryWriting_n01304390.Models.ViewModels;
using System.IO;

namespace StoryWriting_n01304390.Controllers
{
    public class StoryController : Controller
    {
        private StoryWritingDbContext database = new StoryWritingDbContext();

        // Default View for Story, redirect to the GetList View
        public ActionResult Index()
        {
            return RedirectToAction("GetList");
        }

        // Send a list of all stories to the GetList View for Story and returns the view
        public ActionResult GetList()
        {
            IEnumerable<Story> stories = database.Stories.ToList();
            return View(stories);
        }

        // Send a list of all writers to the AddStory view for Story (used to generate the dropdown list) and return the view
        public ActionResult AddStory()
        {
            IEnumerable<Writer> writers = database.Writers.ToList();
            return View(writers);
        }

        /* This runs after the user submits the form to add a story in the view, add the story to the database
            and redirect back to the GetList view */
        [HttpPost]
        public ActionResult Create(string new_story_title, string new_story_desc, string new_writer_username)
        {
            string queryString = "INSERT INTO stories (StoryTitle, StoryDescription, StoryCreator_WriterID, HasFeatureImage) VALUES (@stitle, @sdesc, @suser, @hasImg)";

            SqlParameter[] queryParams = new SqlParameter[4];
            queryParams[0] = new SqlParameter("@stitle", new_story_title);
            queryParams[1] = new SqlParameter("@sdesc", new_story_desc);
            queryParams[2] = new SqlParameter("@suser", new_writer_username);
            queryParams[3] = new SqlParameter("@hasImg", false);

            database.Database.ExecuteSqlCommand(queryString, queryParams);

            return RedirectToAction("GetList");
        }

        // Sends a viewmodel to the EditStory view for Story and returns the view
        // A viewmodel is used because the view needs information about both the story being 
        // edited and the writers in the database
        public ActionResult EditStory(int? id)
        {
            if ((id == null) || (database.Stories.Find(id) == null))
            {
                return HttpNotFound();
            }

            AddOrEditViewModel storyEditView = new AddOrEditViewModel();
            storyEditView.Story = database.Stories.Find(id);
            storyEditView.Writers = database.Writers.ToList();

            return View(storyEditView);
        }

        // This runs after the user submits a form to edit the story in the EditStory view
        // Updates the story in the database and adds/replaces an image if necessary
        [HttpPost]
        public ActionResult Update(int? id, string new_story_title, string new_story_desc, string new_writer_username, HttpPostedFileBase story_feature_image)
        {
            if ((id == null) || (database.Stories.Find(id) == null))
            {
                return HttpNotFound();
            }

            string query = "UPDATE stories SET StoryTitle=@stitle, StoryDescription=@sdesc, StoryCreator_WriterID=@screator WHERE StoryID=@storyid";

            SqlParameter[] queryParams = new SqlParameter[4];
            queryParams[0] = new SqlParameter("@stitle", new_story_title);
            queryParams[1] = new SqlParameter("@sdesc", new_story_desc);
            queryParams[2] = new SqlParameter("@screator", new_writer_username);
            queryParams[3] = new SqlParameter("@storyid", id);

            database.Database.ExecuteSqlCommand(query, queryParams);

            // Code to add an image follows Christine's Example
            if (story_feature_image != null)
            {
                //file extensioncheck taken from https://www.c-sharpcorner.com/article/file-upload-extension-validation-in-asp-net-mvc-and-javascript/
                var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                var extension = Path.GetExtension(story_feature_image.FileName).Substring(1);

                if (valtypes.Contains(extension))
                {
                    // Creates a filename that will be (id of story).(extension)
                    string filename = id + "." + extension;
                    string path = Path.Combine(Server.MapPath("~/images/featureImages"), filename);

                    // Save/Overwrite the image
                    story_feature_image.SaveAs(path);

                    // Update the fields for this story saying the story has a feature image, and save the extension
                    query = "UPDATE stories SET HasFeatureImage=@hasImg, ImageType=@ImgType WHERE StoryID=@storyid";

                    queryParams = new SqlParameter[3];
                    queryParams[0] = new SqlParameter("@hasImg", true);
                    queryParams[1] = new SqlParameter("@ImgType", extension);
                    queryParams[2] = new SqlParameter("@storyid", id);

                    database.Database.ExecuteSqlCommand(query, queryParams);
                }
            }

            return RedirectToAction("ViewStory/" + database.Stories.Find(id).StoryID);
        }

        // Delete all the story contents relating to this story and the story itself
        // Returns the GetList view for Story
        public ActionResult DeleteStory(int? id)
        {
            if ((id == null) || (database.Stories.Find(id) == null))
            {
                return HttpNotFound();
            }

            string queryString = "DELETE FROM storycontents WHERE Story_StoryID=@storyid";
            SqlParameter param = new SqlParameter("@storyid", id);

            database.Database.ExecuteSqlCommand(queryString, param);

            queryString = "DELETE FROM stories WHERE StoryID=@storyid";

            database.Database.ExecuteSqlCommand(queryString, param);

            return RedirectToAction("GetList");
        }

        // Returns the ViewStory view for the story corresponding to id
        public ActionResult ViewStory(int? id)
        {
            if ((id == null) || (database.Stories.Find(id) == null))
            {
                return HttpNotFound();
            }

            return View(database.Stories.Find(id));
        }

        // Returns the ReadStory view for the story corresponding to id
        public ActionResult ReadStory(int? id)
        {
            if ((id == null) || (database.Stories.Find(id) == null))
            {
                return HttpNotFound();
            }

            return View(database.Stories.Find(id));
        }
    }
}