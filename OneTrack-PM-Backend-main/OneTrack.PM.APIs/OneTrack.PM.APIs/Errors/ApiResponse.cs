namespace OneTrack.PM.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ApiResponse(int statusCode, string message = null, object data = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Data = data;
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                401 => "Authorized, you are not",
                400 => "A bad request, you have made",
                404 => "Resource was not found",
                500 => "Error",
                _ => null
            };
        }
    }
}
