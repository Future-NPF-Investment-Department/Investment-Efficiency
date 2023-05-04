namespace InvestmentEfficiency
{
    /// <summary>
    ///     Represents record in efficiency time series.
    /// </summary>
    public class EfficiencyRecord
    {
        /// <summary>
        ///     Date of series.
        /// </summary>
        public DateTime Date { get; init; }
        /// <summary>
        ///     Net asset value in date.
        /// </summary>
        public double? Portfolio { get; init; } = default(double);
        /// <summary>
        ///     Flow value in date.
        /// </summary>
        public double? Flow { get; init; } = default(double);
        /// <summary>
        ///     Commision value in date.
        /// </summary>
        public double? Commision { get; set; } = default(double);
        /// <summary>
        ///     Growth rate for date.
        /// </summary>
        public double Growth { get; set; } = 1.0;
    }
}
