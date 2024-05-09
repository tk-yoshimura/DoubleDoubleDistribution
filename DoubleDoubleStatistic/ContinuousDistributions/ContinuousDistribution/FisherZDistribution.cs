﻿using DoubleDouble;
using DoubleDoubleStatistic.Utils;
using System.Diagnostics;
using static DoubleDouble.ddouble;

namespace DoubleDoubleStatistic.ContinuousDistributions {
    [DebuggerDisplay("{ToString(),nq}")]
    public class FisherZDistribution : ContinuousDistribution {

        public ddouble N { get; }
        public ddouble M { get; }

        private readonly ddouble pdf_lognorm;

        private readonly SnedecorFDistribution randam_gen_f_dist;

        public FisherZDistribution(ddouble n, ddouble m) {
            ValidateShape(n, n => n > 0d);
            ValidateShape(m, m => m > 0d);

            N = n;
            M = m;

            pdf_lognorm = (Log2(n) * n + Log2(m) * m) * 0.5d - LogBeta(n * 0.5d, m * 0.5d) * LbE + 1d;

            randam_gen_f_dist = new(n, m);
        }

        public override ddouble PDF(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            ddouble pdf = Pow2(N * x * LbE - Log2(N * Exp(2d * x) + M) * (N + M) * 0.5d + pdf_lognorm);
            pdf = IsFinite(pdf) ? pdf : 0d;

            return pdf;
        }

        public override ddouble CDF(ddouble x, Interval interval = Interval.Lower) {
            ddouble u = N * Exp(2d * x), upm = u + M;

            if (interval == Interval.Lower) {
                if (IsNegativeInfinity(x)) {
                    return 0d;
                }
                if (IsPositiveInfinity(upm)) {
                    return 1d;
                }

                ddouble cdf = IncompleteBetaRegularized(u / upm, N * 0.5d, M * 0.5d);

                return cdf;
            }
            else {
                if (IsNegativeInfinity(x)) {
                    return 1d;
                }
                if (IsPositiveInfinity(upm)) {
                    return 0d;
                }

                ddouble cdf = IncompleteBetaRegularized(M / upm, M * 0.5d, N * 0.5d);

                return cdf;
            }
        }

        public override ddouble Quantile(ddouble p, Interval interval = Interval.Lower) {
            if (!InRangeUnit(p)) {
                return NaN;
            }

            if (interval == Interval.Lower) {
                ddouble u = InverseIncompleteBeta(p, N * 0.5d, M * 0.5d);
                ddouble x = Log(M * u / (N * (1d - u))) * 0.5d;

                return x;
            }
            else {
                ddouble u = InverseIncompleteBeta(p, M * 0.5d, N * 0.5d);
                ddouble x = Log(M * (1d - u) / (N * u)) * 0.5d;

                return x;
            }
        }

        public override double Sample(Random random) {
            double s;

            do {
                s = randam_gen_f_dist.Sample(random);
            } while (s <= 0d);

            double r = double.Log(s) * 0.5d;

            return r;
        }

        public override bool Symmetric => N == M;

        private ddouble? mean = null;
        public override ddouble Mean => mean ??=
            Abs(N - M) < Hypot(N, M) * 1e-30
            ? 0d
            : IntegrationStatistics.Mean(this, eps: 1e-28, discontinue_eval_points: 2048);

        public override ddouble Median =>
            Abs(N - M) < Hypot(N, M) * 1e-30
            ? 0d
            : Quantile(0.5d);

        public override ddouble Mode => 0d;

        private ddouble? variance = null;
        public override ddouble Variance => variance ??=
            IntegrationStatistics.Variance(this, eps: 1e-28, discontinue_eval_points: 2048);

        private ddouble? skewness = null;
        public override ddouble Skewness => skewness ??=
            Abs(N - M) < Hypot(N, M) * 1e-30
            ? 0d
            : IntegrationStatistics.Skewness(this, eps: 1e-28, discontinue_eval_points: 2048);

        private ddouble? kurtosis = null;
        public override ddouble Kurtosis => kurtosis ??=
            IntegrationStatistics.Kurtosis(this, eps: 1e-28, discontinue_eval_points: 2048);

        private ddouble? entropy = null;
        public override ddouble Entropy => entropy ??=
            IntegrationStatistics.Entropy(this, eps: 1e-28, discontinue_eval_points: 2048);

        public override string ToString() {
            return $"{typeof(FisherZDistribution).Name}[n={N},m={M}]";
        }

        public override string Formula => "p(x; n, m) := 2 * exp(n * x) * (m + exp(2 * x) * n)^((- m - n) / 2) * m^(m / 2) * n^(n / 2) / beta(n / 2, m / 2)";
    }
}