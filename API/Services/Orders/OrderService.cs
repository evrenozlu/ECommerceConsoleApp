using API.DTOs.Campaign.UpdateCampaign;
using API.DTOs.Order.CreateOrder;
using API.DTOs.Product.UpdateProduct;
using API.Services.Campaigns;
using API.Services.Products;
using Domain.Interfaces;
using Domain.Orders;
using System;
using System.Threading.Tasks;

namespace API.Services.Orders
{
    public class OrderService : BaseService
    {
        private readonly ProductService productService;
        private readonly CampaignService campaignService;

        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            productService = new ProductService(unitOfWork);
            campaignService = new CampaignService(unitOfWork);
        }

        public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request)
        {
            var response = new CreateOrderResponse();

            try
            {
                var updateProductQuantityRequest = new UpdateProductQuantityRequest()
                {
                    Id = request.ProductCode,
                    OrderQuantity = request.Quantity
                };

                var productResponse = await productService.UpdateProductQuantity(updateProductQuantityRequest);

                if (!productResponse.IsError)
                {
                    var newOrder = new Order(request.ProductCode, request.Quantity);
                    var repositoryOrder = UnitOfWork.AsyncRepository<Order>();
                    await repositoryOrder.AddAsync(newOrder);
                    await UnitOfWork.SaveChangesAsync();

                    response = new CreateOrderResponse
                    {
                        ProductCode = newOrder.ProductCode,
                        Quantity = newOrder.Quantity
                    };

                    var updateCampaignResponse = await campaignService.UpdateCampaignAfterOrder(new UpdateCampaignAfterOrderRequest() { ProductCode = request.ProductCode, Quantity = request.Quantity });

                    if (updateCampaignResponse.IsError)
                    {
                        response.IsError = true;
                        response.ErrorMessage = updateCampaignResponse.ErrorMessage;
                    }
                }
                else
                {
                    response.IsError = true;
                    response.ErrorMessage = productResponse.ErrorMessage;
                }
            }
            catch (Exception ex) when (ex != null)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }
         
            return response;
        }

        public async Task<bool> DeleteOrders()
        {

            var repository = UnitOfWork.AsyncRepository<Order>();
            var orders = await repository
                    .GetAll();

            foreach (var order in orders)
            {
                await repository.DeleteAsync(order);
                await UnitOfWork.SaveChangesAsync();
            }

            return true;
        }
    }
}