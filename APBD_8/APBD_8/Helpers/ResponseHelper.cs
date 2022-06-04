using System;
using System.Collections.Generic;
using System.Net;

namespace APBD_8.Helpers
{
    public class ResponseHelper
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }

        public object? ResultObject { get; set; }
        public IEnumerable<object>? ResultDataCollection { get; set; }
        public Guid? ResultGuid { get; set; }

        public ResponseHelper(HttpStatusCode statusCode, string? message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public ResponseHelper(HttpStatusCode statusCode, IEnumerable<object>? resultDataCollection)
        {
            StatusCode = statusCode;
            ResultDataCollection = resultDataCollection;
        }

        public ResponseHelper(HttpStatusCode statusCode, string? token, Guid resultGuid)
        {
            StatusCode = statusCode;
            Message = token;
            ResultGuid = resultGuid;
        }

        public ResponseHelper(HttpStatusCode statusCode, object? responseObj)
        {
            StatusCode = statusCode;
            ResultObject = responseObj;
        }
    }
}
