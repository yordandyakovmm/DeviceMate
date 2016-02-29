using DeviceMate.Models.Domain.Interfaces;

namespace DeviceMate.Models.Domain
{
    public class ResponseMessage: IResponseMessage
    {
        public string Message { get; set; }
    }
}
