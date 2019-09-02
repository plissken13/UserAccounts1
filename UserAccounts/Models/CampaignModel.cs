namespace UserAccounts.Models
{
    public class CampaignModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int OwnerId { get; set; }
        
        public double RequiredSum { get; set; }

        public double CurrentSum { get; set; }
    }
}