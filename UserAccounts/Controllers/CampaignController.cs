using System.Collections.Generic;
using System.Web.Mvc;
using UserAccounts.Models;

namespace UserAccounts.Controllers
{
    public class CampaignController : Controller
    {
        public ActionResult Create()
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Edit(int id)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Details(int id)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult CampaignList()
        {
            var viewModel = new CampaignModel
            {
                Id = 1,
                Name = "qwerty",
                OwnerId = 2,
                RequiredSum = 1,
                CurrentSum = 3
            };
            var temp = new List<CampaignModel>
            {
                viewModel
            };
            return View(temp);
        }
    }
}