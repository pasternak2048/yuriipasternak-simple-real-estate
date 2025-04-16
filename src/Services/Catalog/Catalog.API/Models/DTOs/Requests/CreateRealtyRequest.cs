﻿using Catalog.API.Models.Enums;

namespace Catalog.API.Models.DTOs.Requests
{
	public class CreateRealtyRequest
	{
		public RealtyType Type { get; set; }

		public WallType WallType { get; set; }

		public HeatingType HeatingType { get; set; }

		public double Area { get; set; }

		public int Floor { get; set; }

		public int Rooms { get; set; }

		public int Baths { get; set; }

		public DateTimeOffset BuildDate { get; set; }

		public Address Address { get; set; } = new();
	}
}
