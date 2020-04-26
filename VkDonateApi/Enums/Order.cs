namespace VkDonateApi
{
	public sealed class Order : IEnum<Order>
	{
		/// <summary>
		/// По возрастанию.
		/// </summary>
		/// 
		public static readonly Order Ascending = CreateObject("asc");

		/// <summary>
		/// По убыванию.
		/// </summary>
		public static readonly Order Descending = CreateObject("desc");
	}
}
