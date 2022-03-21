using Domain.Interfaces;
using System;
using System.Threading.Tasks;
using API.DTOs.Campaign.GetCampaign;
using Domain.Campaigns;
using API.DTOs.Campaign.CreateCampaign;
using API.DTOs.Campaign.UpdateCampaign;
using System.Collections.Generic;
using System.Linq;
using API.Services.Products;
using API.DTOs.Product.GetProduct;

namespace API.Services.Campaigns
{
    public class CampaignService : BaseService
    {

        private readonly ProductService productService;

        public CampaignService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            productService = new ProductService(unitOfWork);
        }

        public async Task<GetCampaignResponse> GetCampaign(GetCampaignRequest request)
        {
            var response = new GetCampaignResponse();

            try
            {
                var repository = UnitOfWork.AsyncRepository<Campaign>();
                var campaign = await repository
                    .GetAsync(c => c.CampaignName == request.CampaignName);

                if (campaign != null)
                {
                    response = new GetCampaignResponse
                    {
                        CampaignName = campaign.CampaignName,
                        ProductCode = campaign.ProductCode,
                        Duration = campaign.Duration,
                        PriceManipulationLimit = campaign.PriceManipulationLimit,
                        TargetSalesCount = campaign.TargetSalesCount,
                        IsActive = campaign.IsActive,
                        TotalSales = campaign.TotalSales,
                        Turnover = campaign.Turnover,
                        AverageItemPrice = campaign.AverageItemPrice,
                        CreatedTime = campaign.CreatedTime,
                        EndTime = campaign.EndTime,
                    };
                }

                else
                {
                    response.IsError = true;
                    response.ErrorMessage = string.Format("There is no campaign with campaign name {0}.", request.CampaignName);
                }

            }
            catch(Exception ex) when (ex != null)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<UpdateActiveCampaignResponse> UpdateActiveCampaign(UpdateActiveCampaignRequest request)
        {
            var activeCampaigns = new List<GetActiveCampaignResponse>();
            var response = new UpdateActiveCampaignResponse();

            try
            {
                var repository = UnitOfWork.AsyncRepository<Campaign>();
                var campaigns = await repository
                    .ListAsync(c => c.IsActive == true);

                if (campaigns != null && campaigns.Count != 0)
                {
                    activeCampaigns = campaigns.Select(_ => new GetActiveCampaignResponse()
                    {
                        Id = _.Id,
                        CampaignName = _.CampaignName,
                        ProductCode = _.ProductCode,
                        Duration = _.Duration,
                        PriceManipulationLimit = _.PriceManipulationLimit,
                        TargetSalesCount = _.TargetSalesCount,
                        IsActive = _.IsActive,
                        TotalSales = _.TotalSales,
                        Turnover = _.Turnover,
                        AverageItemPrice = _.AverageItemPrice,
                        CreatedTime = _.CreatedTime,
                        EndTime = _.EndTime,
                    }).ToList();

                    foreach (var campaign in activeCampaigns)
                    {
                        var updatedCampaign = new Campaign(campaign.Id, campaign.CampaignName, campaign.ProductCode, campaign.Duration, campaign.PriceManipulationLimit,
                                campaign.TargetSalesCount, campaign.IsActive, campaign.TotalSales, campaign.Turnover, campaign.AverageItemPrice, campaign.CreatedTime, campaign.EndTime);

                        if (DateTime.Parse(request.CurrentDate) >= DateTime.Parse(campaign.EndTime))
                        {
                            await UpdateCampaignStatus(updatedCampaign);
                        }
                        await productService.UpdateProductPrice(updatedCampaign, request.TimeCount);
                    }
                }
            }
            catch (Exception ex) when (ex != null)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<CreateCampaignResponse> CreateCampaign(CreateCampaignRequest request)
        {
            var response = new CreateCampaignResponse();

            try
            {

                var repository = UnitOfWork.AsyncRepository<Campaign>();

                var sameCampaignName = await repository
                        .GetAsync(c => c.CampaignName == request.CampaignName);

                if (sameCampaignName == null)
                {
                    var currentCampaign = await repository
                    .GetAsync(c => c.IsActive == true && c.ProductCode == request.ProductCode);
                    
                    if (currentCampaign == null)
                    {
                        string sqlFormattedDate = request.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        var newCampaign = new Campaign(request.CampaignName, request.ProductCode, request.Duration, request.PriceManipulationLimit, request.TargetSalesCount, sqlFormattedDate);

                        var result = await repository.AddAsync(newCampaign);
                        await UnitOfWork.SaveChangesAsync();

                        response = new CreateCampaignResponse
                        {
                            CampaignName = result.CampaignName,
                            ProductCode = result.ProductCode,
                            Duration = result.Duration,
                            PriceManipulationLimit = result.PriceManipulationLimit,
                            TargetSalesCount = result.TargetSalesCount,
                            IsActive = result.IsActive,
                            TotalSales = result.TotalSales,
                            Turnover = result.Turnover,
                            AverageItemPrice = result.AverageItemPrice,
                            CreatedTime = result.CreatedTime,
                            EndTime = result.EndTime,
                        };
                    }
                    else
                    {
                        response.IsError = true;
                        response.ErrorMessage = string.Format("There is an active campaign for product code {0}.", request.ProductCode);
                    }
                }
                else
                {
                    response.IsError = true;
                    response.ErrorMessage = string.Format("There is a campaign with name {0}.", request.CampaignName);
                }
            }

            catch (Exception ex) when (ex != null)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<bool> UpdateCampaignStatus(Campaign campaign)
        {
            try
            {
                var repository = UnitOfWork.AsyncRepository<Campaign>();
                if (campaign != null)
                {
                    campaign.SetCampaignStatus(false);
                    await repository.UpdateAsync(campaign);
                    await UnitOfWork.SaveChangesAsync();
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<UpdateCampaignAfterOrderResponse> UpdateCampaignAfterOrder(UpdateCampaignAfterOrderRequest request)
        {
            var activeCampaigns = new List<GetActiveCampaignResponse>();
            var response = new UpdateCampaignAfterOrderResponse();

            try
            {
                var repository = UnitOfWork.AsyncRepository<Campaign>();
                var campaign = await repository
                    .GetAsync(c => c.IsActive == true && c.ProductCode == request.ProductCode) ;

                if (campaign != null)
                {
                    GetProductResponse product = await productService.GetProduct(new GetProductRequest() { Id = campaign.ProductCode });

                    campaign.SetCampaignAfterOrder(product.CurrentPrice, request.Quantity);
                    await repository.UpdateAsync(campaign);
                    await UnitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex) when (ex != null)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<bool> DeleteCampaigns()
        {

            var repository = UnitOfWork.AsyncRepository<Campaign>();
            var campaigns = await repository
                    .GetAll();

            foreach (var campaign in campaigns)
            {
                await repository.DeleteAsync(campaign);
                await UnitOfWork.SaveChangesAsync();
            }

            return true;
        }
    }
}
