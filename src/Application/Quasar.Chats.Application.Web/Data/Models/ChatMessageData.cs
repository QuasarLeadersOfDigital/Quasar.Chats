using System;

namespace Quasar.Chats.Application.Web.Data.Models
{
	public class ChatMessageData
	{
		public string Content { get; set; }

		public Guid ChatId { get; set; }

		public Guid Id { get; set; }

		public DateTimeOffset SendDate { get; set; }

		public string State { get; set; }
	}
}