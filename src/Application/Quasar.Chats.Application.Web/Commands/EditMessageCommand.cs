using System;

namespace Quasar.Chats.Application.Web.Commands
{
	public class EditMessageCommand
	{
		public Guid MessageId { get; set; }

		public string Content { get; set; }
	}
}
