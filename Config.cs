// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResource("roles","Role",new List<string>{ JwtClaimTypes.Role}),
                new IdentityResource("locations","Locations",new List<string>{"location"})
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api1", "My API #1")
                {
                    ApiSecrets={new Secret("api1 secret".Sha256())}  //api验证，用于reference方式
                },
                new ApiResource("api2", "My API #2")
                {
                    ApiSecrets={new Secret("api2 secret".Sha256())}  //api验证，用于reference方式
                },
                new ApiResource("SalesManagementApi","The SalesManagementSystem's Api")
                {
                    ApiSecrets={new Secret("SalesManagementApi secret".Sha256())}
                }
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "console client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "api1",IdentityServerConstants.StandardScopes.OpenId}
                },

                //wpf client,password grant
                new Client
                {
                    ClientId="wpf client",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("wpf secrect".Sha256())
                    },
                     AllowedScopes = {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone
                    }
                },

                 //mvc client,authorization code
                new Client
                {
                    ClientId="mvc client",
                    ClientName="ASP.NET Core MVC Client",

                    AllowedGrantTypes=GrantTypes.CodeAndClientCredentials,

                    AccessTokenType=AccessTokenType.Reference, //可选jwt reference安全性较高，jwt安全性较低

                    ClientSecrets =
                    {
                        new Secret("mvc secret".Sha256())
                    },
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AlwaysIncludeUserClaimsInIdToken=true,

                    AllowOfflineAccess = true,

                    AccessTokenLifetime=15,

                    AllowedScopes = {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone

                    }
                },

                // SPA client using code flow + pkce
                new Client
                {
                    ClientId = "angular-client",
                    ClientName = "Angular SPA Client",
                    ClientUri = "http://localhost:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,
                    RequireConsent=true,
                    AccessTokenLifetime=60*5,
                   

                    RedirectUris =
                    {
                        "http://localhost:4200/signin-oidc",
                        "http://localhost:4200/redirect-silentrenew",
                    },

                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    AllowedCorsOrigins = { "http://localhost:4200" },

                    AllowedScopes = { 
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone 
                    }
                },

                 // SPA client using code flow + pkce
                new Client
                {
                    ClientId = "react-client",
                    ClientName = "React SPA Client",
                    ClientUri = "http://localhost:3000",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,
                    RequireConsent=true,
                    AccessTokenLifetime=60*5,


                    RedirectUris =
                    {
                        "http://localhost:3000/Callback","http://localhost:3000/Renew"
                    },

                    PostLogoutRedirectUris = { "http://localhost:3000"},
                    AllowedCorsOrigins = { "http://localhost:3000","http://127.0.0.1:3000" },

                    AllowedScopes = {
                        "SalesManagementApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                         "roles",
                        "locations"
                    }
                },
                //mvc, hybird flow
                new Client
                {
                    ClientId="hybrid client",
                    ClientName="ASP.NET Core Hybrid",
                    ClientSecrets={new Secret("hybrid secret".Sha256())},

                    AllowedGrantTypes=GrantTypes.Hybrid,
                    RedirectUris=
                    {
                        "http://localhost:7000/signin-oidc"
                    },

                    PostLogoutRedirectUris=
                    {
                        "http://localhost:7000/signout-callback-oidc"
                    },

                    AllowOfflineAccess=true,

                    AlwaysIncludeUserClaimsInIdToken=true,

                    AllowedScopes=
                    {
                         "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "roles",
                        "locations"
                    }
                },

                  //swagger client
                new Client
                {
                    ClientId="swagger_client1",
                    ClientName="Swagger UI client",
                    ClientUri = "http://localhost:7000",


                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,
                    RequireConsent=true,


                    RedirectUris=
                    {
                      "http://localhost:7000/signin-oidc",
                      "http://localhost:7000/redirect-silentrenew",
                    },

                    PostLogoutRedirectUris = { "http://localhost:7000" },
                    AllowedCorsOrigins = { "http://localhost:7000" },

                    AllowedScopes=
                    {
                        "api1",
                        "api2",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "roles",
                        "locations"
                    },
                },

                 //swagger client
                new Client
                {
                    ClientId="swagger_client",
                    ClientName="Swagger UI client",
                    ClientUri = "http://localhost:8080",


                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,
                    RequireConsent=true,


                    RedirectUris=
                    {
                       "http://localhost:8080/oauth2-redirect.html"
                    },

                    PostLogoutRedirectUris = { "http://localhost:8080" },
                    AllowedCorsOrigins = { "http://localhost:8080" },

                    AllowedScopes=
                    {
                        "api1",
                        "api2",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "roles",
                        "locations"
                    },
                },

                new Client
                {
                    ClientId="sales_management_client",
                    ClientName="Sales management system client",
                    ClientUri = "http://localhost:8080",


                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,
                    RequireConsent=true,


                    RedirectUris=
                    {
                       "http://localhost:8080/oauth2-redirect.html"
                    },

                    PostLogoutRedirectUris = { "http://localhost:8080" },
                    AllowedCorsOrigins = { "http://localhost:8080" },

                    AllowedScopes=
                    {
                        "SalesManagementApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "roles",
                        "locations"
                    },
                }

                // MVC client using code flow + pkce
                /*new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "http://localhost:5003/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5003/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api1" }
                },

                // SPA client using code flow + pkce
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "http://localhost:5002/index.html",
                        "http://localhost:5002/callback.html",
                        "http://localhost:5002/silent.html",
                        "http://localhost:5002/popup.html",
                    },

                    PostLogoutRedirectUris = { "http://localhost:5002/index.html" },
                    AllowedCorsOrigins = { "http://localhost:5002" },

                    AllowedScopes = { "openid", "profile", "api1" }
                }*/
            };
    }
}