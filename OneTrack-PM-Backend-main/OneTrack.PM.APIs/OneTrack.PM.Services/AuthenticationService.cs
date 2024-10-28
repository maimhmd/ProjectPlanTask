using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OneTrack.PM.Core;
using OneTrack.PM.Core.Services;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models.DB;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OneTrack.PM.APIs.Utility
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly IOneTrackPMContextProcedures _procedures;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(IConfiguration configuration, IOneTrackPMContextProcedures procedures, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            _procedures = procedures;
            _unitOfWork = unitOfWork;
        }

        public async Task<Tokens> CreateToken(SecUser user, string fireBaseToken)
        {
            return await GenerateJWTTokens(user, fireBaseToken);
        }

        public async Task<Tokens> CreateRefreshToken(SecUser user, string fireBaseToken)
        {
            return await GenerateJWTTokens(user, fireBaseToken);
        }

        public async Task<Tokens> GenerateJWTTokens(SecUser user, string fireBaseToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user, fireBaseToken);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var accessToken = tokenHandler.WriteToken(tokenOptions);
            var refreshToken = GenerateRefreshToken();
            return new Tokens { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("SecurityKey").Value);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.GetSection("ValidIssuer").Value,
                ValidAudience = _jwtSettings.GetSection("ValidAudience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (principal == null)
            {
                throw new SecurityTokenException("Invalid token");
            }
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }


            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.GetSection("SecurityKey").Value));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(SecUser user, string fire_base_token)
        {
            var permissions = await _procedures.SP_SEC_SelectUserPermissionAsync(user.Id);
            List<UserPermissions> lst = new List<UserPermissions>();
            foreach (var i in permissions)
            {
                lst.Add(new UserPermissions(i.ModuleId, i.ModuleName, i.PageName, i.Url, i.ParentForm, i.ParentUrl, i._Read ?? false,
                 i._Create ?? false, i._Update ?? false, i._Delete ?? false, i._Export ?? false, i._Print ?? false, i._Approve ?? false, i._Freeze ?? false));
            }
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("contactId", user.ContactId.ToString()),
                new Claim("code", user.Contact.Code.ToString()),
                new Claim("barcode", user.Contact.Barcode),
                new Claim("fullName",(user.Contact.TitleId!=null?user.Contact.Title.Name+" ":"")+ user.Contact.FullName),
                new Claim("avatar", user.Contact.Avatar??string.Empty),
                new Claim("email", user.Contact.Email),
                new Claim("mainModuleId", user.Group.MainModuleId.ToString()),
                new Claim("permissions",UserPermissions.SerializePermissionsList(lst)),
                new Claim("fireBaseToken",fire_base_token!=null?fire_base_token:string.Empty),
            };
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
            (
                issuer: _jwtSettings.GetSection("ValidIssuer").Value,
                audience: _jwtSettings.GetSection("ValidAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.GetSection("DurationInDays").Value)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        public bool IsTokenValid(string token)
        {
            try
            {
                var Key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("SecurityKey").Value);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.GetSection("ValidIssuer").Value,
                    ValidAudience = _jwtSettings.GetSection("ValidAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ClockSkew = TimeSpan.Zero
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            { return false; }
        }

        public TokenClaims DecodeToken(string authorizationValue)
        {
            var headerValue = AuthenticationHeaderValue.Parse(authorizationValue);
            var parameter = headerValue.Parameter;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(parameter);
            return new TokenClaims
            {
                Id = int.Parse(token.Claims.First(x => x.Type == "id").Value),
                ContactId = int.Parse(token.Claims.First(x => x.Type == "contactId").Value),
                Code = int.Parse(token.Claims.First(x => x.Type == "code").Value),
                Barcode = token.Claims.First(x => x.Type == "barcode").Value,
                FullName = token.Claims.First(x => x.Type == "fullName").Value,
                Avatar = token.Claims.First(x => x.Type == "avatar").Value,
                MainModuleId = int.Parse(token.Claims.First(x => x.Type == "mainModuleId").Value),
                Email = token.Claims.First(x => x.Type == "email").Value,
                FireBaseToken = token.Claims.First(x => x.Type == "fireBaseToken").Value
            };
        }
    }
}
