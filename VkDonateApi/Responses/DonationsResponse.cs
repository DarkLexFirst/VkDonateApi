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
}
