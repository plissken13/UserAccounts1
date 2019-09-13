using System.Collections.Generic;

namespace UserAccounts.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public string Role { get; set; }

        public IList<CampaignModel> Campaigns { get; set; }
    }
}