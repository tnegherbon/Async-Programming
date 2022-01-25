using System;

namespace StockAnalyzer.Core.Domain
{
	public class StockPrice
	{
		public string Ticker { get; set; }
		public DateTime TradeDate { get; set; }
		public decimal? Open { get; set; }
		public decimal? High { get; set; }
		public decimal? Low { get; set; }
		public decimal? Close { get; set; }
		public int Volume { get; set; }

		private decimal _change;
		public decimal Change
		{
			get
			{
				return Math.Round(this._change, 2);
			}
			set
			{
				this._change = value;
			}
		}

		private decimal _changePercent;
		public decimal ChangePercent
		{
			get
			{
				return Math.Round(this._changePercent, 2);
			}
			set
			{
				this._changePercent = value;
			}
		}
	}
}