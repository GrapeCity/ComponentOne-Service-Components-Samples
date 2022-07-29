using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Reflection;
using ProxyDataCollection.Shared;

namespace ProxyDataCollection.Server
{
    public class StockCollection : ObservableCollection<Stock>
    {
        readonly Random _rnd = new Random();
        private const int Speed = 600;
        private readonly int BatchSize = 100;

        public StockCollection() : base(GetFinancialData())
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                SimulateChange();
            }
            GenerateRandomChanges();
        }

        private async void GenerateRandomChanges()
        {
            while (true)
            {
                await Task.Delay(Speed);
                for (int i = 0; i < BatchSize; i++)
                {
                    SimulateChange();
                }
            }
        }

        private void SimulateChange()
        {
            var index = _rnd.Next(0, Count);
            var item = this[index];
            var randCoef = 1 + (_rnd.NextDouble() - 0.5) * 0.01;
            item.Bid *= randCoef;
            item.Ask *= randCoef;
            item.BidHistory = (item.BidHistory ?? Array.Empty<double>()).Concat(new double[] { item.Bid }).TakeLast(5).ToArray();
            this[index] = item;
        }

        public static List<Stock> GetFinancialData()
        {
            var list = new List<Stock>();
            var rnd = new Random(0);
            var asm = Assembly.GetExecutingAssembly();
            foreach (string resName in asm.GetManifestResourceNames())
            {
                if (resName.EndsWith("data.zip"))
                {
                    var zip = new ZipArchive(asm.GetManifestResourceStream(resName));
                    using (var sr = new StreamReader(zip.Entries.First(e => e.Name == "StockSymbols.txt").Open()))
                    {
                        while (!sr.EndOfStream)
                        {
                            var sn = sr.ReadLine().Split('\t');
                            if (sn.Length > 1 && sn[0].Trim().Length > 0)
                            {
                                var stock = new Stock();
                                stock.Symbol = sn[0];
                                stock.Name = sn[1];
                                stock.Bid = rnd.Next(1, 1000);
                                stock.Ask = stock.Bid + rnd.NextDouble();
                                stock.LastSale = stock.Bid;
                                list.Add(stock);
                            }
                        }
                    }
                    return list;
                }
            }
            throw new Exception("Can't find 'data.zip' embedded resource.");
        }

    }
}