namespace UserAccounts.Models
{
    public class CommentsModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int CampaignId { get; set; }

        public string AuthorName { get; set; }

        public virtual CampaignModel Campaign { get; set; }
    }
}