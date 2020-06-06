using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quasar.Chats.Application.Web.Commands;
using Quasar.Chats.Application.Web.Data.Models;
using Quasar.Chats.Application.Web.Queries;
using Quasar.Chats.Application.Web.Queries.Chats;

namespace Quasar.Chats.Application.Web.Data
{
	public class MessagesData : IMessagesData
	{
		private readonly IList<ChatMessageData> _chatMessages;

		public MessagesData()
		{
			_chatMessages = new List<ChatMessageData>();
		}

		/// <inheritdoc />
		public async Task<PagedList<ChatMessage>> GetMessagesAsync(Guid chatId, int page, int size, CancellationToken cancellationToken)
		{
			var messages = _chatMessages
				.Where(cm => cm.ChatId == chatId)
				.OrderByDescending(m => m.SendDate)
				.Skip((page - 1) * size)
				.Take(size)
				.ToList();

			List<ChatMessage> items = messages.Select(Mapper.ToChatMessage).ToList();
			
			return new PagedList<ChatMessage>(page, size, _chatMessages.Count, items);
		}

		/// <inheritdoc />
		public async Task AddAsync(SendMessageCommand command, CancellationToken cancellationToken)
		{
			var message = new ChatMessageData
			{
				Id = Guid.NewGuid(),
				Content = command.Content,
				State = "Sending",
				ChatId = command.ChatId,
				SendDate = DateTimeOffset.Now
			};
			
			_chatMessages.Add(message);
		}

		/// <inheritdoc />
		public async Task UpdateAsync(EditMessageCommand command, CancellationToken cancellationToken)
		{
			ChatMessageData message = _chatMessages.FirstOrDefault(m => m.Id == command.MessageId);

			if (message == null)
			{
				return;
			}

			message.Content = command.Content;
		}

		/// <inheritdoc />
		public async Task DeleteAsync(Guid messageId, CancellationToken cancellationToken)
		{
			ChatMessageData message = _chatMessages.FirstOrDefault(m => m.Id == messageId);

			if (message == null)
			{
				return;
			}

			_chatMessages.Remove(message);
		}

		/// <inheritdoc />
		public async Task<ChatMessageData> GetByIdAsync(Guid messageId, CancellationToken cancellationToken)
		{
			return _chatMessages.FirstOrDefault(m => m.Id == messageId);
		}
	}
}
