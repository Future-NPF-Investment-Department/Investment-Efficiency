#pragma warning disable IDE0090


namespace InvestmentEfficiency
{
    /// <summary>
    ///     Represents investment efficiency metrics.
    /// </summary>
    public class Efficiency
    {
        /// <summary>
        ///     Time-weighted rate of return.
        /// </summary>
        public double? Twr { get; set; }

        /// <summary>
        ///     Money-weighted rate of return.
        /// </summary>
        public double? Mwr { get; set; }

        /// <summary>
        ///     Rate of return standard deviation.
        /// </summary>
        public double? Std { get; set; }

        /// <summary>
        ///     Income earned over the period.
        /// </summary>
        public double? Income { get; set; }

        /// <summary>
        ///     Average portfolio (net asset value) over the period.
        /// </summary>
        public double? AverageProtfolio { get; set; }

        /// <summary>
        ///     Period length.
        /// </summary>
        public int? LifeTime { get; set; }

        /// <summary>
        ///     Sharpe coefficient.
        /// </summary>
        public double? SharpeRatio { get; set; }

        /// <summary>
        ///     Information ratio.
        /// </summary>
        public double? InformationRatio { get; set; }

        /// <summary>
        ///     Efficiency benchmarks.
        /// </summary>
        public EfficiencyBenchmarks? Benchmarks { get; set; }

        /// <summary>
        ///     Efficiency subject reference data.
        /// </summary>
        public EfficiencyQueryDetails? Details { get; set; }

        /// <summary>
        ///     Efficiency time series.
        /// </summary>
        public List<EfficiencyRecord>? EfficiencySeries { get; set; } = null!;

        /// <summary>
        ///     Initializes efficiency calculation configuration.
        /// </summary>
        public static EfficiencyConfigurer ConfigureEfficiencyCalculation(EfficiencyQuery query)
            => new EfficiencyConfigurer(query);

        /// <summary>
        ///     Initializes efficiency calculation configuration with benchmarks.
        /// </summary>
        public static EfficiencyConfigurer ConfigureEfficiencyCalculation(EfficiencyQuery query, EfficiencyBenchmarks benchmarks)
            => new EfficiencyConfigurer(query, benchmarks);

        public override string ToString()
        {
            string details = $"AM: {Details?.AmName}\nFUND: {Details?.FundName}\nPN/PR: {Details?.EntityType}\nSTR: {Details?.StrategyName}\nDDU: {Details?.Contract}\nASSET CL.: {Details?.AssetClass}\nRISK: {Details?.RiskType}\n";
            string isins = "ISINs: ";
            if (Details?.IsinList is not null)            
                foreach (var isin in Details.IsinList) isins += $"{isin} ";            
            isins += "\n\n";

            string calcres = $"TWR: {Twr:0.00%}\nMWR: {Mwr:0.00%}\nSTD: {Std:0.00%}\nINC: {Income:n}\nAVG: {AverageProtfolio:n}\nLFT: {LifeTime}\n\n";
            string series = string.Empty;
            if (EfficiencySeries is not null)
            {
                foreach (var row in EfficiencySeries)
                    series += $"{row.Date:dd.MM.yyyy} | {row.Portfolio:n} | {row.Flow:n} | {row.Commision:n} | {row.Growth:0.0%}\n";
            }
            return details + isins + calcres + series;
        }

    }
}
