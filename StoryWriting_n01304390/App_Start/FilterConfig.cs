using System.Web;
using System.Web.Mvc;

namespace StoryWriting_n01304390
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
