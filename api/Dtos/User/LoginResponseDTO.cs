﻿namespace api.Dtos.User
{
    public class LoginResponseDTO
    {
        public string Email { get; set; }
        //public string accessToken { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }


    }
}
