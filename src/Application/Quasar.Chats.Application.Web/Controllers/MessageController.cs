using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quasar.Chats.Application.Web.Commands;
using Quasar.Chats.Application.Web.Data;
using Quasar.Chats.Application.Web.Data.Models;
using Quasar.Chats.Application.Web.Queries;
using Quasar.Chats.Application.Web.Queries.Chats;

namespace Quasar.Chats.Application.Web.Controllers
{
	[Route("messages")]
	public class MessageController : ControllerBase
	{
		private readonly IChatsData _chatsData;
		private readonly IMessagesData _messagesData;

		/// <inheritdoc />
		public MessageController(IChatsData chatsData, IMessagesData messagesData)
		{
			_chatsData = chatsData ?? throw new ArgumentNullException(nameof(chatsData));
			_messagesData = messagesData ?? throw new ArgumentNullException(nameof(messagesData));
		}

		[HttpGet("/chats/{chatId}/messages", Name = "GetChatMessages")]
		[ProducesResponseType(statusCode: 200, type: typeof(PagedList<ChatMessage>))]
		public async Task<IActionResult> Get(
			[FromRoute] Guid chatId,
			[FromQuery] [DefaultValue("1")] int? page, [DefaultValue("25")] int? size,
			CancellationToken cancellationToken)
		{
			ChatDetails chat = await _chatsData.GetByIdAsync(chatId, cancellationToken);

			if (chat == null)
			{
				return this.BadRequest(new ProblemDetails()
				{
					Detail = $"Chat with id {chatId} not found."
				});
			}

			int pageValue = page ?? 1;
			int sizeValue = size ?? 25;
			
			PagedList<ChatMessage> messages = await _messagesData
				.GetMessagesAsync(
					chatId,
					pageValue,
					sizeValue,
					cancellationToken);

			return this.Ok(messages);
		}

		[HttpPost(Name = "PostMessage")]
		public async Task<IActionResult> Post([FromBody] SendMessageCommand command, CancellationToken cancellationToken)
		{
			ChatDetails chat = await _chatsData.GetByIdAsync(command.ChatId, cancellationToken);

			if (chat == null)
			{
				return this.NotFound(new ProblemDetails()
				{
					Detail = $"Chat with id {command.ChatId} not found."
				});
			}
			
			await _messagesData.AddAsync(command, cancellationToken);

			return this.Ok();
		}

		[HttpPut(Name = "PutMessage")]
		public async Task<IActionResult> Put([FromBody] EditMessageCommand command, CancellationToken cancellationToken)
		{
			ChatMessageData message = await _messagesData.GetByIdAsync(command.MessageId, cancellationToken);

			if (message == null)
			{
				return this.NotFound(new ProblemDetails()
				{
					Detail = $"Message with id {command.MessageId} not found."
				});
			}
			
			await _messagesData.UpdateAsync(command, cancellationToken);

			return this.Ok();
		}

		[HttpDelete("{messageId}", Name = "DeleteMessage")]
		public async Task<IActionResult> Delete([FromRoute] Guid messageId, CancellationToken cancellationToken)
		{
			ChatMessageData message = await _messagesData.GetByIdAsync(messageId, cancellationToken);

			if (message == null)
			{
				return this.NotFound(new ProblemDetails()
				{
					Detail = $"Message with id {messageId} not found."
				});
			}
			
			await _messagesData.DeleteAsync(messageId, cancellationToken);

			return this.Ok();
		}
	}
}
