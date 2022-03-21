using Domain.Base;
using Domain.Shared;
using System;

namespace Domain.Campaigns
{
    public partial class Campaign : BaseEntity<int>
    {
        public string CampaignName { get; private set; }
        public string ProductCode { get; private set; }
        public int Duration { get; private set; }
        public decimal PriceManipulationLimit { get; private set; }
        public int TargetSalesCount { get; private set; }
        public bool IsActive { get; private set; }
        public int TotalSales { get; private set; }
        public decimal Turnover { get; private set; }
        public decimal AverageItemPrice { get; private set; }
        public string CreatedTime { get; private set; }
        public string EndTime { get; private set; }
    }
}