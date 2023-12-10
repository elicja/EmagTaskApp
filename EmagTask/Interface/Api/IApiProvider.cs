using RestSharp;
using System.Threading.Tasks;

namespace EmagTask.Interface.Api
{
    public interface IApiProvider
    {
        RestRequest GetRequest(string resource, Method method = Method.Get);
        Task<RestResponse> ExecuteRequest(RestRequest request);
        Task<byte[]> DownloadFile(RestRequest request);
    }
}
