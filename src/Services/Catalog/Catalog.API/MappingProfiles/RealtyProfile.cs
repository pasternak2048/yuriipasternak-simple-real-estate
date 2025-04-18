﻿using AutoMapper;
using Catalog.API.Models.DTOs.Responses;
using Catalog.API.Models;
using Catalog.API.Models.DTOs.Requests;

namespace Catalog.API.MappingProfiles
{
	public class RealtyProfile : Profile
	{
		public RealtyProfile()
		{
			CreateMap<Realty, RealtyResponse>()
				.ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => FormatAddress(src.Address)));

			CreateMap<CreateRealtyRequest, Realty>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.Status, opt => opt.Ignore())
			.ForMember(dest => dest.BuildDate, opt => opt.MapFrom(src => src.BuildDate.UtcDateTime));

			CreateMap<UpdateRealtyRequest, Realty>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.BuildDate, opt => opt.MapFrom(src => src.BuildDate.UtcDateTime));
		}

		private static string FormatAddress(Address address)
		{
			if (address == null) return string.Empty;

			return $"{address.Street}, {address.City}, {address.Region}, {address.ZipCode}".Trim().Replace("  ", " ");
		}
	}
}
