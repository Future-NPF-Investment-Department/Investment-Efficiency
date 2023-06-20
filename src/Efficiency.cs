#pragma warning disable IDE0090
using RuDataAPI;

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
        ///     Initializes efficiency calculation configuration.
        /// </summary>
        public static EfficiencyConfigurer ConfigureEfficiencyCalculation(EfficiencyQuery query)
            => new EfficiencyConfigurer(query);        
        /// <summary>
        ///     Efficiency time series.
        /// </summary>
        public List<EfficiencyRecord>? EfficiencySeries { get; set; } = null!;
    }
}
