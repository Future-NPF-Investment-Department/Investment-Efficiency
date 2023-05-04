namespace InvestmentEfficiency
{
    /// <summary>
    ///     Efficiency calculation queue flags.
    /// </summary>
    [Flags]
    internal enum CalculationStage : ushort
    {
        Initialized = 0,
        /// <summary>
        ///     Growth rates calculation queued.
        /// </summary>
        GrrCalcQueued = 1,
        /// <summary>
        ///     Life-time calculation queued.
        /// </summary>
        LftCalcQueued = 1 << 1,
        /// <summary>
        ///     Income already calculation queued.
        /// </summary>
        IncCalcQueued = 1 << 2,
        /// <summary>
        ///     Average portfolio calculation queued.
        /// </summary>
        AvgCalculated = 1 << 3,
        /// <summary>
        ///     Time-weigthed rate of return calculation queued.
        /// </summary>
        TwrCalcQueued = 1 << 4,
        /// <summary>
        ///     Rate of return standard deviation calculation queued.
        /// </summary>
        StdCalcQueued = 1 << 5,
    }
}
