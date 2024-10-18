﻿using Google.Cloud.Firestore;
using GtvApiHub.Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.DTOs
{
    /// <summary>
    /// Attribute DTO represents product attributes from Api.
    /// </summary>
    [FirestoreData]
    public record AttributeDto : IBaseDto, IResponseDto, IFirestoreDto
    {
        [FirestoreProperty]
        public string AttributeName { get; init; }

        [FirestoreProperty]
        public string AttributeType { get; init; }

        [FirestoreProperty]
        public string ItemCode { get; init; }

        [FirestoreProperty]
        public string LanguageCode { get; init; }

        [FirestoreProperty]
        public string Value { get; init; }

        [FirestoreProperty]
        public string FileHandler { get; init; }

        public string CollectionName { get => "itemsAttributes"; }
        public string DocumentUniqueField { get => ItemCode; }
    }
}
