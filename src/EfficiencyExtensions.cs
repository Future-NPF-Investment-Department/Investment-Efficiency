namespace InvestmentEfficiency
{
    /// <summary>
    ///     Efficiency extensions.
    /// </summary>
    public static class EfficiencyExtensions
    {
        /// <summary>
        ///     Casts IQuerable of EfficiencyRecord to EfficiencyQuery.
        /// </summary>
        public static EfficiencyQuery AsEfficiencyQuery(IQueryable<EfficiencyRecord> query)        
            => new EfficiencyQuery(query);
        
    }
}
