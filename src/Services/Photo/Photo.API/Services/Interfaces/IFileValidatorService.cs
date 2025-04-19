﻿namespace Photo.API.Services.Interfaces
{
	public interface IFileValidatorService
	{
		bool IsValidExtension(string fileName);

		bool IsValidMimeType(string contentType);

		bool IsValid(IFormFile file);
	}
}
