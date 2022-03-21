using API.DTOs.Base;
using System.Collections.Generic;

namespace API.DTOs.Campaign.GetCampaign
{
    public class GetActiveCampaignResponse : BaseResponse
    {
        public int Id { get; set; }
        public string CampaignName { get; set; }
        public string ProductCode { get; set; }
        public int Duration { get; set; }
        public decimal PriceManipulationLimit { get; set; }
        public int TargetSalesCount { get; set; }
        public bool IsActive { get; set; }
        public int TotalSales { get; set; }
        public decimal Turnover { get; set; }
        public decimal AverageItemPrice { get; set; }
        public string CreatedTime { get; set; }
        public string EndTime { get; set; }
    }
}