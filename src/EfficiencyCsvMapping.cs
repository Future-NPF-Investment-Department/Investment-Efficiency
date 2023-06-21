using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.TypeConversion;
using CsvHelper.Configuration;

namespace InvestmentEfficiency
{
    public class EfficiencyCsvMapping : ClassMap<Efficiency>
    {
        public EfficiencyCsvMapping(CsvConfiguration config)
        {           
            Map(eff => eff.Details!.StartDate).Index(0).TypeConverterOption.Format("dd.MM.yyyy");
            Map(eff => eff.Details!.EndDate).Index(1).TypeConverterOption.Format("dd.MM.yyyy");
            Map(eff => eff.Details!.AmName).Index(2);
            Map(eff => eff.Details!.FundName).Index(3);
            Map(eff => eff.Details!.EntityType).Index(4);
            Map(eff => eff.Details!.StrategyName).Index(5);
            Map(eff => eff.Details!.Contract).Index(6);
            Map(eff => eff.Details!.AssetClass).Index(7);
            Map(eff => eff.Details!.IsinList).Index(8).TypeConverter<ArrayConverter>();
            Map(eff => eff.Details!.RiskType).Index(9);
            Map(eff => eff.SharpeRatio).Index(10);
            Map(eff => eff.InformationRatio).Index(11);
            Map(eff => eff.Twr).Index(12);
            Map(eff => eff.Mwr).Index(13);
            Map(eff => eff.Std).Index(14);
            Map(eff => eff.AverageProtfolio).Index(15);
            Map(eff => eff.Income).Index(16);
            Map(eff => eff.LifeTime).Index(17);
            Map(eff => eff.Benchmarks!.RiskFreeRate).Index(18);
            Map(eff => eff.Benchmarks!.NameIndex1).Index(19);
            Map(eff => eff.Benchmarks!.NameIndex2).Index(20);
            Map(eff => eff.Benchmarks!.NameIndex3).Index(21);
            Map(eff => eff.Benchmarks!.NameIndex4).Index(22);
            Map(eff => eff.Benchmarks!.NameIndex5).Index(23);
            Map(eff => eff.Benchmarks!.TwrIndex1).Index(24);
            Map(eff => eff.Benchmarks!.TwrIndex2).Index(25);
            Map(eff => eff.Benchmarks!.TwrIndex3).Index(26);
            Map(eff => eff.Benchmarks!.TwrIndex4).Index(27);
            Map(eff => eff.Benchmarks!.TwrIndex5).Index(28);
            Map(eff => eff.Benchmarks!.StdIndex1).Index(29);
            Map(eff => eff.Benchmarks!.StdIndex2).Index(30);
            Map(eff => eff.Benchmarks!.StdIndex3).Index(31);
            Map(eff => eff.Benchmarks!.StdIndex4).Index(32);
            Map(eff => eff.Benchmarks!.StdIndex5).Index(33);
            Map(eff => eff.EfficiencySeries).Ignore();
        }
    }
}
