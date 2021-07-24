using System.Net;
using System.Runtime.Serialization;

namespace Common.DataModel.DTO.Communication
{

    [DataContract]
    public class BaseResponse<T>
    {
        public BaseResponse()
        {

        }

        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public T Resource { get; private set; }

        public BaseResponse(T resource)
        {
            Message = string.Empty;
            Code = (int)HttpStatusCode.OK;
            Resource = resource;
        }

        public BaseResponse(string message)
        {
            Message = message;
            Code = (int)HttpStatusCode.BadRequest;
            Resource = default;
        }

        public BaseResponse(T resource, string message, HttpStatusCode httpStatusCode)
        {
            Message = message;
            Code = (int)httpStatusCode;
            Resource = resource;
        }

        public BaseResponse<T> InternalServerError()
        {
            Message = HttpStatusCode.InternalServerError.ToString();
            Code = (int)HttpStatusCode.InternalServerError;
            Resource = default;

            return this;
        }

        public bool HasError() {
            return !string.IsNullOrEmpty(this.Message);
        }
    }
}
