﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNetCore.IdentityServer4.Auth.Models;
using AspNetCore.IdentityServer4.WebApi.Models;
using AspNetCore.IdentityServer4.WebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AspNetCore.IdentityServer4.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService auth = null;

        public AuthController(
            IAuthService authService)
        {
            this.auth = authService;
        }

        // GET api/values
        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<JObject> SignIn(LdapUser user)
        {
            var tokenResponse = await this.auth.SignInAsync(user.Username, user.Password);


            if (!tokenResponse.IsError)
            {
                return tokenResponse.Json;
            }

            this.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return null;
        }

        [HttpPost("UserInfo")]
        public async Task<JObject> UserInfo([FromBody] string accessToken)
        {
            //string accessToken = string.Empty;
            //var authHeaderVal = this.Request.Headers["Authorization"];
            //if (!string.IsNullOrEmpty(authHeaderVal))
            //{
            //    accessToken = authHeaderVal.ToString().Replace("Bearer ", "").Replace("bearer ", "");
            //}

            var userInfoResponse = await this.auth.GetUserInfoAsync(accessToken);

            if (!userInfoResponse.IsError)
            {
                return userInfoResponse.Json;
            }

            this.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return null;
        }
    }
}
