﻿using AutoMapper;
using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Exceptions;
using GLORIA.BuildingBlocks.Identity;
using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Dtos.DocumentMetadata;
using GLORIA.Contracts.Enums;
using GLORIA.DocumentMetadata.API.Models.Entities;

namespace GLORIA.DocumentMetadata.API.Services
{
	public class DocumentMetadataService : IGenericService<DocumentMetadataResponse, DocumentMetadataCreateRequest, DocumentMetadataUpdateRequest, DocumentMetadataFilters>
	{
		private readonly IGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters> _repository;
		private readonly IMapper _mapper;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public DocumentMetadataService(IGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters> repository, IMapper mapper, IUserIdentityProvider userIdentityProvider)
		{
			_repository = repository;
			_mapper = mapper;
			_userIdentityProvider = userIdentityProvider;
		}

		public async Task<DocumentMetadataResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetByIdAsync(id, cancellationToken);

			if(entity is null)
			{
				return null;
			}

			if(entity.DocumentType != DocumentType.RealtyImage && entity.DocumentType != DocumentType.UserAvatar)
			{
				if(entity.OwnerUserId != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				{
					throw new ForbiddenAccessException("You are not owner of this document.");
				}
			}

			return _mapper.Map<DocumentMetadataResponse>(entity);
		}

		public async Task<PaginatedResult<DocumentMetadataResponse>> GetPaginatedAsync(DocumentMetadataFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var currentUserId = _userIdentityProvider.UserId;

			var isPhotoOrAvatar = filters.DocumentType == DocumentType.RealtyImage
							   || filters.DocumentType == DocumentType.UserAvatar;

			if (!_userIdentityProvider.IsAdmin && !isPhotoOrAvatar)
			{
				if(currentUserId == null)
				{
					throw new UnauthorizedException("Unauthorized access");
				}

				filters.OwnerUserId = currentUserId;
			}

			var result = await _repository.GetPaginatedAsync(filters, pagination, cancellationToken);
			var mapped = result.Data.Select(_mapper.Map<DocumentMetadataResponse>);
			return new PaginatedResult<DocumentMetadataResponse>(pagination.PageIndex, pagination.PageSize, result.Count, mapped);
		}

		public async Task CreateAsync(DocumentMetadataCreateRequest request, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<DocumentMetadataEntity>(request);
			entity.Id = Guid.NewGuid();
			entity.CreatedBy = _userIdentityProvider.UserId.GetValueOrDefault();
			entity.OwnerUserId = _userIdentityProvider.UserId;
			entity.CreatedAt = DateTime.UtcNow;
			await _repository.CreateAsync(entity, cancellationToken);
		}

		public async Task UpdateAsync(Guid id, DocumentMetadataUpdateRequest request, CancellationToken cancellationToken)
		{
			var existing = await _repository.GetByIdAsync(id, cancellationToken)
				?? throw new NotFoundException("Metadata not found");

			if (existing.CreatedBy != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner of this photo.");

			_mapper.Map(request, existing);

			existing.ModifiedBy = _userIdentityProvider.UserId;
			existing.ModifiedAt = DateTime.UtcNow;
			await _repository.UpdateAsync(id, existing, cancellationToken);
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			var existing = await _repository.GetByIdAsync(id, cancellationToken)
				?? throw new NotFoundException("Metadata not found");

			if (existing.CreatedBy != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner of this photo.");

			await _repository.DeleteAsync(id, cancellationToken);
		}
	}
}
