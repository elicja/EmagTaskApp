using EmagTask.Interface.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmagTask.Implementation.Api
{
    public class ApiProvider : IApiProvider
    {
        public readonly IRestClient restClient;

        public ApiProvider(string apiUrl)
        {
            restClient = new RestClient(apiUrl);
        }

        public RestRequest GetRequest(string resource, Method method = Method.Get)
        {
            return new RestRequest(resource, method);
        }

        public async Task<RestResponse> ExecuteRequest(RestRequest request)
        {
            return await restClient.ExecutePostAsync(request);
        }

        public async Task<byte[]> DownloadFile(RestRequest request)
        {
            request.AddHeader("Content-Type", "application/octet-stream");
            return await restClient.DownloadDataAsync(request);
        }
    }
}
