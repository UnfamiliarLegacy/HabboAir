using System;

namespace HabBridge.Api.Services.Hotel
{
    public class HotelHttpException : Exception
    {
        public HotelHttpException(string message) : base(message)
        {
            Content = null;
        }
        
        public HotelHttpException(string message, string content) : base(message)
        {
            Content = content;
        }

        public string Content { get; }
    }
}