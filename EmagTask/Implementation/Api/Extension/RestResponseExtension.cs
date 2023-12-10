using Newtonsoft.Json;
using RestSharp;

namespace EmagTask.Implementation.Api.Extension
{
    public static class RestResponseExtension
    {
        public static T Deserialize<T>(this RestResponse source)
        {
            return JsonConvert.DeserializeObject<T>(source.Content);
        }
    }
}
