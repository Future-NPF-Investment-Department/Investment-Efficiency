#pragma warning disable IDE0017
#pragma warning disable IDE1006

using Microsoft.EntityFrameworkCore;
using RuDataAPI;

namespace InvestmentEfficiency
{
    /// <summary>
    ///     Efficiency calculation configurer.
    /// </summary>
    public class EfficiencyConfigurer
    {
        private const CalculationStage GRR = CalculationStage.GrrCalcQueued;
        private const CalculationStage LFT = CalculationStage.LftCalcQueued;
        private const CalculationStage INC = CalculationStage.IncCalcQueued;
        private const CalculationStage AVG = CalculationStage.AvgCalculated;
        private const CalculationStage TWR = CalculationStage.TwrCalcQueued;
        private const CalculationStage STD = CalculationStage.StdCalcQueued;
        private const CalculationStage BENCH = CalculationStage.HasBenchmarks;

        private readonly EfficiencyQuery _effQuery;
        private readonly List<EfficiencyRecord> _effSeries;
        private readonly Efficiency _eff;
        private CalculationStage _stage;

        private Func<int>?    _lftCalcHandler;
        private Func<double>? _incCalcHandler;
        private Func<double>? _avgCalcHandler;
        private Func<double>? _twrCalcHandler;
        private Func<double>? _mwrCalcHandler;
        private Func<double>? _stdCalcHandler;
        private Func<double>? _shrCalcHandler;
        private Func<double>? _infCalcHandler;

        private Task? _benchCalcHandler;

        /// <summary>
        ///     Initializes efficiency configurer using info from <see cref="EfficiencyQuery"/>.
        /// </summary>
        /// <param name="effQuery">Efficiency query.</param>
        public EfficiencyConfigurer(EfficiencyQuery effQuery)
        {
            _effQuery = effQuery;
            _eff = new Efficiency();
            _eff.Details = _effQuery.GetDetails();
            _effSeries = CalculateGrowthRates().ToList();
            _stage |= CalculationStage.GrrCalcQueued;
        }

        /// <summary>
        ///     Initializes efficiency configurer using info from <see cref="EfficiencyQuery"/> and <see cref="EfficiencyBenchmarks"/>. 
        /// </summary>
        /// <param name="effQuery">Efficiency query.</param>
        public EfficiencyConfigurer(EfficiencyQuery effQuery, EfficiencyBenchmarks benchmarks) : this(effQuery)
        {
            _eff.Benchmarks = benchmarks;
        }

        /// <summary>
        ///     Adds life-time to efficiency calculation.
        /// </summary>
        public EfficiencyConfigurer AddLifeTimeCalculation()
        {
            if (_effSeries.Count == 0) return this;

            _lftCalcHandler = () => (_effSeries.Last().Date - _effSeries.First().Date).Days;

            _stage |= CalculationStage.LftCalcQueued;
            return this;
        }

        /// <summary>
        ///     Adds income to calculation.
        /// </summary>
        public EfficiencyConfigurer AddIncomeCalculation()
        {
            if (_effSeries.Count == 0) return this;

            _incCalcHandler = () =>
            {
                EfficiencyRecord first = _effSeries.First();

                var inc = _effSeries.Last().Portfolio!.Value
                    - first.Portfolio!.Value
                    - _effSeries.Sum(effs => effs.Flow!.Value)
                    - _effSeries.Sum(effs => effs.Commision!.Value)
                    + first.Flow!.Value + first.Commision!.Value;
                
                return inc;
            };

            _stage |= CalculationStage.IncCalcQueued;
            return this;
        }

        /// <summary>
        ///     Adds average portfolio to calculation.
        /// </summary>
        public EfficiencyConfigurer AddAveragePortfolioCalculation()
        {
            if (_effSeries.Count == 0) return this;
           
            _avgCalcHandler = () => (_effSeries.Sum(effs => effs.Portfolio) != 0)
                ? _effSeries
                  .Where(effs => effs.Portfolio != .0)
                  .Average(effs => effs.Portfolio!.Value)
                : .0
                ;

            _stage |= CalculationStage.AvgCalculated;
            return this;
        }

        /// <summary>
        ///     Adds time-weighted rate of return to calculation.
        /// </summary>
        public EfficiencyConfigurer AddTwrCalculation()
        {
            if (_effSeries.Count == 0) return this;

            if (!_stage.HasFlag(GRR | LFT))
                throw new Exception("Growth rates and Life-time should be calculated before the time-weighted rate of return (TWR).");

            _twrCalcHandler = () =>
            {
                double product = _effSeries
                    .Select(effs => effs.Growth!)
                    .Aggregate((gr1, gr2) => gr1 * gr2);

                return Math.Pow(product, 365.0 / _eff.LifeTime!.Value) - 1.0;

            };

            _stage |= CalculationStage.TwrCalcQueued;
            return this;
        }

        /// <summary>
        ///     Adds money-weighted rate of return to calculation.
        /// </summary>
        public EfficiencyConfigurer AddMwrCalculation()
        {
            if (_effSeries.Count == 0) return this;

            if (!_stage.HasFlag(INC | AVG | LFT))
                throw new Exception("Income, average portfolio and Life-time should be calculated before the money-weighted rate of return (MWR).");

            _mwrCalcHandler = () => _eff.Income!.Value 
                / _eff.AverageProtfolio!.Value
                / _eff.LifeTime!.Value 
                * 365.0;

            return this;
        }

        /// <summary>
        ///     Adds TWR standard deviation to calculation.
        /// </summary>
        public EfficiencyConfigurer AddStdCalculation()
        {
            if (_effSeries.Count == 0) return this;

            if (!_stage.HasFlag(GRR))
                throw new Exception("Growth rates should be calculated before the standard deviation (STD).");

            _stdCalcHandler = () =>
            {
                double avg = _effSeries.Average(effs => effs.Growth!);
                int count = _effSeries.Count;
                var stds = _effSeries.Select(effs => Math.Pow(effs.Growth! - avg, 2));
                return Math.Sqrt(stds.Sum() / (count - 1)) * Math.Sqrt(252);
            };

            _stage |= CalculationStage.StdCalcQueued;
            return this;
        }

        /// <summary>
        ///     Adds Sharpe coefficient to calculation.
        /// </summary>
        public EfficiencyConfigurer AddSharpeCalculation()
        {
            return AddSharpeCalculation(_eff.Benchmarks!.RiskFreeRate);
        }


        /// <summary>
        ///     Adds Sharpe coefficient to calculation.
        /// </summary>
        public EfficiencyConfigurer AddSharpeCalculation(double riskFreeRate)
        {
            if (_effSeries.Count == 0) return this;

            if (!_stage.HasFlag(TWR | STD | BENCH))
                throw new Exception("TWR and STD rates and benchmarks should be calculated before the Sharpe coefficient.");

            _shrCalcHandler = () =>            
                (_eff.Twr!.Value - riskFreeRate) / _eff.Std!.Value;
            

            return this;
        }


        /// <summary>
        ///     Adds Information ratio to calculation.
        /// </summary>
        public EfficiencyConfigurer AddInformationRatioCalculation(double indexTwr)
        {
            if (_effSeries.Count == 0) return this;

            if (!_stage.HasFlag(TWR | STD | BENCH))
                throw new Exception("TWR and STD rates and benchmarks should be calculated before the Sharpe coefficient.");

            _infCalcHandler = () =>
                (_eff.Twr!.Value - indexTwr) / _eff.Std!.Value;

            return this;
        }

        public EfficiencyConfigurer AddBenchmarksCalculation(EfirClient efir)
        {

            _benchCalcHandler = new Task(async () =>
            {
                if(efir.IsLoggedIn is false)
                    await efir.LoginAsync();

                var bench = await EfficiencyBenchmarks.CalculateBenchmarks(_eff.Details!.StartDate!.Value, _eff.Details!.EndDate!.Value, efir);
                _eff.Benchmarks = bench;
            });

            _stage |= CalculationStage.HasBenchmarks;

            return this;
        }

        public EfficiencyConfigurer UseBenchmarks(EfficiencyBenchmarks benchmarks)
        {
            _eff.Benchmarks = benchmarks;
            _stage |= CalculationStage.HasBenchmarks;
            return this;
        }

        /// <summary>
        ///     Calculates investment efficiency.
        /// </summary>
        /// <returns>
        ///     Instance of <see cref="Efficiency"/> class.
        /// </returns>
        public Efficiency Calculate()
        {
            _benchCalcHandler?.Wait();
            _eff.EfficiencySeries = _effSeries;
            _eff.LifeTime = _lftCalcHandler?.Invoke();
            _eff.Income = _incCalcHandler?.Invoke();
            _eff.AverageProtfolio = _avgCalcHandler?.Invoke();
            _eff.Mwr = _mwrCalcHandler?.Invoke();
            _eff.Twr = _twrCalcHandler?.Invoke();
            _eff.Std = _stdCalcHandler?.Invoke();
            _eff.SharpeRatio = _shrCalcHandler?.Invoke();
            _eff.InformationRatio = _infCalcHandler?.Invoke();
            return _eff;
        }

        // calculation of growth rates for efficiency series
        private IEnumerable<EfficiencyRecord> CalculateGrowthRates()
        {
            double? prevPortfolio = default;
            foreach (var effrec in _effQuery)
            {
                yield return new EfficiencyRecord
                {
                    Date = effrec.Date,
                    Portfolio = effrec.Portfolio,
                    Flow = effrec.Flow,
                    Commision = effrec.Commision,
                    Growth = CalculateGrowth(effrec.Portfolio!.Value, effrec.Flow!.Value, effrec.Commision!.Value)
                };

                prevPortfolio = effrec.Portfolio;
            }

            double CalculateGrowth(double portfolio, double flow, double commision)
            {
                if (Math.Floor(portfolio / 1000) > 10
                    && Math.Floor(portfolio / 1000) - Math.Floor(flow / 1000) is 0.0
                    || prevPortfolio is null || prevPortfolio is 0.0) return 1.0;

                //if(prevPortfolio is null) return 1.0;

                return (portfolio - flow - commision) / prevPortfolio.Value;
            }
        }

    }
}
