using System;

namespace Quasar.Chats.Application.Web.Queries.Chats
{
	public class Chat
	{
		public Guid Id { get; set; }

		public ChatMember Member { get; set; }
	}
}
