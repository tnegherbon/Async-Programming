using Newtonsoft.Json;
using StockAnalyzer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StockAnalyzer.Windows.Services
{
	public interface IStockService
	{
		Task<IEnumerable<StockPrice>> GetStockPricesFor(string ticker, CancellationToken cancellationToken);
	}

	public class StockService : IStockService
	{
		private int i = 0;

		public async Task<IEnumerable<StockPrice>> GetStockPricesFor(string ticker, CancellationToken cancellationToken)
		{
			using (var client = new HttpClient())
			{
#if DEBUG
				await Task.Delay(i++ * 1000);
#endif

				var result = await client.GetAsync($"http://localhost:61363/api/stocks/{ticker}",
					cancellationToken);

				result.EnsureSuccessStatusCode();

				var content = await result.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<IEnumerable<StockPrice>>(content);
			}
		}
	}

	public class MockStockService : IStockService
	{
		public Task<IEnumerable<StockPrice>> GetStockPricesFor(string ticker, CancellationToken cancellationToken)
		{
			var r = new Random();

			var stocks = new List<StockPrice>
			{
				new StockPrice { Ticker = "O", Change = (decimal)r.NextDouble(), ChangePercent = (decimal)r.NextDouble() },
				new StockPrice { Ticker = "GOOGL", Change = (decimal)r.NextDouble(), ChangePercent = (decimal)r.NextDouble() },
				new StockPrice { Ticker = "MSFT", Change = (decimal)r.NextDouble(), ChangePercent = (decimal)r.NextDouble() },
				new StockPrice { Ticker = "FAST", Change = (decimal)r.NextDouble(), ChangePercent = (decimal)r.NextDouble() },
				new StockPrice { Ticker = "UDR", Change = (decimal)r.NextDouble(), ChangePercent = (decimal)r.NextDouble() },
				new StockPrice { Ticker = "AAPL", Change = (decimal)r.NextDouble(), ChangePercent = (decimal)r.NextDouble() },
			};

			return Task.FromResult(stocks.Where(stock => stock.Ticker == ticker));
		}
	}

}
