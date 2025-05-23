﻿using GLORIA.BuildingBlocks.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GLORIA.BuildingBlocks.Extensions.Application
{
	public static class IdentityExtensions
	{
		public static void AddCurrentUser(this IServiceCollection services)
		{
			services.AddScoped<IUserIdentityProvider, UserIdentityProvider>();
		}
	}
}
