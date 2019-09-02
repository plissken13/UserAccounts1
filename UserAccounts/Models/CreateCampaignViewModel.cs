using System.ComponentModel.DataAnnotations;

namespace UserAccounts.Models
{
    public class CreateCampaignViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 1)]
        public string Description { get; set; }

        [Required]
        public double Sum { get; set; }
    }
}