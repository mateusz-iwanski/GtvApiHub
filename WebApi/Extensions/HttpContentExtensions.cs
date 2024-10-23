using FirebaseManager.Firestore;
using GtvApiHub.WebApi.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.Extensions
{
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Deserialize Json HttpContent from GTV Api to Firestore DTOs
        /// </summary>
        /// <typeparam name="T">IFirestoreDto</typeparam>
        /// <param name="responseContent">HttpContent</param>
        /// <returns>IEnumerable<T></returns>
        public async static Task<IEnumerable<T>> GetListObjectAsync<T>(this HttpContent responseContent) where T : IFirestoreDto
        {
            var tokenResponse = await responseContent.ReadAsStringAsync();

            var deserializedResponseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(tokenResponse);

            var listItemDto = JsonConvert.DeserializeObject<IEnumerable<T>>(deserializedResponseDict["value"].ToString());

            return listItemDto;
        }        
    }
}
