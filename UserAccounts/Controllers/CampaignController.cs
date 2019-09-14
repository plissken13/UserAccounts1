using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using UserAccounts.Models;

namespace UserAccounts.Controllers
{
    public class CampaignController : Controller
    {
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                return View();
            }

            using (var db = new ApplicationDbContext())
            {
                var users = new SelectList(db.Users.ToList(), "Id", "UserName");
                ViewBag.Users = users;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(CreateCampaignViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                db.CampaignModels.Add(GetCampaignModel(model, db));
                db.SaveChanges();
            }

            return RedirectToAction("CampaignList", "Campaign");
        }

        private CreateCampaignViewModel EditCampaign(CampaignModel campaign)
        {
            var temp = new CreateCampaignViewModel
            {
                Name = campaign.Name,
                Description = campaign.Description,
                Sum = campaign.RequiredSum
            };
            return temp;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var campaign = db.CampaignModels.SingleOrDefault(x => x.Id == id);
                if (campaign != null)
                {
                    return View(EditCampaign(campaign));
                }

                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(CreateCampaignViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                var campaign = GetCampaignModel(model, db);
                db.CampaignModels.AddOrUpdate(campaign);
                db.SaveChanges();
                return RedirectToAction("CampaignList", "Campaign");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var campaign = db.CampaignModels.SingleOrDefault(x => x.Id == id);
                if (campaign == null)
                {
                    return View("CampaignNotFound");
                }

                var posts = db.PostModels.Where(x => x.CampaignId == id).ToList();
                campaign.Posts = posts;
                return View(campaign);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var campaign = await db.CampaignModels.SingleOrDefaultAsync(x => x.Id == id);
                if (campaign != null)
                {
                    db.CampaignModels.Remove(campaign);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("CampaignList", "Campaign");
        }

        public ActionResult CampaignList()
        {
            using (var db = new ApplicationDbContext())
            {
                return View(db.CampaignModels.ToList());
            }
        }

        private CampaignModel GetCampaignModel(CreateCampaignViewModel model, ApplicationDbContext db)
        {
            var ownerId = model.OwnerId ?? User.Identity.GetUserId();
            var ownerName = db.Users.SingleOrDefault(x => x.Id == ownerId)?.UserName;
            return new CampaignModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                RequiredSum = model.Sum,
                OwnerName = ownerName,
                OwnerId = ownerId
            };
        }

        [Authorize]
        public ActionResult Write(int id)
        {
            var post = new PostModel
            {
                CampaignId = id
            };
            return View(post);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Write(PostModel post)
        {
            using (var db = new ApplicationDbContext())
            {
                db.PostModels.Add(post);
                db.SaveChanges();
            }

            return RedirectToAction("CampaignList", "Campaign");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Donate(int id)
        {
            var model = new DonationModel()
            {
                CampaignId = id
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Donate(DonationModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                var campaign = db.CampaignModels.SingleOrDefault(x => x.Id == model.CampaignId);
                if (campaign == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                campaign.CurrentSum += model.Sum;
                db.CampaignModels.AddOrUpdate(campaign);
                db.SaveChanges();
                return RedirectToAction("CampaignList", "Campaign");
            }
        }
    }
}