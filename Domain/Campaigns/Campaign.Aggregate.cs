using Domain.Base;
using Domain.Shared;
using System;

namespace Domain.Campaigns
{
    public partial class Campaign : IAggregateRoot
    {
        public Campaign(string campaignName, string productCode, int duration, decimal priceManipulationLimit, int targetSalesCount, string createdTime)
        {
            CampaignName = campaignName;
            ProductCode = productCode;
            Duration = duration;
            PriceManipulationLimit = priceManipulationLimit;
            TargetSalesCount = targetSalesCount;
            IsActive = true;
            TotalSales = 0;
            Turnover = 0;
            AverageItemPrice = 0;
            DateTime createdTimeDT = DateTime.Parse(createdTime);
            DateTime endTimeDT = createdTimeDT.AddHours(Duration);
            CreatedTime = createdTime;
            EndTime = endTimeDT.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public Campaign(string campaignName, string productCode, int duration, decimal priceManipulationLimit, int targetSalesCount, bool isActive, int totalSales, decimal turnover, decimal averageItemPrice, string createdTime, string endTime)
        {
            CampaignName = campaignName;
            ProductCode = productCode;
            Duration = duration;
            PriceManipulationLimit = priceManipulationLimit;
            TargetSalesCount = targetSalesCount;
            IsActive = isActive;
            TotalSales = totalSales;
            Turnover = turnover;
            AverageItemPrice = averageItemPrice;
            CreatedTime = createdTime;
            EndTime = endTime;
        }
        public Campaign(int id, string campaignName, string productCode, int duration, decimal priceManipulationLimit, int targetSalesCount, bool isActive, int totalSales, decimal turnover, decimal averageItemPrice, string createdTime, string endTime)
        {
            Id = id;
            CampaignName = campaignName;
            ProductCode = productCode;
            Duration = duration;
            PriceManipulationLimit = priceManipulationLimit;
            TargetSalesCount = targetSalesCount;
            IsActive = isActive;
            TotalSales = totalSales;
            Turnover = turnover;
            AverageItemPrice = averageItemPrice;
            CreatedTime = createdTime;
            EndTime = endTime;
        }


        public void SetCampaignStatus(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetCampaignAfterOrder(decimal currentPrice, int quantity)
        {
            TotalSales += quantity;
            Turnover += currentPrice * quantity;
            AverageItemPrice = Turnover / (decimal) TotalSales;
        }
    }
}
