using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quasar.Chats.Application.Web.Commands;
using Quasar.Chats.Application.Web.Data.Models;
using Quasar.Chats.Application.Web.Queries.Chats;

namespace Quasar.Chats.Application.Web.Data
{
	public class ChatsData : IChatsData
	{
		private readonly IList<ChatData> _chatData;

		public ChatsData()
		{
			_chatData = new List<ChatData>();
		}

		/// <inheritdoc />
		public async Task<IEnumerable<Chat>> GetAsync(CancellationToken cancellationToken)
		{
			var chats = _chatData.Select(Mapper.ToChat);

			return chats;
		}

		/// <inheritdoc />
		public async Task<ChatDetails> GetByIdAsync(Guid chatId, CancellationToken cancellationToken)
		{
			var chat = _chatData
				.FirstOrDefault(ch => ch.Id == chatId);

			return chat.ToChatDetails();
		}

		/// <inheritdoc />
		public async Task AddAsync(CreateDirectChatCommand command, CancellationToken cancellationToken)
		{
			var chat = new ChatData
			{
				Id = Guid.NewGuid(),
				Member = new MemberData
				{
					Id = command.To
				},
				Name = command.To.ToString()
			};

			_chatData.Add(chat);
		}

		/// <inheritdoc />
		public async Task UpdateAsync(ChangeChatInfoCommand command, CancellationToken cancellationToken)
		{
			ChatData chat = _chatData.FirstOrDefault(ch => ch.Id == command.ChatId);

			if (chat == null)
			{
				return;
			}

			chat.Name = command.Name;
		}

		/// <inheritdoc />
		public async Task RemoveAsync(Guid chatId, CancellationToken cancellationToken)
		{
			var chat = _chatData.FirstOrDefault(ch => ch.Id == chatId);

			_chatData.Remove(chat);
		}
	}
}
