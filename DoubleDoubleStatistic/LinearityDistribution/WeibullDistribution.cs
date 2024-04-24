﻿using DoubleDouble;
using System.Numerics;
using static DoubleDouble.ddouble;

namespace DoubleDoubleStatistic {
    public class WeibullDistribution : LinearityDistribution<WeibullDistribution>,
        IAdditionOperators<WeibullDistribution, ddouble, WeibullDistribution>,
        ISubtractionOperators<WeibullDistribution, ddouble, WeibullDistribution>,
        IMultiplyOperators<WeibullDistribution, ddouble, WeibullDistribution> {

        public ddouble Alpha { get; }
        public ddouble Mu { get; }
        public ddouble Theta { get; }

        private readonly ddouble pdf_norm, theta_inv;

        public WeibullDistribution(ddouble alpha, ddouble theta) : this(alpha: alpha, mu: 0d, theta: theta) { }

        public WeibullDistribution(ddouble alpha, ddouble mu, ddouble theta) {
            ValidateShape(alpha, alpha => alpha > 0d);
            ValidateLocation(mu);
            ValidateScale(theta);

            Alpha = alpha;
            Mu = mu;
            Theta = theta;

            pdf_norm = Alpha / Theta;
            theta_inv = 1d / theta;
        }

        public override ddouble PDF(ddouble x) {
            ddouble u = (x - Mu) * theta_inv;
            if (IsNegative(u)) {
                return 0d;
            }
            if (IsNaN(u)) {
                return NaN;
            }
            if (u <= 0d) {
                return Alpha < 1d ? PositiveInfinity : Alpha == 1d ? theta_inv : 0d;
            }

            ddouble v = Log2(u) * Alpha;

            ddouble pdf = pdf_norm * Pow2(-Pow2(v) * LbE + v) / u;

            return pdf;
        }

        public override ddouble CDF(ddouble x, Interval interval = Interval.Lower) {
            ddouble u = (x - Mu) * theta_inv;

            if (interval == Interval.Lower) {
                if (u <= 0d) {
                    return 0d;
                }

                ddouble cdf = -Expm1(-Pow(u, Alpha));

                return cdf;
            }
            else {
                if (u <= 0d) {
                    return 1d;
                }

                ddouble cdf = Exp(-Pow(u, Alpha));

                return cdf;
            }
        }

        public override ddouble Quantile(ddouble p, Interval interval = Interval.Lower) {
            if (!InRangeUnit(p)) {
                return NaN;
            }

            if (interval == Interval.Lower) {
                if (p == 1d) {
                    return PositiveInfinity;
                }

                ddouble u = Pow(-Log1p(-p), 1d / Alpha);
                ddouble x = Mu + u * Theta;

                return x;
            }
            else {
                if (p == 0d) {
                    return PositiveInfinity;
                }

                ddouble u = Pow(-Log(p), 1d / Alpha);
                ddouble x = Mu + u * Theta;

                if (IsNegative(x)) {
                    return 0d;
                }

                return x;
            }
        }

        public override (ddouble min, ddouble max) Support => (Mu, PositiveInfinity);

        public override ddouble Mean =>
            Mu + Theta * Gamma(1d + 1d / Alpha);

        public override ddouble Median =>
            Mu + Theta * Pow(Ln2, 1d / Alpha);

        public override ddouble Mode => Alpha <= 1d
            ? Mu
            : Mu + Theta * Pow((Alpha - 1d) / Alpha, 1d / Alpha);

        public override ddouble Variance =>
            Theta * Theta * (Gamma(1d + 2d / Alpha) - Square(Gamma(1d + 1d / Alpha)));

        public override ddouble Skewness {
            get {
                ddouble mu = Gamma(1d + 1d / Alpha), var = Gamma(1d + 2d / Alpha) - Square(mu);

                return (Gamma(1d + 3d / Alpha) - 3d * mu * var - Cube(mu)) / ExMath.Pow3d2(var);
            }
        }

        public override ddouble Kurtosis {
            get {
                ddouble mu = Gamma(1d + 1d / Alpha), var = Gamma(1d + 2d / Alpha) - Square(mu);

                return (Gamma(1d + 4d / Alpha)
                    - 4d * mu * (Gamma(1d + 3d / Alpha) - 3d * mu * var - Cube(mu))
                    - 6d * Square(mu) * var
                    - Square(Square(mu))) /
                    Square(var) - 3d;
            }
        }

        public override ddouble Entropy => 1d + EulerGamma * (1d - 1d / Alpha) + Log(Theta / Alpha);

        public static WeibullDistribution operator +(WeibullDistribution dist, ddouble s) {
            return new(dist.Alpha, dist.Mu + s, dist.Theta);
        }

        public static WeibullDistribution operator -(WeibullDistribution dist, ddouble s) {
            return new(dist.Alpha, dist.Mu - s, dist.Theta);
        }

        public static WeibullDistribution operator *(WeibullDistribution dist, ddouble k) {
            return new(dist.Alpha, dist.Mu * k, dist.Theta * k);
        }

        public override string ToString() {
            return $"{typeof(WeibullDistribution).Name}[alpha={Alpha},mu={Mu},theta={Theta}]";
        }
    }
}