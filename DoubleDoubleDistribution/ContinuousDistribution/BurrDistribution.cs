﻿using DoubleDouble;
using static DoubleDouble.ddouble;

namespace DoubleDoubleDistribution {
    public class BurrDistribution : ContinuousDistribution {

        public ddouble C { get; }
        public ddouble K { get; }

        private readonly ddouble ck;

        public BurrDistribution(ddouble c, ddouble k) {
            ValidateShape(c, c => c > 0);
            ValidateShape(k, k => k > 0);

            C = c;
            K = k;

            ck = c * k;
        }

        public override ddouble PDF(ddouble x) {
            if (IsNegative(x)) {
                return 0d;
            }

            ddouble xc = Pow(x, C);

            ddouble pdf = ck * xc / (x * Pow(xc + 1d, K + 1d));

            return pdf;
        }

        public override ddouble CDF(ddouble x, Interval interval = Interval.Lower) {
            ddouble xc = Pow(x, C);

            if (interval == Interval.Lower) {
                if (x <= 0d) {
                    return 0d;
                }
                if (IsPositiveInfinity(x) || IsPositiveInfinity(xc)) { 
                    return 1d;
                }

                ddouble cdf = Max(0d, 1d - Pow(1d + xc, -K));

                return cdf;
            }
            else {
                if (x <= 0d) {
                    return 1d;
                }
                if (IsPositiveInfinity(x) || IsPositiveInfinity(xc)) { 
                    return 0d;
                }

                ddouble cdf = Min(1d, Pow(1d + xc, -K));

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

                ddouble x = Pow(Pow(1d / (1d - p), 1d / K) - 1d, 1d / C);

                return x;
            }
            else {
                if (p <= 0d) {
                    return PositiveInfinity;
                }
                if (p >= 1d) {
                    return 0d;
                }

                ddouble x = Pow(Pow(1d / p, 1d / K) - 1d, 1d / C);

                return x;
            }
        }

        public override (ddouble min, ddouble max) Support => (0d, PositiveInfinity);

        public override ddouble Mean => K * Beta(K - 1d / C, 1d + 1d / C);
        public override ddouble Median => Pow(Pow2(1d / K) - 1d, 1d / C);
        public override ddouble Mode => Pow((C - 1d) / (K * C + 1d), 1d / C);
        public override ddouble Variance {
            get {
                ddouble mu1 = K * Beta(K - 1d / C, 1d + 1d / C);
                ddouble mu2 = K * Beta((C * K - 2d) / C, (C + 2d) / C);

                return mu2 - mu1 * mu1;
            }
        }
        public override ddouble Skewness {
            get {
                ddouble mu1 = K * Beta(K - 1d / C, 1d + 1d / C);
                ddouble mu2 = K * Beta((C * K - 2d) / C, (C + 2d) / C);
                ddouble mu3 = K * Beta((C * K - 3d) / C, (C + 3d) / C);

                return (2 * Cube(mu1) - 3d * mu1 * mu2 + mu3) / Cube(Sqrt(mu2 - mu1 * mu1));
            }
        }

        public override ddouble Kurtosis {
            get {
                ddouble mu1 = K * Beta(K - 1d / C, 1d + 1d / C);
                ddouble mu2 = K * Beta((C * K - 2d) / C, (C + 2d) / C);
                ddouble mu3 = K * Beta((C * K - 3d) / C, (C + 3d) / C);
                ddouble mu4 = K * Beta((C * K - 4d) / C, (C + 4d) / C);

                return (-3d * Square(Square(mu1)) + 6d * mu1 * mu1 * mu2 - 4d * mu1 * mu3 + mu4) / Square(mu2 - mu1 * mu1);
            }
        }

        public override string ToString() {
            return $"{typeof(BurrDistribution).Name}[k={K},c={C}]";
        }
    }
}
