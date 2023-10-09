using System.Collections.Generic;

namespace dotnet_resources_api.Wrappers
{
    public class ResourcesResponse<T>
    {
        public ResourcesResponse()
        {
        }
        public ResourcesResponse(T data, string message = null, List<string> errors = null)
        {
            Succeeded = errors == null ? true : false;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
