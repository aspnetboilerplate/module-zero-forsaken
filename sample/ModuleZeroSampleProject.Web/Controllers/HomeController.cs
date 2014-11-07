using System.Web.Mvc;

namespace ModuleZeroSampleProject.Web.Controllers
{
    public class HomeController : ModuleZeroSampleProjectControllerBase
    {
        public ActionResult Index()
        {
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
    }
}