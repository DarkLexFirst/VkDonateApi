using Newtonsoft.Json;
using System;

namespace VkDonateApi
{
	public class DonationsResponse : BaseResponse
	{
		/// <summary>
		/// Общее количество донатов в вашем сообществе.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// Донаты.
		/// </summary>
		public Donate[] Donates { get; set; }
	}

	public class Donate
	{
		/// <summary>
		/// Уникальный ID платежа.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// VK ID донатера.
		/// </summary>
		[JsonProperty("uid")]
		public int UserId { get; set; }

		/// <summary>
		/// Дата доната.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Сумма доната.
		/// </summary>
		public int Sum { get; set; }

		/// <summary>
		/// Сообщение от пользователя.
		/// </summary>
		[JsonProperty("msg")]
		public string Message { get; set; }

		/// <summary>
		/// Анонимный донат.
		/// </summary>
		[JsonProperty("anon")]
		public bool Anonymous { get; set; }

		/// <summary>
		/// Видимость доната в группе.
		/// </summary>
		public bool Visible { get; set; }
	}
}
