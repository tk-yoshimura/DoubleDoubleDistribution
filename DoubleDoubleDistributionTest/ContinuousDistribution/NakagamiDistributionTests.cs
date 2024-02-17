﻿using DoubleDouble;
using DoubleDoubleDistribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleDistributionTest.ContinuousDistribution {
    [TestClass()]
    public class NakagamiDistributionTests {
        readonly NakagamiDistribution dist_m1omega1 = new(m: 1, omega: 1);
        readonly NakagamiDistribution dist_m2omega1 = new(m: 2, omega: 1);
        readonly NakagamiDistribution dist_m1omega2 = new(m: 1, omega: 2);
        readonly NakagamiDistribution dist_m2omega2 = new(m: 2, omega: 2);
        readonly NakagamiDistribution dist_m3omega4 = new(m: 3, omega: 4);

        NakagamiDistribution[] Dists => new[]{
            dist_m1omega1,
            dist_m2omega1,
            dist_m1omega2,
            dist_m2omega2,
            dist_m3omega4,
        };

        [TestMethod()]
        public void InfoTest() {
            foreach (NakagamiDistribution dist in Dists) {
                Console.WriteLine(dist);
                Console.WriteLine($"Support={dist.Support}");
                Console.WriteLine($"M={dist.M}");
                Console.WriteLine($"Omega={dist.Omega}");
                Console.WriteLine($"Mean={dist.Mean}");
                Console.WriteLine($"Median={dist.Median}");
                Console.WriteLine($"Mode={dist.Mode}");
                Console.WriteLine($"Variance={dist.Variance}");
                Console.WriteLine($"Skewness={dist.Skewness}");
                Console.WriteLine($"Kurtosis={dist.Kurtosis}");
                /* TODO: Implement */
                //Console.WriteLine($"Entropy={dist.Entropy}");
            }
        }

        [TestMethod()]
        public void PDFTest() {
            foreach (NakagamiDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = 0; x <= 1; x += 0.125) {
                    ddouble pdf = dist.PDF(x);

                    Console.WriteLine($"pdf({x})={pdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFLowerTest() {
            foreach (NakagamiDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = 0; x <= 1; x += 0.125) {
                    ddouble cdf = dist.CDF(x, Interval.Lower);

                    Console.WriteLine($"cdf({x})={cdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFUpperTest() {
            foreach (NakagamiDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = 0; x <= 1; x += 0.125) {
                    ddouble cdf = dist.CDF(x, Interval.Lower);
                    ddouble ccdf = dist.CDF(x, Interval.Upper);

                    Console.WriteLine($"ccdf({x})={ccdf}");

                    Assert.IsTrue(ddouble.Abs(cdf + ccdf - 1) < 1e-28);
                }
            }
        }

        [TestMethod()]
        public void QuantileLowerTest() {
            foreach (NakagamiDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (int i = 0; i <= 10; i++) {
                    ddouble p = (ddouble)i / 10;
                    ddouble x = dist.Quantile(p, Interval.Lower);
                    ddouble cdf = dist.CDF(x, Interval.Lower);

                    Console.WriteLine($"quantile({p})={x}, cdf({x})={cdf}");

                    if (ddouble.IsFinite(x)) {
                        Assert.IsTrue(ddouble.Abs(p - cdf) < 1e-28);
                    }
                }
            }
        }

        [TestMethod()]
        public void QuantileUpperTest() {
            foreach (NakagamiDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (int i = 0; i <= 10; i++) {
                    ddouble p = (ddouble)i / 10;
                    ddouble x = dist.Quantile(p, Interval.Upper);
                    ddouble ccdf = dist.CDF(x, Interval.Upper);

                    Console.WriteLine($"cquantile({p})={x}, ccdf({x})={ccdf}");

                    if (ddouble.IsFinite(x)) {
                        Assert.IsTrue(ddouble.Abs(p - ccdf) < 1e-28);
                    }
                }
            }
        }

        [TestMethod()]
        public void PDFExpectedTest() {
            ddouble[] expected_dist_m1omega1 = [
            ];
            ddouble[] expected_dist_m2omega1 = [
            ];
            ddouble[] expected_dist_m1omega2 = [
            ];
            ddouble[] expected_dist_m2omega2 = [
            ];
            ddouble[] expected_dist_m3omega4 = [
            ];

            foreach ((NakagamiDistribution dist, ddouble[] expecteds) in new[]{
                (dist_m1omega1, expected_dist_m1omega1),
                (dist_m2omega1, expected_dist_m2omega1),
                (dist_m1omega2, expected_dist_m1omega2),
                (dist_m2omega2, expected_dist_m2omega2),
                (dist_m3omega4, expected_dist_m3omega4),
            }) {
                for ((ddouble x, int i) = (0, 0); i < expecteds.Length; x += 0.5, i++) {
                    ddouble expected = expecteds[i];
                    ddouble actual = dist.PDF(x);

                    Console.WriteLine($"{dist} pdf({x})");
                    Console.WriteLine(expected);
                    Console.WriteLine(actual);

                    Assert.IsTrue(ddouble.Abs(expected - actual) / expected < 1e-30, $"{dist} pdf({x})\n{expected}\n{actual}");
                }
            }
        }

        [TestMethod()]
        public void CDFExpectedTest() {
            ddouble[] expected_dist_m1omega1 = [
            ];
            ddouble[] expected_dist_m2omega1 = [
            ];
            ddouble[] expected_dist_m1omega2 = [
            ];
            ddouble[] expected_dist_m2omega2 = [
            ];
            ddouble[] expected_dist_m3omega4 = [
            ];

            foreach ((NakagamiDistribution dist, ddouble[] expecteds) in new[]{
                (dist_m1omega1, expected_dist_m1omega1),
                (dist_m2omega1, expected_dist_m2omega1),
                (dist_m1omega2, expected_dist_m1omega2),
                (dist_m2omega2, expected_dist_m2omega2),
                (dist_m3omega4, expected_dist_m3omega4),
            }) {
                for ((ddouble x, int i) = (0, 0); i < expecteds.Length; x += 0.5, i++) {
                    ddouble expected = expecteds[i];
                    ddouble actual = dist.CDF(x);

                    Console.WriteLine($"{dist} cdf({x})");
                    Console.WriteLine(expected);
                    Console.WriteLine(actual);

                    Assert.IsTrue(ddouble.Abs(expected - actual) / expected < 1e-30, $"{dist} cdf({x})\n{expected}\n{actual}");
                }
            }
        }
    }
}