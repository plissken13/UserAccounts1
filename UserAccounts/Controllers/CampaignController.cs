﻿using System.Data.Entity;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using UserAccounts.Models;

namespace UserAccounts.Controllers
{
    public class CampaignController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(CreateCampaignViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                db.CampaignModels.Add(GetCampaignModel(model));
                db.SaveChanges();
            }

            return RedirectToAction("CampaignList", "Campaign");
        }

        //TODO: private method and null check.
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var campaign = db.CampaignModels.SingleOrDefault(x => x.Id == id);
                var temp = new CreateCampaignViewModel
                {
                    Name = campaign.Name,
                    Description = campaign.Description,
                    Sum = campaign.RequiredSum
                };
                return View(temp);
            }
        }

        [HttpPost]
        public ActionResult Edit(CreateCampaignViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                var campaign = GetCampaignModel(model);
                db.CampaignModels.AddOrUpdate(campaign);
                db.SaveChanges();
                return RedirectToAction("CampaignList", "Campaign");
            }
        }

        public ActionResult Details(int id)
        {
            throw new System.NotImplementedException();
        }

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

        private CampaignModel GetCampaignModel(CreateCampaignViewModel model)
        {
            return new CampaignModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                RequiredSum = model.Sum,
                OwnerId = User.Identity.GetUserId()
            };
        }
    }
}