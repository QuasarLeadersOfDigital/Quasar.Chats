using System;

namespace Quasar.Chats.Application.Web.Data.Models
{
	public class ChatData
	{
		public Guid Id { get; set; }

		public MemberData Member { get; set; }

		public string Name { get; set; }
	}
}
