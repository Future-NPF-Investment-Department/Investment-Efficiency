using InvestmentData.Context;
using InvestmentData.Context.Entities;
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

        /// <summary>
        ///     Asset value (portfolio) subquery used to construct this efficiency query.
        /// </summary>
        public IQueryable<AssetValue>? AssetsSubquery { get; init; }

        /// <summary>
        ///     Asset flows subquery used to construct this efficiency query.
        /// </summary>
        public IQueryable<AssetFlow>? FlowsSubquery { get; init; }

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
            InvestmentDataContext context = _connstr is not null
                ? new InvestmentDataContext(_connstr)
                : new InvestmentDataContext();
            context.Database.SetCommandTimeout(500);
            return new EfficiencyQueryBuilder(context);
        }

        public IEnumerator<EfficiencyRecord> GetEnumerator()
            => _query.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    
        /// <summary>
        ///     Returns translated SQL-query.
        /// </summary>
        public override string ToString()
            => _query.ToQueryString();

        /// <summary>
        ///     Provides query details.
        /// </summary>
        public EfficiencyQueryDetails GetDetails()
            => _details;
        
        /// <summary>
        ///     Get unique isins from query.
        /// </summary>
        /// <returns></returns>
        public string[] GetUniqueIsins()
        {
            return AssetsSubquery is null
                ? Array.Empty<string>() 
                : AssetsSubquery.Select(av => av.Isin).Distinct().ToArray();
        }
    }
}
