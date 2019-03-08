using System.Web;
using System.Web.Mvc;

namespace Urok1_povtor_metanit
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
