using InvestmentDataContext;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;

namespace InvestmentEfficiency
{
    /// <summary>
    ///     Represents efficiency query information.
    /// </summary>
    /// <remarks>
    ///     Implements IQuerable of <see cref="EfficiencyRecord"/>.
    /// </remarks>
    public class EfficiencyQuery : IQueryable<EfficiencyRecord>
    {
        private readonly IQueryable<EfficiencyRecord> _query;
        private readonly EfficiencyQueryDetails _details;

        internal EfficiencyQuery(IQueryable<EfficiencyRecord> query, EfficiencyQueryDetails details)
        {
            _query = query;        
            _details = details;
        }     

        public Type ElementType 
            => _query.ElementType;

        public Expression Expression 
            => _query.Expression;

        public IQueryProvider Provider 
            => _query.Provider;

        /// <summary>
        ///     Initializes new efficiency query configuration.
        /// </summary>
        public static EfficiencyQueryBuilder ConfigureNew(InvestmentData context)
            => new(context);

        public IEnumerator<EfficiencyRecord> GetEnumerator()
            => _query.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public override string ToString()
            => _query.ToQueryString();

        /// <summary>
        ///     Provides query details.
        /// </summary>
        public EfficiencyQueryDetails GetDetails()
            => _details;
            
        
    }
}
