using Efir.DataHub.Models.Models.Moex;
using RuDataAPI;
using RuDataAPI.Extensions;

namespace InvestmentEfficiency
{
    public class EfficiencyBenchmarks
    {
        private static readonly string[] _benchmarks = new string[5]
        {
            "RUPCI",
            "RUPMI",
            "RUPAI",
            "IMOEX",
            "RUCBTR3A3YNS"
        };

        public double RiskFreeRate { get; set; }
        public string? NameIndex1 { get; set; }
        public string? NameIndex2 { get; set; }
        public string? NameIndex3 { get; set; }
        public string? NameIndex4 { get; set; }
        public string? NameIndex5 { get; set; }
        public double TwrIndex1 { get; set; }
        public double TwrIndex2 { get; set; }
        public double TwrIndex3 { get; set; }
        public double TwrIndex4 { get; set; }
        public double TwrIndex5 { get; set; }
        public double StdIndex1 { get; set; }
        public double StdIndex2 { get; set; }
        public double StdIndex3 { get; set; }
        public double StdIndex4 { get; set; }
        public double StdIndex5 { get; set; }

        public static void UseBenchmarks(params string[] benchmarks)
        {
            if (benchmarks.Length > 5)
                throw new IndexOutOfRangeException("Too many benchmarks. Should be less or equal than 5.");

            for (int i = 0; i < benchmarks.Length; i++)
                _benchmarks[i] = benchmarks[i];
        }
        
        public static async Task<EfficiencyBenchmarks> CalculateBenchmarks(DateTime start, DateTime end, EfirClient efir)
        {
            var bench = new EfficiencyBenchmarks()
            {
                NameIndex1 = _benchmarks[0],
                NameIndex2 = _benchmarks[1],
                NameIndex3 = _benchmarks[2],
                NameIndex4 = _benchmarks[3],
                NameIndex5 = _benchmarks[4]
            };

            if (efir.IsLoggedIn is false)
                await efir.LoginAsync();

            var curveDate = new DateTime(start.Year, 12, 31);
            var tenor = (end - start).Days / 365.0;
            bench.RiskFreeRate = await efir.CalculateGcurveForDate(curveDate, tenor);

            var his = await efir.GetMoexIndexHistoryAsync(start, end, bench.NameIndex1, bench.NameIndex2, bench.NameIndex3, bench.NameIndex4, bench.NameIndex5);

            var index1his = his.Where(hf => hf.secid == bench.NameIndex1).AsQueryable();
            var index2his = his.Where(hf => hf.secid == bench.NameIndex2).AsQueryable();
            var index3his = his.Where(hf => hf.secid == bench.NameIndex3).AsQueryable();
            var index4his = his.Where(hf => hf.secid == bench.NameIndex4).AsQueryable();
            var index5his = his.Where(hf => hf.secid == bench.NameIndex5).AsQueryable();


            var index1EffSeries = index1his.Select(ser => new EfficiencyRecord
            {
                Date = ser.tradedate!.Value,
                Portfolio = (double?)ser.close,
                Flow = 0,
                Commision = 0,
            }).AsEfficiencyQuery();

            var index2EffSeries = index2his.Select(ser => new EfficiencyRecord
            {
                Date = ser.tradedate!.Value,
                Portfolio = (double?)ser.close,
                Flow = 0,
                Commision = 0,
            }).AsEfficiencyQuery();

            var index3EffSeries = index3his.Select(ser => new EfficiencyRecord
            {
                Date = ser.tradedate!.Value,
                Portfolio = (double?)ser.close,
                Flow = 0,
                Commision = 0,
            }).AsEfficiencyQuery();

            var index4EffSeries = index4his.Select(ser => new EfficiencyRecord
            {
                Date = ser.tradedate!.Value,
                Portfolio = (double?)ser.close,
                Flow = 0,
            }).AsEfficiencyQuery();

            var index5EffSeries = index5his.Select(ser => new EfficiencyRecord
            {
                Date = ser.tradedate!.Value,
                Portfolio = (double?)ser.close,
                Flow = 0,
                Commision = 0,
            }).AsEfficiencyQuery();

            var eff1 = Efficiency.ConfigureEfficiencyCalculation(index1EffSeries)
                .AddLifeTimeCalculation()
                .AddTwrCalculation()
                .AddStdCalculation()
                .Calculate();

            var eff2 = Efficiency.ConfigureEfficiencyCalculation(index2EffSeries)
                .AddLifeTimeCalculation()
                .AddTwrCalculation()
                .AddStdCalculation()
                .Calculate();

            var eff3 = Efficiency.ConfigureEfficiencyCalculation(index3EffSeries)
                .AddLifeTimeCalculation()
                .AddTwrCalculation()
                .AddStdCalculation()
                .Calculate();

            var eff4 = Efficiency.ConfigureEfficiencyCalculation(index4EffSeries)
                .AddLifeTimeCalculation()
                .AddTwrCalculation()
                .AddStdCalculation()
                .Calculate();

            var eff5 = Efficiency.ConfigureEfficiencyCalculation(index5EffSeries)
                .AddLifeTimeCalculation()
                .AddTwrCalculation()
                .AddStdCalculation()
                .Calculate();

            bench.TwrIndex1 = eff1.Twr ?? default;
            bench.TwrIndex2 = eff2.Twr ?? default;
            bench.TwrIndex3 = eff3.Twr ?? default;
            bench.TwrIndex4 = eff4.Twr ?? default;
            bench.TwrIndex5 = eff5.Twr ?? default;

            bench.StdIndex1 = eff1.Std ?? default;
            bench.StdIndex2 = eff2.Std ?? default;
            bench.StdIndex3 = eff3.Std ?? default;
            bench.StdIndex4 = eff4.Std ?? default;
            bench.StdIndex5 = eff5.Std ?? default;
            return bench;
        }
    }
}
