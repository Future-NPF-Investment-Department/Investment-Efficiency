# Investment Efficiency

Небольшая библиотека для расчета оценки эффективности инвестирования средств Фонда.

## Зависимости

- InvestmentDataContext ([подробнее](https://github.com/Future-NPF-Investment-Department/Investment-Data-Context))
- RuDataApi ([подробнее](https://github.com/Future-NPF-Investment-Department/RuData-API))

## Использование

```cs
using InvestmentEfficiency;
using InvestmentDataContext;
using InvestmentDataContext.Classifications;

var connstring = "MyConnectionString";
using var data = new InvestmentData(connstring);


var query = EfficiencyQuery.ConfigureNew(data)
    .WithFairPricesUsed()
    .WithDates(new(2022, 12, 31), new(2023, 3, 31))
    .WithFundName("FundName")
    .WithStrategy("StrategyName")
    .WithAssetClass(AssetClass.Bonds)
    .WithRiskType(RiskType.NonRisk)
    .GenerateQuery()
    ;

var eff = Efficiency.ConfigureEfficiencyCalculation(query)
    .AddLifeTimeCalculation()
    .AddIncomeCalculation()
    .AddAveragePortfolioCalculation()
    .AddMwrCalculation()
    .AddTwrCalculation()
    .AddStdCalculation()
    .Calculate()
    ;

Console.WriteLine(eff.Income);
```