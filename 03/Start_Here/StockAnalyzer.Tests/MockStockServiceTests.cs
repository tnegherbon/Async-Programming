using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockAnalyzer.Windows.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StockAnalyzer.Tests
{
	[TestClass]
	public class MockStockServiceTests
	{
		[TestMethod]
		public async Task Can_Load_All_GOOGL_Stocks()
		{
			var service = new MockStockService();
			var stocks = await service.GetStockPricesFor("GOOGL", CancellationToken.None);

			Assert.AreEqual(stocks.Count(), 1);
		}
	}
}