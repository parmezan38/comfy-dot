using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "colors",
                    UserClaims = { "color1", "color2" }
                }
            };


        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("comfyDotApi", "Comfy Dot API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "js",
                    ClientName = "Comfy Dot Chat",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireConsent = false,
                    RequireClientSecret = false, // TODO: Add Client Secret
                    
                    RedirectUris = {
                        "http://localhost:4200/callback",
                        "http://localhost:4200/silent"
                    },
                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    AllowedCorsOrigins =     { "http://localhost:4200" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "comfyDotApi",
                        "colors"
                    }
                }
            };
    }
}