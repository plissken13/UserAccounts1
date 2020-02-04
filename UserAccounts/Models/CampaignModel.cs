using System;
using System.Collections.Generic;

namespace UserAccounts.Models
{
    public class CampaignModel
    {
        private const int maxShortSize = 250;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortenedDescription => GetShortened();

        public string OwnerName { get; set; }

        public string OwnerId { get; set; }

        public double RequiredSum { get; set; }

        public double CurrentSum { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public IList<PostModel> Posts { get; set; }

        public IList<CommentsModel> Comments { get; set; }

        private string GetShortened()
        {
            if (Description == null || Description.Length < maxShortSize)
            {
                return Description;
            }

            var substring = Description.Substring(0, maxShortSize);
            if (substring.Length < Description.Length)
            {
                substring += "...";
            }

            return substring;
        }
    }
}