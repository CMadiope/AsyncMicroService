using System;
using MassTransit;
using Shared.Dtos;

namespace EmailNotificationWebhook.Consumer
{
	public class WebhookConsumer : IConsumer<EmailDto>
	{
        private readonly HttpClient _httpClient;

        public WebhookConsumer(HttpClient httpClient)
		{
            _httpClient = httpClient;
        }

        public async Task Consume(ConsumeContext<EmailDto> context)
        {
            var result = await _httpClient.PostAsJsonAsync("https://localhost:7261/email-webhook", new EmailDto(context.Message.Title, context.Message.Content));
            result.EnsureSuccessStatusCode();
        }
    }
}

