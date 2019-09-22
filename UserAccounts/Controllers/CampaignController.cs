using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using UserAccounts.Models;

namespace UserAccounts.Controllers
{
    [Authorize]
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
                var campaignModel = GetCampaignModel(model, db);
                campaignModel.CreatedOn = DateTime.Now;
                db.CampaignModels.Add(campaignModel);
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
                Sum = campaign.RequiredSum,
                CreatedOn = campaign.CreatedOn
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
                return RedirectToAction("Details", "Campaign", new {id = model.Id});
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

                var comments = db.CommentsModels.Where(x => x.CampaignId == id).ToList();
                campaign.Comments = comments;

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



        [Authorize]
        public ActionResult CampaignList(string sortOrder, string currentSort, int? page)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int pageSize = 3;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Name" : sortOrder;
            IPagedList<CampaignModel> campaigns = null;
            switch (sortOrder)
            {
                case "CreatedOn":
                    if (sortOrder.Equals(currentSort))
                        campaigns = db.CampaignModels.OrderByDescending
                            (m => m.CreatedOn).ToPagedList(pageIndex, pageSize);
                    else
                        campaigns = db.CampaignModels.OrderBy
                            (m => m.CreatedOn).ToPagedList(pageIndex, pageSize);
                    break;
                case "Name":
                    if (sortOrder.Equals(currentSort))
                        campaigns = db.CampaignModels.OrderByDescending
                                (m => m.Name).ToPagedList(pageIndex, pageSize);
                    else
                        campaigns = db.CampaignModels.OrderBy
                                (m => m.Name).ToPagedList(pageIndex, pageSize);
                    break;
                case "RequiredSum":
                    if (sortOrder.Equals(currentSort))
                        campaigns = db.CampaignModels.OrderByDescending
                                (m => m.RequiredSum).ToPagedList(pageIndex, pageSize);
                    else
                        campaigns = db.CampaignModels.OrderBy
                                (m => m.RequiredSum).ToPagedList(pageIndex, pageSize);
                    break;
            }
            return View(campaigns);

            //using (var db = new ApplicationDbContext())
            //{
            //    return View(db.CampaignModels.ToList());
            //}
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
                OwnerId = ownerId,
                ImageUrl = model.ImageUrl,
                CreatedOn = model.CreatedOn
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

            return RedirectToAction("Details", "Campaign", new {id = post.CampaignId});
        }

        [Authorize]
        public ActionResult Comment(int id)
        {
            var comment = new CommentsModel()
            {
                CampaignId = id
            };
            return View(comment);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Comment(CommentsModel comment)
        {
            using (var db = new ApplicationDbContext())
            {

                db.CommentsModels.Add(comment);
                var authorId = User.Identity.GetUserId();
                var authorName = db.Users.SingleOrDefault(x => x.Id == authorId)?.UserName;
                comment.AuthorName = authorName;
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Campaign", new { id = comment.CampaignId });
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
                return RedirectToAction("Details", "Campaign", new {id = campaign.Id});
            }
        }
    }
}