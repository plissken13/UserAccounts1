using System.Collections.Generic;
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

        public virtual List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        
        public string Title { get; set; }

        public string Content { get; set; }

        public int Id { get; set; }

        public virtual CampaignModel Campaign { get; set; }
    }
}