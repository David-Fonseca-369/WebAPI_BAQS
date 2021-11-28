using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.DTOs.Login;

namespace WebAPI_BAQS.Helpers
{
    public class AuthenticationWebAPI
    {
        public static async Task<bool> AutheticationAD(LoginDTO loginDTO)
        {
            var client = new RestClient("https://eis-latam.info/WebService/api/Servicios/AutenticacionAD");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(loginDTO);

            IRestResponse response = await client.ExecuteAsync(request);

            if (response.StatusDescription == "OK")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
