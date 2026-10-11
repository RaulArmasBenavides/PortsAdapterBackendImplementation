using System.IdentityModel.Tokens.Jwt;
namespace CompanyName.DDD.API.Extensions
{
    public class UserDataRetriever
    {
        public string UserCode { get; private set; }
        public string UserName { get; private set; }
        public UserDataRetriever(string autorizationHeader)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(autorizationHeader[7..]);
            UserName = jwtSecurityToken.Claims.Single(x => x.Type.Equals("preferred_username")).Value;
            UserCode = jwtSecurityToken.Claims.Single(x => x.Type.Equals("sid")).Value;
        }
    }
}