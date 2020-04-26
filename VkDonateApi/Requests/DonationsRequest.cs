using System;
using System.Collections.Generic;
using System.Text;

namespace VkDonateApi
{
	/// <summary>
	/// Запрос для получение списка донатов сообщества.
	/// </summary>
	public class DonationsRequest : IRequest
	{
		/// <summary>
		/// Количество донатов.
		/// </summary>
		public int? Count { get; set; }

		/// <summary>
		/// Смещение для поиска.
		/// </summary>
		public int? Offset { get; set; }

		/// <summary>
		/// Порядок сортировки донатов.
		/// </summary>
		public Sort? Sort { get; set; }

		/// <summary>
		/// Тип сортировки.
		/// </summary>
		public Order Order { get; set; }

		public Dictionary<string, string> GetParameters()
		{
			var parameters = new Dictionary<string, string>() { { "action", "donates" } };

			if (Count != null) parameters.Add("count", Count.ToString());
			if (Offset != null) parameters.Add("offset", Offset.ToString());
			if (Sort != null) parameters.Add("sort", Sort.ToString().ToLower());
			if (Order != null) parameters.Add("order", Order.ToString().ToLower());

			return parameters;
		}
	}
}
