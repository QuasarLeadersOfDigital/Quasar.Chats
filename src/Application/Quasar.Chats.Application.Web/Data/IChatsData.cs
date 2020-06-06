using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Quasar.Chats.Application.Web.Commands;
using Quasar.Chats.Application.Web.Queries.Chats;

namespace Quasar.Chats.Application.Web.Data
{
	public interface IChatsData
	{
		Task<IEnumerable<Chat>> GetAsync(CancellationToken cancellationToken);

		Task<ChatDetails> GetByIdAsync(Guid chatId, CancellationToken cancellationToken);

		Task AddAsync(CreateDirectChatCommand command, CancellationToken cancellationToken);

		Task UpdateAsync(ChangeChatInfoCommand command, CancellationToken cancellationToken);

		Task RemoveAsync(Guid chatId, CancellationToken cancellationToken);
	}
}
