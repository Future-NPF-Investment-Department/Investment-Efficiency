using InvestmentDataContext;
using InvestmentDataContext.Classifications;
using InvestmentDataContext.SqlServer;

namespace InvestmentEfficiency
{
    /// <summary>
    ///     Provides configuration methods for <see cref="EfficiencyQuery"/>.
    /// </summary>
    public class EfficiencyQueryBuilder
    {
        private readonly AssetsQueryBuilder _assets;
        private readonly FlowsQueryBuilder _flows;
        private readonly EfficiencyQueryDetails _details;

        public EfficiencyQueryBuilder(string? connectionString)
        {
            if (connectionString is not null)
                InvestmentDataManager.UseConnectionString(connectionString);
            _assets = InvestmentDataManager.NewAssetsQuery();
            _flows = InvestmentDataManager.NewFlowsQuery();
            _details = new EfficiencyQueryDetails();
        }

        /// <summary>
        ///     Configures period for which the efficiency is calculated.
        /// </summary>
        /// <param name="start">Start date.</param>
        /// <param name="end">End date.</param>
        public EfficiencyQueryBuilder WithDates(DateTime? start, DateTime? end)
        {
            _assets.WithinDates(start, end);
            _flows.WithinDates(start, end);
            _details.StartDate = start;
            _details.EndDate = end;
            return this;
        }

        /// <summary>
        ///     Configures asset management for which the efficiency is calculated.
        /// </summary>
        /// <param name="amName">Asset management name.</param>
        public EfficiencyQueryBuilder WithAssetManagementCompany(string? amName)
        {
            _assets.WithAssetManagementCompany(amName);
            _flows.WithAssetManagementCompany(amName);
            _details.AmName = amName;
            return this;
        }

        /// <summary>
        ///     Configures fund for which the efficiency is calculated.
        /// </summary>
        /// <param name="fundName">Fund name.</param>
        public EfficiencyQueryBuilder WithFundName(string? fundName)
        {
            _assets.WithFundName(fundName);
            _flows.WithFundName(fundName);
            _details.FundName = fundName;
            return this;
        }

        /// <summary>
        ///     Configures asseet property type for which the efficiency is calculated.
        /// </summary>
        /// <param name="pptype"></param>
        public EfficiencyQueryBuilder WithPensionPropertyType(PensionPropertyType? pptype)
        {
            _assets.WithPensionPropertyType(pptype);
            _flows.WithPensionPropertyType(pptype);
            _details.EntityType = pptype;
            return this;
        }

        /// <summary>
        ///     Configures strategy for which the efficiency is calculated.
        /// </summary>
        /// <param name="strategy">Strategy name.</param>
        public EfficiencyQueryBuilder WithStrategy(string? strategy)
        {
            _assets.WithStrategy(strategy);
            _flows.WithStrategy(strategy);
            _details.StrategyName = strategy;
            return this;
        }

        /// <summary>
        ///     Configures contract for which the efficiency is calculated.
        /// </summary>
        /// <param name="contract">Contract ID.</param>
        public EfficiencyQueryBuilder WithContract(string? contract)
        {
            _assets.WithContract(contract);
            _flows.WithContract(contract);
            _details.Contract = contract;
            return this;
        }

        /// <summary>
        ///     Configures issuer for which the efficiency is calculated.
        /// </summary>
        /// <param name="name">Issuer name.</param>
        public EfficiencyQueryBuilder WithIssuerName(string? name)
        {
            _assets.WithIssuerName(name);
            _flows.WithIssuerName(name);
            
            return this;
        }

        /// <summary>
        ///     Configures issuer for which the efficiency is calculated.
        /// </summary>
        /// <param name="issuerId">Issuer ID.</param>
        public EfficiencyQueryBuilder WithIssuerId(string? issuerId)
        {
            _assets.WithIssuerId(issuerId);
            _flows.WithIssuerId(issuerId);
            return this;
        }

        /// <summary>
        ///     Configures collection of isins for which the efficiency is calculated.
        /// </summary>
        /// <param name="isins">Array of isins.</param>
        public EfficiencyQueryBuilder WithIsins(params string[]? isins)
        {
            _assets.WithIsins(isins);
            _flows.WithIsins(isins);
            _details.IsinList = isins;
            return this;
        }

        /// <summary>
        ///     Configures asset class for which the efficiency is calculated.
        /// </summary>
        /// <param name="class">Asset class.</param>
        public EfficiencyQueryBuilder WithAssetClass(AssetClass? @class)
        {
            _assets.WithAssetClass(@class);
            _flows.WithAssetClass(@class);
            _details.AssetClass = @class;
            return this;
        }

        /// <summary>
        ///     Configures asset type for which the efficiency is calculated.
        /// </summary>
        /// <param name="type">Asset type</param>
        public EfficiencyQueryBuilder WithAssetType(AssetType? type)
        {
            _assets.WithAssetType(type);
            _flows.WithAssetType(type);
            
            return this;
        }

        /// <summary>
        ///     Configures risk type for which the efficiency is calculated.
        /// </summary>
        /// <param name="risk">Risk type.</param>
        public EfficiencyQueryBuilder WithRiskType(RiskType? risk)
        {
            _assets.WithRiskType(risk);
            _flows.WithRiskType(risk);
            _details.RiskType = risk;
            return this;
        }

        /// <summary>
        ///     Configures usage of real prices forefficiency calculation.
        /// </summary>
        public EfficiencyQueryBuilder WithRealPricesUsed()
        {
            _assets.WithRealPrices();
            
            return this;
        }

        /// <summary>
        ///     Configures usage of real prices forefficiency calculation.
        /// </summary>
        public EfficiencyQueryBuilder WithFairPricesUsed()
        {
            _assets.WithFairPrices();
            return this;
        }

        /// <summary>
        ///     Configures accounting method for which the efficiency is calculated.
        /// </summary>
        /// <param name="method">Accounting method.</param>
        public EfficiencyQueryBuilder WithAccountingMethod(AccountingMethod? method)
        {
            _assets.WithAccountingMethod(method);
            return this;
        }

        /// <summary>
        ///     Configures flows transition types which are used in efficiency calculation.
        /// </summary>
        /// <param name="transitionType">Flows transition type.</param>
        public EfficiencyQueryBuilder WithTransType(params TransType[]? transitionTypes)
        {
            _flows.WithTransType(transitionTypes);
            return this;
        }

        /// <summary>
        ///     Generates query for time series of <see cref="EfficiencyRecord"/>.
        /// </summary>
        public EfficiencyQuery GenerateQuery()
        {
            // obtainig distinct dates from asset and flows queries unioned
            IQueryable<DateTime> distinctDates = _assets.GetQuery()
                .Select(av => av.Date)
                .Distinct()
                .Union(_flows.GetQuery()
                .Select(av => av.Date)
                .Distinct())
                ;

            // obtaining Asset values grouped by dates
            var assetValueSeries = _assets.GetQuery()
                .GroupBy(av => av.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Portfolio = g.Sum(av => av.FullValue)
                })
                ;

            // obtaining Asset flows grouped by dates
            var assetFlowSeries = _flows.GetQuery()
                .GroupBy(fl => fl.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Flow = g.Sum(fl => fl.FullValue * IdepSqlFunctions.GetFlowDirection(fl.TransType)),
                    Commision = g.Sum(fl => fl.Comissions.Comission + fl.Comissions.BrokerComission)
                });
                ;

            // left join distinct dates and asset values in EfficiencyRecord
            IQueryable<EfficiencyRecord> semiEfficiencySeries =
                from days in distinctDates
                join av in assetValueSeries on days.Date equals av.Date into joined
                from avj in joined.DefaultIfEmpty()
                select new EfficiencyRecord
                {
                    Date = days.Date,
                    Portfolio = avj.Portfolio 
                };

            // left join distinct dates, asset values and asset flows in new EfficiencyRecord
            IQueryable<EfficiencyRecord> efficiencySeries =
                from eff in semiEfficiencySeries
                join flow in assetFlowSeries on eff.Date equals flow.Date into joined
                from effj in joined.DefaultIfEmpty()
                select new EfficiencyRecord
                {
                    Date = eff.Date,
                    Portfolio = eff.Portfolio,
                    Flow = effj.Flow,
                    Commision = effj.Commision ?? default
                };

            // replace nulls with default values + order by dates
            var efficiencySeriesWihoutNulls = efficiencySeries.Select(effs => new EfficiencyRecord
            {
                Date = effs.Date,
                Portfolio = effs.Portfolio ?? default,
                Flow = effs.Flow ?? default,
                Commision = effs.Commision ?? default,
            }).OrderBy(effrec => effrec.Date);

            return new EfficiencyQuery(efficiencySeriesWihoutNulls, _details);
        }
    }
}
