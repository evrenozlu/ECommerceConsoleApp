using API.DTOs.Product.AddProduct;
using API.DTOs.Product.GetProduct;
using Domain.Interfaces;
using Domain.Products;
using System;
using System.Threading.Tasks;
using API.DTOs.Product.UpdateProduct;
using Domain.Campaigns;

namespace API.Services.Products
{
    public class ProductService : BaseService
    {
        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<GetProductResponse> GetProduct(GetProductRequest request)
        {
            var productInfo = new GetProductResponse();

            try
            {
                var repository = UnitOfWork.AsyncRepository<Product>();
                var product = await repository
                    .GetAsync(p => p.Id == request.Id);

                if (product != null)
                {
                    productInfo.Id = product.Id;
                    productInfo.CurrentPrice = product.CurrentPrice;
                    productInfo.Stock = product.Stock;

                }
                else
                {
                    productInfo.IsError = true;
                    productInfo.ErrorMessage = string.Format("There is no product with product code {0}.", request.Id);
                }

            }
            catch(Exception ex) when (ex != null)
            {
                productInfo.IsError = true;
                productInfo.ErrorMessage = ex.Message;
            }

            return productInfo;
        }

        public async Task<AddProductResponse> AddProduct(AddProductRequest request)
        {
            var response = new AddProductResponse();

            try
            {
                var repository = UnitOfWork.AsyncRepository<Product>();
                var product = await repository
                    .GetAsync(p => p.Id == request.Id);

                if (product == null)
                {
                    var newProduct = new Product(request.Id, request.Stock, request.DefaultPrice, request.DefaultPrice);
                    await repository.AddAsync(newProduct);
                    await UnitOfWork.SaveChangesAsync();

                    response = new AddProductResponse
                    {
                        Id = newProduct.Id,
                        Stock = newProduct.Stock,
                        CurrentPrice = newProduct.CurrentPrice,
                        DefaultPrice = newProduct.DefaultPrice
                    };
                }
                else
                {
                    response.IsError = true;
                    response.ErrorMessage = string.Format("There is a product with code {0}.", request.Id);
                }
            }

            catch (Exception ex) when (ex != null)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<UpdateProductQuantityResponse> UpdateProductQuantity(UpdateProductQuantityRequest request)
        {
            var updateProductResponse = new UpdateProductQuantityResponse();

            try
            {
                var repository = UnitOfWork.AsyncRepository<Product>();
                var product = await repository.GetAsync(_ => _.Id == request.Id);
                if (product != null)
                {
                    if (product.Stock < request.OrderQuantity)
                    {
                        updateProductResponse.IsError = true;
                        updateProductResponse.ErrorMessage = string.Format("There is no stock with product code {0} - Current stock is: {1}.", request.Id, product.Stock);
                    }
                    else
                    {
                        product.SetProductStock(product.Stock - request.OrderQuantity);
                        await repository.UpdateAsync(product);
                        await UnitOfWork.SaveChangesAsync();
                        updateProductResponse = new UpdateProductQuantityResponse
                        {
                            Id = product.Id,
                            Stock = product.Stock,
                            CurrentPrice = product.CurrentPrice,
                            DefaultPrice = product.DefaultPrice
                        };
                    }
                }

                else
                {
                    updateProductResponse.IsError = true;
                    updateProductResponse.ErrorMessage = string.Format("There is no product with product code {0}.", request.Id);
                }

            }
            catch (Exception ex) when (ex != null)
            {
                updateProductResponse.IsError = true;
                updateProductResponse.ErrorMessage = ex.Message;
            }

            return updateProductResponse;
        }

        public async Task<bool> UpdateProductPrice(Campaign campaign, decimal timeCount)
        {
            try
            {
                var repository = UnitOfWork.AsyncRepository<Product>();
                var product = await repository.GetAsync(_ => _.Id == campaign.ProductCode);
                if (product != null)
                {
                    if (!campaign.IsActive)
                    {
                        product.SetProductCurrentPrice(product.DefaultPrice);
                        await repository.UpdateAsync(product);
                        await UnitOfWork.SaveChangesAsync();
                    }
                    else
                    {
                        decimal newPrice = product.CurrentPrice + GetCampaignPrice(campaign, product, timeCount);
                        decimal limit = (product.DefaultPrice * campaign.PriceManipulationLimit) / (decimal) 100;
                        if (Math.Abs(product.DefaultPrice - newPrice) <= limit)
                        {
                            product.SetProductCurrentPrice(newPrice);
                            await repository.UpdateAsync(product);
                            await UnitOfWork.SaveChangesAsync();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex) when (ex != null)
            {
                return false;
            }

        }

        public decimal GetCampaignPrice(Campaign campaign, Product product, decimal timeCount)
        {
            return -5 * timeCount;
        }

        public async Task<bool> DeleteProducts()
        {

            var repository = UnitOfWork.AsyncRepository<Product>();
            var products = await repository
                    .GetAll();

            foreach (var product in products)
            {
                await repository.DeleteAsync(product);
                await UnitOfWork.SaveChangesAsync();
            }

            return true;
        }
    }
}
