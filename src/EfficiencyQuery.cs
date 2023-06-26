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
        private static string? _connstr;
        private readonly IQueryable<EfficiencyRecord> _query;
        private readonly EfficiencyQueryDetails _details;

        internal EfficiencyQuery(IQueryable<EfficiencyRecord> query, EfficiencyQueryDetails details)
        {
            _query = query;        
            _details = details;
        }

        public EfficiencyQuery(IQueryable<EfficiencyRecord> query) : this(query, EfficiencyQueryDetails.Empty)
        {          
        }

        public Type ElementType 
            => _query.ElementType;

        public Expression Expression 
            => _query.Expression;

        public IQueryProvider Provider 
            => _query.Provider;

        /// <summary>
        ///     Sets connection string for efficiency queries.
        /// </summary>
        /// <param name="connString"></param>
        public static void UseConnectionString(string connString)
            => _connstr = connString;

        /// <summary>
        ///     Initializes new efficiency query configuration.
        /// </summary>
        public static EfficiencyQueryBuilder ConfigureNew()
        {
            InvestmentData context = _connstr is not null
                ? new InvestmentData(_connstr)
                : new InvestmentData();
            return new(context);
        }

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
