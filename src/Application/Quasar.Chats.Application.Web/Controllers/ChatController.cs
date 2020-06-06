using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quasar.Chats.Application.Web.Commands;
using Quasar.Chats.Application.Web.Data;
using Quasar.Chats.Application.Web.Queries.Chats;

namespace Quasar.Chats.Application.Web.Controllers
{
	[Route("chats")]
	public class ChatController : ControllerBase
	{
		private readonly IChatsData _chatsData;

		/// <inheritdoc />
		public ChatController(IChatsData chatsData)
		{
			_chatsData = chatsData ?? throw new ArgumentNullException(nameof(chatsData));
		}

		[HttpGet(Name = "GetChats")]
		[ProducesResponseType(statusCode: 200, type: typeof(List<Chat>))]
		public async Task<IActionResult> Get(CancellationToken cancellationToken)
		{
			IEnumerable<Chat> chats = await _chatsData.GetAsync(cancellationToken);
			
			return this.Ok(chats);
		}

		[HttpGet("{chatId}", Name = "GetChat")]
		[ProducesResponseType(statusCode: 200, type: typeof(ChatDetails))]
		public async Task<IActionResult> Get(Guid chatId, CancellationToken cancellationToken)
		{
			ChatDetails chat = await _chatsData.GetByIdAsync(chatId, cancellationToken);
			
			return this.Ok(chat);
		}

		[HttpPost(Name = "PostChat")]
		public async Task<IActionResult> Post([FromBody] CreateDirectChatCommand command, CancellationToken cancellationToken)
		{
			await _chatsData.AddAsync(command, cancellationToken);

			return this.Ok();
		}

		[HttpPut(Name = "PutChat")]
		public async Task<IActionResult> Put([FromBody] ChangeChatInfoCommand command, CancellationToken cancellationToken)
		{
			ChatDetails chat = await _chatsData.GetByIdAsync(command.ChatId, cancellationToken);

			if (chat == null)
			{
				return this.NotFound(new ProblemDetails()
				{
					Detail = $"Chat with id {command.ChatId} not found."
				});
			}
			
			await _chatsData.UpdateAsync(command, cancellationToken);

			return this.Ok();
		}

		[HttpDelete("{chatId}", Name = "DeleteChat")]
		public async Task<IActionResult> Delete([FromRoute] Guid chatId, CancellationToken cancellationToken)
		{
			ChatDetails chat = await _chatsData.GetByIdAsync(chatId, cancellationToken);

			if (chat == null)
			{
				return this.NotFound(new ProblemDetails()
				{
					Detail = $"Chat with id {chatId} not found."
				});
			}
			
			await _chatsData.RemoveAsync(chatId, cancellationToken);

			return this.Ok();
		}
	}
}
