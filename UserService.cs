using Microsoft.IdentityModel.Tokens;
using Part_3_Lesson_4.Controllers.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Part_3_Lesson_4
{
    public class UserService:IUserServices
    {
        private IDictionary<string, string> users = new Dictionary<string, string>()
        {
            {"test","test" }
        };
        public  const string SecreteCode = "aaaaa";
    public string Authentication(string name , string pass)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pass))
            {
                return string.Empty;
            }
            int i = 0;
            foreach( var item in users)
            {
                i++;
                if (string.CompareOrdinal(item.Key, name) == 0 || string.CompareOrdinal(item.Key, pass) == 0)
                {
                    return GenerateJwToken(i);
                }
            }
            return string.Empty;
        }

        public string GenerateJwToken(int id)
        {
            JwtSecurityTokenHandler jwt = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(SecreteCode);//переводим в байты наш секретный код
            SecurityTokenDescriptor securityToken = new SecurityTokenDescriptor()//класс который описывает наш токен (что он будет делать)
            {
                Subject = new ClaimsIdentity(new Claim[]//все что может делать пользователь храниться вв паре ключ значений 
                {
                    new Claim(ClaimTypes.Name, id.ToString())// а вот и наша пара имя и ид
                }),
                Expires = DateTime.UtcNow.AddMinutes(2),//время жизни токена 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)// применяем алгоритмы шифрования к нашему ключу 


            };

            SecurityToken security = jwt.CreateToken(securityToken);
            return jwt.WriteToken(security);
        }

    }
}
