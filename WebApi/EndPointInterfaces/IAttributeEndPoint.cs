﻿using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.EndPointInterfaces
{
    public interface IAttributeEndPoint
    {
        [Get("/odata/Attribute")]
        Task<HttpResponseMessage> GetAsync();

        [Get("/odata/Attribute?$filter=ItemCode eq '{ItemCode}'")]
        Task<HttpResponseMessage> GetByItemCodeAsync([AliasAs("ItemCode")] string itemCode);

        [Get("/odata/Attribute?$filter=LanguageCode eq '{LanguageCode}'")]
        Task<HttpResponseMessage> GetByLanguageCodeAsync([AliasAs("LanguageCode")] string languageCode);

        [Get("/odata/Attribute?$filter=AttributeType eq '{AttributeType}'")]
        Task<HttpResponseMessage> GetByAttributeTypeAsync([AliasAs("AttributeType")] string attributeType);

        [Get("/odata/Attribute?$filter=AttributeType eq '{AttributeType}' and LanguageCode eq '{LanguageCode}'")]        
        Task<HttpResponseMessage> GetByAttributeTypeLanguageCodeAsync(
            [AliasAs("AttributeType")] string attributeType, 
            [AliasAs("LanguageCode")] string languageCode
            );
    }
}
