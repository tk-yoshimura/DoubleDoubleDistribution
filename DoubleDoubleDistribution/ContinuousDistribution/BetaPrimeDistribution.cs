﻿using DoubleDouble;
using static DoubleDouble.ddouble;

namespace DoubleDoubleDistribution {
    public class BetaPrimeDistribution : ContinuousDistribution {

        public ddouble Alpha { get; }
        public ddouble Beta { get; }

        private readonly ddouble pdf_norm;

        public BetaPrimeDistribution(ddouble alpha, ddouble beta) {
            ValidateShape(alpha, alpha => alpha > 0);
            ValidateShape(beta, beta => beta > 0);

            Alpha = alpha;
            Beta = beta;

            pdf_norm = 1d / Beta(alpha, beta);
        }

        public override ddouble PDF(ddouble x) {
            if (x <= 0d) {
                return 0d;
            }

            ddouble pdf = pdf_norm * Exp(Log(x) * (Alpha - 1d) - Log1p(x) * (Alpha + Beta));

            return pdf;
        }

        public override ddouble CDF(ddouble x, Interval interval = Interval.Lower) {
            if (interval == Interval.Lower) {
                if (x <= 0d) {
                    return 0d;
                }

                ddouble cdf = IncompleteBetaRegularized(x / (x + 1d), Alpha, Beta);

                return cdf;
            }
            else {
                if (x <= 0d) {
                    return 1d;
                }

                ddouble cdf = IncompleteBetaRegularized(1d / (x + 1d), Beta, Alpha);

                return cdf;
            }
        }

        public override ddouble Quantile(ddouble p, Interval interval = Interval.Lower) {
            if (!InRangeUnit(p)) {
                return NaN;
            }

            if (interval == Interval.Lower) {
                if (p <= 0d) {
                    return 0d;
                }
                if (p >= 1d) {
                    return PositiveInfinity;
                }

                ddouble u = InverseIncompleteBeta(p, Alpha, Beta);
                ddouble x = u / (1d - u);

                return x;
            }
            else {
                if (p <= 0d) {
                    return PositiveInfinity;
                }
                if (p >= 1d) {
                    return 0d;
                }

                ddouble u = InverseIncompleteBeta(1d - p, Alpha, Beta);
                ddouble x = u / (1d - u);

                return x;
            }
        }

        public override (ddouble min, ddouble max) Support => (0d, PositiveInfinity);

        public override ddouble Mean => (Beta > 1d) ? Alpha / (Beta - 1d) : NaN;

        public override ddouble Median => Quantile(0.5d);

        public override ddouble Mode => (Alpha >= 1d) ?
            (Alpha - 1d / (Beta + 1d))
            : 0d;

        public override ddouble Variance => (Beta > 2d)
            ? Alpha * (Alpha + Beta - 1d) / ((Beta - 2d) * Square(Beta - 1d))
            : NaN;

        public override ddouble Skewness => (Beta > 3d)
            ? 2d * (2d * Alpha + Beta - 1d) / (Beta - 3d) * Sqrt((Beta - 2d) / (Alpha * (Alpha + Beta - 1d)))
            : NaN;

        public override ddouble Kurtosis => (Beta > 4d)
            ? 6d * (Alpha * (Alpha + Beta - 1d) * (5d * Beta - 11d) + Square(Beta - 1d) * (Beta - 2d)) / (Alpha * (Alpha + Beta - 1d) * (Beta - 3d) * (Beta - 4d))
            : NaN;

        public override string ToString() {
            return $"{typeof(BetaPrimeDistribution).Name}[alpha={Alpha},beta={Beta}]";
        }
    }
}
