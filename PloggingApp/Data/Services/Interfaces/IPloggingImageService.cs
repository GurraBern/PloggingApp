﻿using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces;

public interface IPloggingImageService
{
    Task<PloggingImage> SaveImage(string imagePath);
}
