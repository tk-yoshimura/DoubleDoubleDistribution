﻿using DoubleDouble;
using DoubleDoubleDistribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleDistributionTest.ContinuousDistribution {
    [TestClass()]
    public class HyperbolicSecantDistributionTests {
        readonly HyperbolicSecantDistribution dist = new();
        readonly HyperbolicSecantDistribution dist_a1b2 = new();
        readonly HyperbolicSecantDistribution dist_a2b4 = new();

        HyperbolicSecantDistribution[] Dists => new[]{
            dist
        };

        [TestMethod()]
        public void InfoTest() {
            foreach (HyperbolicSecantDistribution dist in Dists) {
                Console.WriteLine(dist);
                Console.WriteLine($"Support={dist.Support}");
                Console.WriteLine($"Mean={dist.Mean}");
                Console.WriteLine($"Median={dist.Median}");
                Console.WriteLine($"Mode={dist.Mode}");
                Console.WriteLine($"Variance={dist.Variance}");
                Console.WriteLine($"Skewness={dist.Skewness}");
                Console.WriteLine($"Kurtosis={dist.Kurtosis}");
                Console.WriteLine($"Entropy={dist.Entropy}");
            }
        }

        [TestMethod()]
        public void PDFTest() {
            foreach (HyperbolicSecantDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = -1; x <= 5; x += 0.125) {
                    ddouble pdf = dist.PDF(x);

                    Console.WriteLine($"pdf({x})={pdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFLowerTest() {
            foreach (HyperbolicSecantDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = -1; x <= 5; x += 0.125) {
                    ddouble cdf = dist.CDF(x, Interval.Lower);

                    Console.WriteLine($"cdf({x})={cdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFUpperTest() {
            foreach (HyperbolicSecantDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = -1; x <= 5; x += 0.125) {
                    ddouble cdf = dist.CDF(x, Interval.Lower);
                    ddouble ccdf = dist.CDF(x, Interval.Upper);

                    Console.WriteLine($"ccdf({x})={ccdf}");

                    Assert.IsTrue(ddouble.Abs(cdf + ccdf - 1) < 1e-28);
                }
            }
        }

        [TestMethod()]
        public void QuantileLowerTest() {
            foreach (HyperbolicSecantDistribution dist in Dists) {
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
            foreach (HyperbolicSecantDistribution dist in Dists) {
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
            ddouble[] expected_dist = [
            ];

            foreach ((HyperbolicSecantDistribution dist, ddouble[] expecteds) in new[]{
                (dist, expected_dist)
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
            ddouble[] expected_dist = [
            ];

            foreach ((HyperbolicSecantDistribution dist, ddouble[] expecteds) in new[]{
                (dist, expected_dist),
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