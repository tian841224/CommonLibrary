using CommonLibrary.DTOs;
using CommonLibrary.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CommonLibrary.Services
{
    /// <summary>
    /// 身分認證
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private const string ADMIN_USER_REFRESH_TOKEN_KEY_PRE = "ADMIN_USER_REFRESH_TOKEN_KEY_PRE:";
        private const string ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE = "ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE:";
        private const string CAPTCHA_CODE_PRE = "CAPTCHA_CODE_PRE:";
        private readonly ILogger<IdentityService> _log;
        private readonly JwtConfig _jwtConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRedisService _redisService;

        public IdentityService(IOptions<JwtConfig> jwtConfig, ILogger<IdentityService> log, IHttpContextAccessor httpContextAccessor, IRedisService redisService)
        {
            _jwtConfig = jwtConfig.Value;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
            _redisService = redisService;
        }

        /// <summary>
        /// 取得使用者資訊
        /// </summary>
        /// <returns></returns>
        public ClaimDto GetUser()
        {
            var JwtClaims = _httpContextAccessor.HttpContext?.User?.Claims.ToList();

            var claimsDto = new ClaimDto();

            if (JwtClaims != null)
            {
                claimsDto = new ClaimDto
                {
                    UserId = JwtClaims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id)?.Value ?? string.Empty,
                    RoleNane = JwtClaims.FirstOrDefault(c => c.Type == JwtClaimTypes.Role)?.Value ?? string.Empty,
                    UserNane = JwtClaims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? string.Empty,
                    Email = JwtClaims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email
                                               || c.Type == ClaimTypes.Email
                                               || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? string.Empty,

                    //ReferenceTokenId = JwtClaims.FirstOrDefault(c => c.Type == JwtClaimTypes.ReferenceTokenId)?.Value ?? string.Empty,
                    RoleId = JwtClaims.FirstOrDefault(c => c.Type == "RoleId")?.Value ?? string.Empty,
                    Account = JwtClaims.FirstOrDefault(c => c.Type == "Account")?.Value ?? string.Empty,
                };
            }
            return claimsDto;
        }

        /// <summary>
        /// 生成Jwt Token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string GenerateToken(GenerateTokenDto dto)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Id,dto.Id.ToString()),
                    new Claim(JwtClaimTypes.Role,dto.Claims.RoleNane),
                    new Claim(JwtClaimTypes.Name,dto.Claims.UserNane),
                    new Claim(JwtClaimTypes.Email,dto.Claims.Email),
                    //new Claim(JwtClaimTypes.ReferenceTokenId,dto.RefreshToken),
                    new Claim("Account",dto.Claims.Account),
                    new Claim("RoleId",dto.Claims.RoleId),
                };

                var securityToken = new JwtSecurityToken(
                    issuer: _jwtConfig.Issuer,
                    audience: _jwtConfig.Audience,
                    claims: claims,
                    notBefore: _jwtConfig.NotBefore,
                    expires: _jwtConfig.Expiration,
                    signingCredentials: _jwtConfig.SigningCredentials
                );

                var access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);

                ////設定RefreshToken
                //var adminKey = ADMIN_USER_REFRESH_TOKEN_KEY_PRE + dto.Id;
                //await redisDb.SetAddAsync(adminKey, dto.RefreshToken);

                //var adminUserIdKey = ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE + dto.RefreshToken;
                //await redisDb.SetAddAsync(adminUserIdKey, dto.Id);

                return access_token;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 驗證驗證碼
        /// </summary>
        /// <param name="captcha"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task VerifyCaptchaAsync(string captcha, string captchaUserInput)
        {
            var cacheKey = CAPTCHA_CODE_PRE + captcha;
            var cacheValue = await _redisService.redisDb.StringGetAsync(cacheKey);
            if (string.IsNullOrEmpty(cacheValue))
                throw new Exception("驗證碼已過期");

            var verifyResult = cacheValue.ToString().Equals(captchaUserInput, StringComparison.OrdinalIgnoreCase);

            await _redisService.redisDb.StringGetDeleteAsync(cacheKey);

            if (verifyResult == false)
                throw new Exception("圖形密碼不正確");
        }

        /// <summary>
        /// 取得驗證碼
        /// </summary>
        /// <returns></returns>
        public CaptchaDto GetCaptcha()
        {
            // 生成驗證碼
            var Captcha = GenerateCaptcha();

            var result = new CaptchaDto
            {
                CaptchaCode = Guid.NewGuid().ToString("N").ToUpper(),
                ImageBase64 = "data:image/png;base64," + Captcha.CaptchaBase64Data
            };

            var cacheKey = CAPTCHA_CODE_PRE + result.CaptchaCode;

            //await redisDb.StringSetAsync(cacheKey, Captcha.CaptchaCode, new TimeSpan(0, 5, 0));

            return result;
        }

        private CaptchaResult GenerateCaptcha(int width = 100, int height = 36)
        {
            try
            {
                string captchaText = GenerateCode();

                using (var bitmap = new SKBitmap(width, height))
                using (var canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear(SKColors.White);

                    // 增加背景燥點
                    // AddNoise(canvas, width, height);

                    // 設定字體樣式
                    var typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);

                    for (int i = 0; i < captchaText.Length; i++)
                    {
                        float fontSize = height * 0.7f; // 調整字體大小
                        using (var paint = new SKPaint
                        {
                            Color = GetRandomColor(),
                            IsAntialias = true,
                            Style = SKPaintStyle.Fill,
                            TextSize = fontSize,
                            Typeface = typeface
                        })
                        {
                            float charWidth = width / captchaText.Length;
                            float x = i * charWidth;
                            float y = (height + fontSize) / 2 - paint.FontMetrics.Descent;

                            canvas.DrawText(captchaText[i].ToString(), x, y, paint);
                        }
                    }

                    using (var image = SKImage.FromBitmap(bitmap))
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    using (var ms = new MemoryStream())
                    {
                        data.SaveTo(ms);
                        return new CaptchaResult
                        {
                            CaptchaCode = captchaText,
                            CaptchaByteData = ms.ToArray()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 亂數產生顏色
        /// </summary>
        /// <returns></returns>
        private static SKColor GetRandomColor()
        {
            Random random = new Random();
            return new SKColor((byte)random.Next(0, 256), (byte)random.Next(0, 256), (byte)random.Next(0, 256));
        }

        /// <summary>
        /// 亂數產生驗證碼
        /// </summary>
        /// <returns></returns>
        private static string GenerateCode(int length = 4)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }
            return new string(code);
        }
    }
}
