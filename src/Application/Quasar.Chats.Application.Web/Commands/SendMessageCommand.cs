using System;

namespace Quasar.Chats.Application.Web.Commands
{
	public class SendMessageCommand
	{
		public string Content { get; set; }

		public Guid ChatId { get; set; }
	}
}
