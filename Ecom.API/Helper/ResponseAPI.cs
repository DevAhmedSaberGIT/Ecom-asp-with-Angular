﻿using Microsoft.AspNetCore.Http;

namespace Ecom.API.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(StatusCode);
        }

        private string GetMessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Done",
                400 => "Bad Request",
                401 => "Un Authorized",
                404 => "not found resource",
                500 => "Server Error",
                _ => null,
            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
