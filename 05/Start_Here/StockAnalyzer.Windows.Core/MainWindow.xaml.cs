using StockAnalyzer.Core.Domain;
using StockAnalyzer.Windows.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;

namespace StockAnalyzer.Windows.Core
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		CancellationTokenSource cancellationTokenSource = null;

		private async void Search_Click(object sender, RoutedEventArgs e)
		{
			#region Before loading stock data
			var watch = new Stopwatch();
			watch.Start();
			StockProgress.Visibility = Visibility.Visible;

			Search.Content = "Cancel";
			#endregion

			#region Cancellation
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
				cancellationTokenSource = null;
				return;
			}

			cancellationTokenSource = new CancellationTokenSource();
			cancellationTokenSource.Token.Register(() =>
			{
				Notes.Text += "Cancellation requested" + Environment.NewLine;
			});
			#endregion

			try
			{
				var tickers = Ticker.Text.Split(' ');

				var prices = new ObservableCollection<StockPrice>();

				Stocks.ItemsSource = prices;

				var service = new StockDiskStreamService();

				await foreach (var price in service.GetAllStockPrices(cancellationTokenSource.Token))
				{
					if (tickers.Contains(price.Ticker))
					{
						prices.Add(price);
					}
				}
			}
			catch (Exception ex)
			{
				Notes.Text += ex.Message + Environment.NewLine;
			}
			finally
			{
				cancellationTokenSource = null;
			}

			#region After stock data is loaded
			StocksStatus.Text = $"Loaded stocks for {Ticker.Text} in {watch.ElapsedMilliseconds}ms";
			StockProgress.Visibility = Visibility.Hidden;
			Search.Content = "Search";
			#endregion
		}

		private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));

			e.Handled = true;
		}

		private void Close_OnClick(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}