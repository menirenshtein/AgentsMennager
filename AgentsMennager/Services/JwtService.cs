//using AgentsMennager.Controllers;
//using AgentsMennager.DTO;
//using AgentsMennager.Models;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.IdentityModel.Tokens;
//using NuGet.Common;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace LinkOut.Services
//{
//    public class JwtService
//    {
//        private IConfiguration config;
//        public JwtService(IConfiguration configuration) { config = configuration; }

//        public string genJwToken(TokenDTO login)
//        {
//            string? key = config.GetValue("jwt:key", string.Empty);
//            int exp = config.GetValue("jwt:exp", 3);

//            SymmetricSecurityKey secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
//            SigningCredentials crd = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
//            Claim[] claims = new[]
//            {
//                new Claim("token",login.Token.ToString()),
//            };
//            JwtSecurityToken token = new JwtSecurityToken(
//                expires: DateTime.Now.AddMinutes((double)exp),
//                signingCredentials: crd,
//                claims: claims
//            );

//            string tkn = new JwtSecurityTokenHandler().WriteToken(token);
//            return tkn;
//        }

//    }
//}
