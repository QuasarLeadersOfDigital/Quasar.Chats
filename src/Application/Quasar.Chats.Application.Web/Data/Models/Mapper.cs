using System;
using Quasar.Chats.Application.Web.Queries.Chats;

namespace Quasar.Chats.Application.Web.Data.Models
{
	public static class Mapper
	{
		public static Chat ToChat(this ChatData chatData)
		{
			if (chatData == null)
			{
				return null;
			}
			
			var target = new Chat
			{
				Id = chatData.Id,
				Member = new ChatMember
				{
					Id = chatData.Member.Id
				}
			};

			return target;
		}

		public static ChatDetails ToChatDetails(this ChatData chatData)
		{
			if (chatData == null)
			{
				return null;
			}

			var target = new ChatDetails
			{
				Id = chatData.Id,
				Member = new ChatDetailsMember
				{
					Id = chatData.Member.Id
				},
				Name = chatData.Name
			};

			return target;
		}

		public static ChatMessage ToChatMessage(ChatMessageData source)
		{
			if (source == null)
			{
				return null;
			}
			
			return new ChatMessage
			{
				Content = source.Content,
				Id = source.Id,
				State = source.State,
				SentDate = DateTimeOffset.Now
			};
		}
	}
}
