﻿using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IFirestorageFileHandler
    {
        Task<bool> StoreAsync(IStorageStrategy dto, string? fileUrlPrefix = null, string? directoryNameOnFirestore = null);
    }
}