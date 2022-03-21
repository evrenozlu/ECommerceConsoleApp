using System;

namespace API.DTOs.Campaign.CreateCampaign
{
    public class CreateCampaignRequest
    {
        public string CampaignName { get; set; }
        public string ProductCode { get; set; }
        public int Duration { get; set; }
        public decimal PriceManipulationLimit { get; set; }
        public int TargetSalesCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}