using RestSharp;
using System.Collections.Generic;

namespace Application
{
    public class RequestHandler
    {
        public const string BASE_SERVICE_URL = "https://localhost:44369/api/";
        public const string APPLICATION_CONTEXT = "appication/json; charset=utf-8";

        public static string SendRequest(string restRequest, Method httpMethod, Dictionary<string, object> parameterList, object entity)
        {
            var client = new RestClient(BASE_SERVICE_URL);
            var request = new RestRequest(restRequest);
            request.Method = httpMethod;

            if(httpMethod == Method.GET)
            {
                foreach (var item in parameterList)
                    request.AddParameter(item.Key, item.Value);
            }

            else if(httpMethod == Method.POST || httpMethod == Method.PUT)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddBody(entity);
            }
            else
            {
                // DELETE
            }


            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                return rawResponse;
            }
            else
            {
                return null;
            }
        }
    }
}
