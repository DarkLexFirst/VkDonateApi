using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VkDonateApi
{
	/// <summary>
	/// VkDonate API.
	/// </summary>
	public class VkDonate
	{
		/// <summary>
		/// Обработчик вызовов к серверу.
		/// </summary>
		public Caller Caller { get; }

		public VkDonate(string key)
		{
			Caller = new Caller();
			Caller.Key = key;

			Caller.Request.Offset = GetOffset();
		}

		/// <summary>
		/// Запускает регулярные запросы к серверу.
		/// </summary>
		/// <param name="newDonateAction">Событие, вызываемое при появлении нового доната.</param>
		public void Start(Action<Donate> newDonateAction)
		{
			Caller.NewDonate = newDonateAction;
			Caller.StartCalling();
		}

		/// <summary>
		/// Получает реальное смещение для поиска.
		/// </summary>
		protected virtual int GetOffset()
		{
			return GetDonations(new DonationsRequest() { Count = 1 }).Count;
		}

		/// <summary>
		/// Делает запрос донатов.
		/// </summary>
		public DonationsResponse GetDonations(DonationsRequest request)
		{
			return Caller.Call<DonationsResponse>(request).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Делает запрос донатов.
		/// </summary>
		public async Task<DonationsResponse> GetDonationsAsync(DonationsRequest request)
		{
			return await Caller.Call<DonationsResponse>(request);
		}



		/// <summary>
		/// Делает запрос всех донатов сообщества. (запрос может быть очень долгим)
		/// </summary>
		public Donate[] GetAllDonations()
		{
			return GetAllDonationsAsync().GetAwaiter().GetResult();
		}

		/// <summary>
		/// Делает запрос всех донатов сообщества. (запрос может быть очень долгим)
		/// </summary>
		public async Task<Donate[]> GetAllDonationsAsync()
		{
			DonationsRequest request = new DonationsRequest()
			{
				Order = Order.Ascending,
				Offset = 0,
				Count = 50
			};

			List<Donate> donates = new List<Donate>();

			int count = 0;
			do
			{
				DonationsResponse response = await Caller.Call<DonationsResponse>(request);
				donates.AddRange(response.Donates);
				count = response.Count;
				request.Offset += response.Donates.Length;
			} 
			while (request.Offset < count);

			return donates.ToArray();
		}
	}
}
