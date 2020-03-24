using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AccessDataApi.Authentication
{
    public class Credentials
    {
        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }


        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
