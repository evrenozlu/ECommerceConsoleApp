using System;

namespace API.DTOs.Campaign.UpdateCampaign
{
    public class UpdateActiveCampaignRequest
    {
        public string CurrentDate { get; set; }
        public decimal TimeCount { get; set; }
    }
}