﻿using System.Collections.Generic;
using System.Data.Entity;

namespace UserAccounts.Models
{
    public class CampaignModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string OwnerId { get; set; }
        
        public double RequiredSum { get; set; }

        public double CurrentSum { get; set; }

        public virtual IList<PostModel> Posts { get; set; }
    }
}