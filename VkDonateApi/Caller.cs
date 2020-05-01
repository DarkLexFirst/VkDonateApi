using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Flurl.Http;

namespace VkDonateApi
{
	public class Caller
	{
		/// <summary>
		/// Секретный ключ.
		/// </summary>
		public string Key { get; internal set; }

		private string Server { get; } = "https://api.vkdonate.ru";

		/// <summary>
		/// Интервал обращения к серверу.
		/// </summary>
		public int MsCallInterval { get; } = 1500;

		/// <summary>
		/// Возвращает или устанавливает состояние обработчика.
		/// </summary>
		public bool Enabled => _callTimer.Enabled;

		internal DonationsRequest Request { get; set; }

		/// <summary>
		/// Событие, вызываемое при появлении нового доната.
		/// </summary>
		public Action<Donate> NewDonate { get; set; }

		/// <summary>
		/// Задаёт максимальное количество обрабатываемых донатов в секунду.
		/// </summary>
		public int DonatesPerCall { get => Request.Count.Value; set => Request.Count = value; }

		public Caller()
		{
			_callTimer = CreateCallTimer();

			Request = new DonationsRequest()
			{
				Order = Order.Ascending,
				Offset = 0,
				Count = 20,
			};
		}

		private Timer _callTimer { get; set; }
		private Timer CreateCallTimer()
		{
			Timer timer = new Timer(50);
			timer.AutoReset = false;
			timer.Elapsed += async(s, e) =>
			{
				await OnCall();
				((Timer)s).Start();
			};
			return timer;
		}

		/// <summary>
		/// Запускает обработчик.
		/// </summary>
		public void StartCalling()
		{
			if (Enabled) return;

			_callTimer.Start();
		}

		/// <summary>
		/// Останавливает обработчик.
		/// </summary>
		public void StopCallong()
		{
			_callTimer.Stop();
		}

		private async Task OnCall()
		{
			try
			{
				DonationsResponse response = await Call<DonationsResponse>(null);

				if (response.Donates.Length == 0) return;
				if (NewDonate == null) return;

				if (Request.Offset == response.Count) return;

				Request.Offset += response.Donates.Length;
				if (Request.Offset >= response.Count) Request.Offset = response.Count;

				foreach(var donate in response.Donates)
					NewDonate?.Invoke(donate);
			}
			catch (Exception e)
			{
				if (e.Message.StartsWith("Ошибка #3:")) return;
				Console.WriteLine(e);
			}
		}

		private List<Task> _calls = new List<Task>();
		private async Task AwaitCalls()
		{
			while (_calls.Count > 0)
			{
				await Task.WhenAll(_calls.ToArray());
				_calls.RemoveAll(c => c.IsCompleted);
			}
		}

		private async Task<T> AddToAwait<T>(Task<T> call)
		{
			_calls.Add(call);

			T result = await call; 
			_calls.Add(Task.Delay(MsCallInterval));
			return result;
		}

		private async Task<string> Call(IRequest request)
		{
			if (string.IsNullOrWhiteSpace(Key)) return null;

			await AwaitCalls();

			if (request == null) request = Request;

			var parameters = request.GetParameters();
			parameters.Add("key", Key);

			var uriRequest = new FormUrlEncodedContent(parameters);
			var response = await AddToAwait(Server.AllowAnyHttpStatus().PostAsync(uriRequest));

			if (!response.IsSuccessStatusCode) return null;

			return await response.Content.ReadAsStringAsync();
		}

		internal async Task<Response> Call<Response>(IRequest request) where Response : BaseResponse
		{
			var content = await Call(request);
			var response = BaseResponse.FromJson<Response>(content);

			if (!response.Success) throw new RequestException(response.Text);

			return response;
		}
	}
}
