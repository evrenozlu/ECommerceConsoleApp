using System;

namespace API.DTOs.Campaign.UpdateCampaign
{
    public class UpdateCampaignAfterOrderRequest
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}