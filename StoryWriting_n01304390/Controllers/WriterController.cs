using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using StoryWriting_n01304390.Models;

namespace StoryWriting_n01304390.Controllers
{
    public class WriterController : Controller
    {
        private StoryWritingDbContext database = new StoryWritingDbContext();

        // Default View for Writer, redirect to the GetList View
        public ActionResult Index()
        {
            return RedirectToAction("GetList");
        }

        // Send a list of all writers to the GetList View for Writer and returns the view
        public ActionResult GetList()
        {
            IEnumerable<Writer> writers = database.Writers.ToList();
            return View(writers);
        }

        // Simply returns the AddWriter view
        public ActionResult AddWriter()
        {
            return View();
        }

        /* This runs after the user submits the form to add a writer in the view, add the writer to the database
            and redirect back to the GetList view */
        [HttpPost]
        public ActionResult Create(string new_writer_first_name, string new_writer_last_name, string new_writer_username)
        {
            string queryString = "INSERT INTO writers (FirstName, LastName, UserName) VALUES (@fname, @lname, @uname)";

            SqlParameter[] queryParams = new SqlParameter[3];
            queryParams[0] = new SqlParameter("@fname", new_writer_first_name);
            queryParams[1] = new SqlParameter("@lname", new_writer_last_name);
            queryParams[2] = new SqlParameter("@uname", new_writer_username);

            database.Database.ExecuteSqlCommand(queryString, queryParams);

            return RedirectToAction("GetList");
        }

        // Send the specific Writer model with corresponding with id to the EditWriter view and return the view
        public ActionResult EditWriter(int? id)
        {
            if ((id == null) || (database.Writers.Find(id) == null))
            {
                return HttpNotFound();
            }
            
            return View(database.Writers.Find(id));
        }

        /* This runs after the user submits the form to edit the writer in the view, update the writer to the database
            and redirect back to the view for the specific writer */
        [HttpPost]
        public ActionResult Update(int? id, string new_writer_first_name, string new_writer_last_name, string new_writer_username)
        {
            if ((id == null) || (database.Writers.Find(id) == null))
            {
                return HttpNotFound();
            }

            string query = "UPDATE writers SET FirstName=@fname, LastName=@lname, Username=@uname WHERE WriterID=@writerid";

            SqlParameter[] queryParams = new SqlParameter[4];
            queryParams[0] = new SqlParameter("@fname", new_writer_first_name);
            queryParams[1] = new SqlParameter("@lname", new_writer_last_name);
            queryParams[2] = new SqlParameter("@uname", new_writer_username);
            queryParams[3] = new SqlParameter("@writerid", id);

            database.Database.ExecuteSqlCommand(query, queryParams);

            return RedirectToAction("ViewWriter/" + database.Writers.Find(id).WriterID);
        }

        // Delete all the story contents and stories created by this writer, then delete the writer itself.
        // Returns the GetList view for Writer
        public ActionResult DeleteWriter(int? id)
        {
            if ((id == null) || (database.Writers.Find(id) == null))
            {
                return HttpNotFound();
            }

            string queryString = "DELETE FROM storycontents WHERE ContentWriter_WriterID=@writerid";
            SqlParameter param = new SqlParameter("@writerid", id);

            database.Database.ExecuteSqlCommand(queryString, param);

            queryString = "DELETE FROM stories WHERE StoryCreator_WriterID=@writerid";

            database.Database.ExecuteSqlCommand(queryString, param);

            queryString = "DELETE FROM writers WHERE WriterID=@writerid";

            database.Database.ExecuteSqlCommand(queryString, param);

            return RedirectToAction("GetList");
        }

        // Send the specific Writer model with corresponding with id to the ViewWriter view and return the view
        public ActionResult ViewWriter(int? id)
        {
            if ((id == null) || database.Writers.Find(id) == null)
            {
                return HttpNotFound();
            }

            return View(database.Writers.Find(id));
        }
    }
}