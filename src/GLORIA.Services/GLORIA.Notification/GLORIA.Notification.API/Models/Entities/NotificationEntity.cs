﻿using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.Contracts.Dtos.Notification;
using GLORIA.Contracts.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GLORIA.Notification.API.Models.Entities
{
	public class NotificationEntity : IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid Id { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid UserId { get; set; }

		public NotificationEventType EventType { get; set; }

		public string Title { get; set; } = string.Empty;

		public string Message { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public bool IsRead { get; set; } = false;

        public NotificationObject? Object { get; set; }
    }
}
