#pragma warning disable IDE0090

namespace InvestmentEfficiency
{
    /// <summary>
    ///     Efficiency extensions.
    /// </summary>
    public static class EfficiencyExtensions
    {
        /// <summary>
        ///     Casts IQuerable of <see cref="EfficiencyRecord"/> to <see cref="EfficiencyQuery"/>.
        /// </summary>
        public static EfficiencyQuery AsEfficiencyQuery(this IQueryable<EfficiencyRecord> query)        
            => new EfficiencyQuery(query);        
    }
}
