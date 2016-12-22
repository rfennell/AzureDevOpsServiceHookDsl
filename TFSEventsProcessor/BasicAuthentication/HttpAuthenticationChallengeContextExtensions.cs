﻿//------------------------------------------------------------------------------------------------- 
// <copyright file="HttpAuthenticationChallengeContextExtensions.cs" company="Black Marble">
// Copyright (c) Black Marble. All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;
using BasicAuthentication.Results;

namespace BasicAuthentication.Filters
{
    public static class HttpAuthenticationChallengeContextExtensions
    {
        public static void ChallengeWith(this HttpAuthenticationChallengeContext context, string scheme)
        {
            ChallengeWith(context, new AuthenticationHeaderValue(scheme));
        }

        public static void ChallengeWith(this HttpAuthenticationChallengeContext context, string scheme, string parameter)
        {
            ChallengeWith(context, new AuthenticationHeaderValue(scheme, parameter));
        }

        public static void ChallengeWith(this HttpAuthenticationChallengeContext context, AuthenticationHeaderValue challenge)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
        }
    }
}
