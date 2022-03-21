using API.DTOs.Campaign.CreateCampaign;
using API.DTOs.Campaign.GetCampaign;
using API.DTOs.Campaign.UpdateCampaign;
using API.DTOs.Order.CreateOrder;
using API.DTOs.Product.AddProduct;
using API.DTOs.Product.GetProduct;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Application
{
    public class DispatcherEngine
    {
        public RequestHandler requestHandler = new RequestHandler();

        public static class Constants
        {
            public const string PRODUCT = "Product";
            public const string CAMPAIGN = "Campaign";
            public const string ORDER = "Order";
        }

        #region Command Dictionary
        public static Dictionary<string, Delegate> _CommandSet;

        public static Dictionary<string, Delegate> CommandSet
        {
            get
            {
                if (_CommandSet == null)
                {
                    _CommandSet = new Dictionary<string, Delegate>();

                    //Product
                    _CommandSet.Add("create_product", new DelegateAddProduct(AddProduct));
                    _CommandSet.Add("get_product_info", new DelegateGetProduct(GetProduct));

                    //Order
                    _CommandSet.Add("create_order", new DelegateCreateOrder(CreateOrder));

                    //Campaign
                    _CommandSet.Add("create_campaign", new DelegateCreateCampaign(CreateCampaign));
                    _CommandSet.Add("get_campaign_info", new DelegateGetCampaign(GetCampaign));

                    //Application 
                    _CommandSet.Add("increase_time", new DelegateIncreaseTime(IncreaseTime));
                }
                return _CommandSet;
            }
        }
        #endregion

        #region Product Commands
        public delegate void DelegateAddProduct(string productCode, string stock, string defaultPrice);
        public delegate void DelegateGetProduct(string productCode);
        #endregion

        #region Order Delegates
        public delegate void DelegateCreateOrder(string productCode, string quantity);
        #endregion

        #region Campaign Delegates
        public delegate void DelegateCreateCampaign(string campaignName, string productCode, string duration, string priceManipulationLimit, string targetSalesCount);
        public delegate void DelegateGetCampaign(string campaignName);
        #endregion

        #region Application Delegates
        public delegate void DelegateIncreaseTime(string timeCount);
        #endregion

        #region Product Methods
        
        public static void AddProduct(string productCode, string stock, string defaultPrice)
        {
            try
            {
                AddProductRequest product = new AddProductRequest
                {
                    Id = productCode,
                    DefaultPrice = Convert.ToDecimal(stock),
                    Stock = Convert.ToInt32(defaultPrice)
                };

                var response = RequestHandler.SendRequest(Constants.PRODUCT, Method.POST, null, product);

                if (response != null)
                {
                    AddProductResponse result = JsonConvert.DeserializeObject<AddProductResponse>(response);
                    if (!result.IsError)
                    {
                        ShowSuccessMessage(string.Format("Product created; code {0}, price {1}, stock {2}", result.Id, result.DefaultPrice, result.Stock));
                    }
                    else
                    {
                        ShowErrorMessage(result.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }
        
        public static void GetProduct(string productCode)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("Id", productCode);

                var response = RequestHandler.SendRequest(Constants.PRODUCT, Method.GET, parameters, null);

                if (response != null)
                {
                    GetProductResponse result = JsonConvert.DeserializeObject<GetProductResponse>(response);
                    if (!result.IsError)
                    {
                        ShowSuccessMessage(string.Format("Product created; code {0}, price {1}, stock {2}", result.Id, result.CurrentPrice, result.Stock));
                    }
                    else
                    {
                        ShowErrorMessage(result.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Order Methods

        public static void CreateOrder(string productCode, string quantity)
        {
            try
            {
                CreateOrderRequest order = new CreateOrderRequest
                {
                    ProductCode = productCode,
                    Quantity = Convert.ToInt32(quantity)
                };

                var response = RequestHandler.SendRequest(Constants.ORDER, Method.POST, null, order);

                if (response != null)
                {
                    CreateOrderResponse result = JsonConvert.DeserializeObject<CreateOrderResponse>(response);
                    if (!result.IsError)
                    {
                        ShowSuccessMessage(string.Format("Order created; product {0}, quantity {1}", result.ProductCode, result.Quantity));
                    }
                    else
                    {
                        ShowErrorMessage(result.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Campaign Methods
        public static void CreateCampaign(string campaignName, string productCode, string duration, string priceManipulationLimit, string targetSalesCount)
        {
            try
            {
                CreateCampaignRequest campaign = new CreateCampaignRequest
                {
                    CampaignName = campaignName,
                    ProductCode = productCode,
                    Duration = Convert.ToInt32(duration),
                    PriceManipulationLimit = Convert.ToDecimal(priceManipulationLimit),
                    TargetSalesCount = Convert.ToInt32(targetSalesCount),
                    CreatedDate = Program.CURRENT_TIME
                };

                var response = RequestHandler.SendRequest(Constants.CAMPAIGN, Method.POST, null, campaign);

                if (response != null)
                {
                    CreateCampaignResponse result = JsonConvert.DeserializeObject<CreateCampaignResponse>(response);
                    if (!result.IsError)
                    {
                        ShowSuccessMessage(string.Format("Campaign created; name {0}, product {1}, duration {2}, limit {3}, target sales count {3}", result.CampaignName, result.ProductCode, result.Duration, result.PriceManipulationLimit, result.TargetSalesCount));
                    }
                    else
                    {
                        ShowErrorMessage(result.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        } 

        public static void GetCampaign(string campaignName)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("CampaignName", campaignName);

                var response = RequestHandler.SendRequest(Constants.CAMPAIGN, Method.GET, parameters, null);

                if (response != null)
                {
                    GetCampaignResponse result = JsonConvert.DeserializeObject<GetCampaignResponse>(response);
                    if (!result.IsError)
                    {
                        ShowSuccessMessage(string.Format("Campaign {0} info; Status {1}, Target Sales {2}, Total Sales {3}, Turnover {4}, Average Item Price {5}", 
                            result.CampaignName, result.IsActive == true ? "Active" : "Ended", result.TargetSalesCount, result.TotalSales, result.Turnover, result.AverageItemPrice == 0 ? "-" : result.AverageItemPrice));
                    }
                    else
                    {
                        ShowErrorMessage(result.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region Application Methods

        public static void IncreaseTime(string timeCount)
        {
            try
            {
                Program.CURRENT_TIME = Program.CURRENT_TIME.AddHours(Convert.ToDouble(timeCount));
                UpdateActiveCampaignRequest request = new UpdateActiveCampaignRequest
                {
                    CurrentDate = Program.CURRENT_TIME.ToString("yyyy-MM-dd HH:mm:ss"),
                    TimeCount = Convert.ToDecimal(timeCount)
                };

                var response = RequestHandler.SendRequest(Constants.CAMPAIGN, Method.PUT, null, request);

                if (response != null)
                {
                    UpdateActiveCampaignResponse result = JsonConvert.DeserializeObject<UpdateActiveCampaignResponse>(response);
                    if (!result.IsError)
                    {
                        ShowSuccessMessage(string.Format("Time is {0}", Program.CURRENT_TIME.ToString("HH:mm")));
                    }
                    else
                    {
                        ShowErrorMessage(result.ErrorMessage);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        public static void ShowErrorMessage(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ClearTables()
        {
            RequestHandler.SendRequest(Constants.CAMPAIGN, Method.DELETE, null, null);
            RequestHandler.SendRequest(Constants.PRODUCT, Method.DELETE, null, null);
            RequestHandler.SendRequest(Constants.ORDER, Method.DELETE, null, null);
        }
    }
}
