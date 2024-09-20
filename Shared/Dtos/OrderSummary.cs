using System;
namespace Shared.Dtos
{
	public record OrderSummary(
		int Id,
		int ProductId,
		string ProductName,
		decimal ProductPrice,
		int Quantity,
		decimal TotalAmount,
		DateTime Date
		);
	
}

