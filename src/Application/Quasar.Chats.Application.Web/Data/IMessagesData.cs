using System;
using System.Threading;
using System.Threading.Tasks;
using Quasar.Chats.Application.Web.Commands;
using Quasar.Chats.Application.Web.Data.Models;
using Quasar.Chats.Application.Web.Queries;
using Quasar.Chats.Application.Web.Queries.Chats;

namespace Quasar.Chats.Application.Web.Data
{
	public interface IMessagesData
	{
		Task<PagedList<ChatMessage>> GetMessagesAsync(
			Guid chatId, int page, int size, CancellationToken cancellationToken);

		Task AddAsync(SendMessageCommand command, CancellationToken cancellationToken);

		Task UpdateAsync(EditMessageCommand command, CancellationToken cancellationToken);

		Task DeleteAsync(Guid messageId, CancellationToken cancellationToken);

		Task<ChatMessageData> GetByIdAsync(Guid messageId, CancellationToken cancellationToken);
	}
}
