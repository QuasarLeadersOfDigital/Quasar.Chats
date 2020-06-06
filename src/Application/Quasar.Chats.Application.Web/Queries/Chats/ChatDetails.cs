using System;

namespace Quasar.Chats.Application.Web.Queries.Chats
{
	public class ChatDetails
	{
		public Guid Id { get; set; }
		
		public string Name { get; set; }

		public ChatDetailsMember Member { get; set; }
	}
}
