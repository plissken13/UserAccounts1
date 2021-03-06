﻿namespace UserAccounts.Models
{
    public class PostModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int CampaignId { get; set; }

        public virtual CampaignModel Campaign { get; set; }
    }
}