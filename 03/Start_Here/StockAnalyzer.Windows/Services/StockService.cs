using Newtonsoft.Json;
using StockAnalyzer.Core.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StockAnalyzer.Windows.Services
{
	public class StockService
	{
		public async Task<IEnumerable<StockPrice>> GetStockPricesFor(string ticker, CancellationToken cancellationToken)
		{
			using (var client = new HttpClient())
			{
				var result = await client.GetAsync($"http://localhost:61363/api/stocks/{ticker}",
					cancellationToken);

				result.EnsureSuccessStatusCode();

				var content = await result.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<IEnumerable<StockPrice>>(content);
			}
		}
	}
}
