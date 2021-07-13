using Newtonsoft.Json;

namespace Common.DataModel.Domain.Logger
{
    public class RequestResponseDto
    {
        public RequestResponseDto(object response, params object[] request)
        {
            this.Request = request;
            this.Response = response;
        }
        public RequestResponseDto()
        {

        }

        public object[] Request { set; get; }

        public object Response { set; get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
