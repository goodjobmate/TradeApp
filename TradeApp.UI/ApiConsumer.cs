using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace TradeApp.UI
{
    public static class ApiConsumer
    {
        public static IRestResponse<T> Put<T>(object objectToUpdate, string apiEndPoint)
        {
            var client = new RestClient();
            var request = new RestRequest(apiEndPoint, Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(objectToUpdate);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute<T>(request);
            return response;
        }

        public static IRestResponse<T> Get<T>(string apiEndPoint)
        {
            var client = new RestClient();
            var request = new RestRequest(apiEndPoint, Method.GET);

            var response = client.Execute<T>(request);
            response.Data = JsonConvert.DeserializeObject<T>(response.Content);
            return response;
        }

        public static IRestResponse<T> Post<T>(object objectToCreate, string apiEndPoint)
        {
            var client = new RestClient();
            var request = new RestRequest(apiEndPoint, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(objectToCreate);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute<T>(request);
            return response;
        }

        public static IRestResponse<T> Delete<T>(string apiEndPoint)
        {
            var client = new RestClient();
            var request = new RestRequest(apiEndPoint, Method.DELETE);

            var response = client.Execute<T>(request);
            return response;
        }
    }
}
