using System;

namespace Quasar.Chats.Application.Web.Commands
{
	public class DeleteMessageCommand
	{
		public Guid AccountId { get; set; }

		public Guid MessageId { get; set; }
	}
}