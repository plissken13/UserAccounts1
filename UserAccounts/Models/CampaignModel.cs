using System.Collections.Generic;

namespace UserAccounts.Models
{
    public class CampaignModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string OwnerName { get; set; }

        public string OwnerId { get; set; }
        
        public double RequiredSum { get; set; }

        public double CurrentSum { get; set; }

        public IList<PostModel> Posts { get; set; }

        public IList<CommentsModel> Comments { get; set; }
    }
}