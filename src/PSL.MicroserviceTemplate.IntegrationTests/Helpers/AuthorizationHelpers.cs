using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PSL.MicroserviceTemplate.IntegrationTests.Helpers;

public static class AuthorizationHelpers
{
    public enum ClientAuthenticatedUserTypes
    {
        None,
        BasicAccess,
    }

    internal static HttpClient CreateClientWithAuthenticatedUser<TEntryPoint>(
        this WebApplicationFactory<TEntryPoint> apiFactory,
        ClientAuthenticatedUserTypes userType = ClientAuthenticatedUserTypes.BasicAccess) where TEntryPoint : class
    {
        var client = apiFactory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication("Test")
                    .AddScheme<TestAuthenticationSchemeOptions, TestSchemeAuthenticationHandler>("Test", opts =>
                    {
                        opts.AuthenicationScheme = userType;
                    });
        })).CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        return client;
    }

    internal class TestSchemeAuthenticationHandler : AuthenticationHandler<TestAuthenticationSchemeOptions>
    {
        public TestSchemeAuthenticationHandler(IOptionsMonitor<TestAuthenticationSchemeOptions> options,
                                               ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, "d152587e-900d-4200-92f9-ecd49e0db62c"),
                new Claim(JwtClaimTypes.Name, "Test User with Read"),
            };

            claims.AddRange(ResolveScopesForAuthenticationType(Options.AuthenicationScheme).Select(s => new Claim(JwtClaimTypes.Scope, s)));

            claims.Add(new Claim("location_id", "70d112e4-f57c-47dd-80ef-d00a188cd68f"));

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");
            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }

        private static string[] ResolveScopesForAuthenticationType(ClientAuthenticatedUserTypes authenticatedUserType)
        {
            return authenticatedUserType switch
            {
                ClientAuthenticatedUserTypes.BasicAccess => new[] { "hxtemplate.basicaccess" },
                _ => Array.Empty<string>()
            };
        }
    }

    internal class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public ClientAuthenticatedUserTypes AuthenicationScheme { get; set; }
    }
}

