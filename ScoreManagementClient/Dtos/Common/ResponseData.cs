namespace ScoreManagementClient.Dtos.Common
{
    public class ResponseData<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public List<ErrorMessage> Erorrs { get; set; }
    }

    public class ErrorMessage
    {
        public string Key { get; set; }
        public string Message { get; set; }
    }
}
