using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Microservice.IdentityServer;

public static class Config
{
    //****
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermission"}},
            new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
            new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.Email(),
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new []{"role"}}
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalog_fullpermission","Catalog Api için full erişim"),
            new ApiScope("photo_stock_fullpermission","Photo Stock Api için full erişim"),
            new ApiScope("basket_fullpermission","Basket Api için full erişim"),
            new ApiScope("discount_fullpermission","Discount Api için full erişim"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            //new ApiScope("scope1"),
            //new ApiScope("scope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {

            new Client
            {
                ClientId= "WebMvcClient",
                ClientName= "Asp.Net Core MVC",
                ClientSecrets= {new Secret("secret".Sha256())},
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                AllowedScopes= {"catalog_fullpermission","photo_stock_fullpermission",IdentityServerConstants.LocalApi.ScopeName}
            },
            new Client
            {
                ClientId= "WebMvcClientForUser",
                ClientName= "Asp.Net Core MVC",
                AllowOfflineAccess=true,
                ClientSecrets= {new Secret("secret".Sha256())},
                AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                AllowedScopes= {IdentityServerConstants.LocalApi.ScopeName,IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,"roles","catalog_fullpermission","photo_stock_fullpermission","basket_fullpermission","discount_fullpermission"},
                AccessTokenLifetime=1*60*60,
                RefreshTokenExpiration=TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                RefreshTokenUsage=TokenUsage.ReUse
            }




            //// m2m client credentials flow client
            //new Client
            //{
            //    ClientId = "m2m.client",
            //    ClientName = "Client Credentials Client",

            //    AllowedGrantTypes = GrantTypes.ClientCredentials,
            //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

            //    AllowedScopes = { "scope1" }
            //},

            //// interactive client using code flow + pkce
            //new Client
            //{
            //    ClientId = "interactive",
            //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

            //    AllowedGrantTypes = GrantTypes.Code,

            //    RedirectUris = { "https://localhost:44300/signin-oidc" },
            //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
            //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

            //    AllowOfflineAccess = true,
            //    AllowedScopes = { "openid", "profile", "scope2" }
            //},
        };
}
