using System;

namespace Quasar.Chats.Application.Web.Commands
{
	public class ChangeChatInfoCommand
	{
		public Guid ChatId { get; set; }

		public string Name { get; set; }
	}
}
