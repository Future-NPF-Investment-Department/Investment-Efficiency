using InvestmentDataContext.Classifications;

namespace InvestmentEfficiency
{
    /// <summary>
    ///     Represents efficiency query configuration details.
    /// </summary>
    public record EfficiencyQueryDetails
    {
        /// <summary>
        ///     Efficiency calculation start date.
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        ///     Efficiency calculation end date.
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        ///     Asset management company name configured for efficiency calculation.
        /// </summary>
        public string? AmName { get; set; }
        /// <summary>
        ///     Fund name configured for efficiency calculation.
        /// </summary>
        public string? FundName { get; set; }
        /// <summary>
        ///     Pension property type configured for efficiency calculation.
        /// </summary>
        public PensionPropertyType? EntityType { get; set; }
        /// <summary>
        ///     Strategy name configured for efficiency calculation.
        /// </summary>
        public string? StrategyName { get; set; }
        /// <summary>
        ///     Contract ID configured for efficiency calculation.
        /// </summary>
        public string? Contract { get; set; }
        /// <summary>
        ///     Asset class configured for efficiency calculation.
        /// </summary>
        public AssetClass? AssetClass { get; set; }
        /// <summary>
        ///     Collection of ISINs for which efficiency is calculated.
        /// </summary>
        public string[]? IsinList { get; set; }
        /// <summary>
        ///     Risk type configured for efficiency calculation.
        /// </summary>
        public RiskType? RiskType { get; set; }
    }
}
