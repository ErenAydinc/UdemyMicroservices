using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microservice.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; private set; }

        //sonuç dödürürken json çıktısında bu gözükmesin sadece kodda kolaylık sağlaması için yazıyoruz bu bilgiyi
        [JsonIgnore]
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool IsSuccessful { get; set; }
        public List<string> Error { get; private set; }
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T> { Error = errors, StatusCode = statusCode, IsSuccessful = false };
        }

        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T> { Error = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}
