using GtvApiHub.WebApi.DTOs;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.Objects
{
    /// <summary>
    /// Reqest object for a token response.
    /// </summary>
    public class TokenRequestDto : IBaseDto
    {
        [AliasAs("username")]
        public string Username { get; set; }

        [AliasAs("password")]
        public string Password { get; set; }

        [AliasAs("grant_type")]
        public string GrantType { get => "password"; }

    }

}
