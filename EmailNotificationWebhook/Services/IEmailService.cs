using System;
using Shared.Dtos;

namespace EmailNotificationWebhook.Services
{
	public interface IEmailService
	{
		string SendEmail(EmailDto email);
	}
}

