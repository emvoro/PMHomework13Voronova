using Microsoft.AspNetCore.Authentication;

namespace DepsWebApp.Authentication
{
#pragma warning disable CS1591 
    public class CustomAuthSchemaOptions : AuthenticationSchemeOptions
    {
        public CustomAuthSchemaOptions()
        {
            ClaimsIssuer = CustomAuthSchema.Issuer;
        }
    }
#pragma warning restore CS1591
}
