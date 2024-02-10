﻿using DoubleDouble;
using static DoubleDouble.ddouble;

namespace DoubleDoubleDistribution {
    public class RayleighDistribution : ContinuousDistribution {

        public ddouble Sigma { get; }

        private readonly ddouble sigma_sq;

        public RayleighDistribution() : this(sigma: 1d) { }

        public RayleighDistribution(ddouble sigma) {
            ValidateScale(sigma);

            Sigma = sigma;
            sigma_sq = sigma * sigma;
        }

        public override ddouble PDF(ddouble x) {
            if (IsNegative(x)) {
                return 0d;
            }

            ddouble pdf = x / sigma_sq * Exp(-x * x / (2 * sigma_sq));

            if (IsNaN(x)) {
                return 0d;
            }

            return pdf;
        }

        public override ddouble CDF(ddouble x, Interval interval = Interval.Lower) {
            if (interval == Interval.Lower) {
                if (x <= 0d) {
                    return 0d;
                }

                ddouble cdf = 1d - Exp(-x * x / (2 * sigma_sq));

                return cdf;
            }
            else {
                if (x <= 0d) {
                    return 1d;
                }

                ddouble cdf = Exp(-x * x / (2 * sigma_sq));

                return cdf;
            }
        }

        public override ddouble Quantile(ddouble p, Interval interval = Interval.Lower) {
            if (!InRangeUnit(p)) {
                return NaN;
            }

            if (interval == Interval.Lower) {
                ddouble x = Sigma * Sqrt(-2 * Log1p(-p));

                return x;
            }
            else {
                ddouble x = Sigma * Sqrt(-2 * Log(p));

                return x;
            }
        }

        public override (ddouble min, ddouble max) Support => (Zero, PositiveInfinity);

        public override ddouble Mean => Sigma * Sqrt(PI / 2);
        public override ddouble Median => Sigma * Sqrt(2 * Ln2);
        public override ddouble Mode => Sigma;
        public override ddouble Variance => (4d - PI) / 2 * sigma_sq;
        public override ddouble Skewness => 2 * Sqrt(PI) * (PI - 3d) / Cube(Sqrt(4d - PI));
        public override ddouble Kurtosis => (-16d + PI * (24d + PI * -6d)) / Square(4d - PI);

        public override ddouble Entropy => 1d + Log(Sigma / Sqrt2) + EulerGamma / 2;

        public override string ToString() {
            return $"{typeof(RayleighDistribution).Name}[sigma={Sigma}]";
        }
    }
}
