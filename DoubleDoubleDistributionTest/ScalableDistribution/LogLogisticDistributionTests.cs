﻿using DoubleDouble;
using DoubleDoubleDistribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleDistributionTest.ScalableDistribution {
    [TestClass()]
    public class LogLogisticDistributionTests {
        readonly LogLogisticDistribution dist_sigma1gamma1 = new(sigma: 1, gamma: 1);
        readonly LogLogisticDistribution dist_sigma2gamma1 = new(sigma: 2, gamma: 1);
        readonly LogLogisticDistribution dist_sigma1gamma2 = new(sigma: 1, gamma: 2);
        readonly LogLogisticDistribution dist_sigma2gamma2 = new(sigma: 2, gamma: 2);
        readonly LogLogisticDistribution dist_sigma3gamma4 = new(sigma: 3, gamma: 4);
        readonly LogLogisticDistribution dist_sigma4gamma5 = new(sigma: 4, gamma: 5);

        LogLogisticDistribution[] Dists => new[]{
            dist_sigma1gamma1,
            dist_sigma2gamma1,
            dist_sigma1gamma2,
            dist_sigma2gamma2,
            dist_sigma3gamma4,
            dist_sigma4gamma5,
        };

        [TestMethod()]
        public void InfoTest() {
            foreach (LogLogisticDistribution dist in Dists) {
                Console.WriteLine(dist);
                Console.WriteLine($"Support={dist.Support}");
                Console.WriteLine($"Sigma={dist.Sigma}");
                Console.WriteLine($"Gamma={dist.Gamma}");
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
            foreach (LogLogisticDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = 0; x <= 1; x += 0.125) {
                    ddouble pdf = dist.PDF(x);

                    Console.WriteLine($"pdf({x})={pdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFLowerTest() {
            foreach (LogLogisticDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = 0; x <= 1; x += 0.125) {
                    ddouble cdf = dist.CDF(x, Interval.Lower);

                    Console.WriteLine($"cdf({x})={cdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFUpperTest() {
            foreach (LogLogisticDistribution dist in Dists) {
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
            foreach (LogLogisticDistribution dist in Dists) {
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
            foreach (LogLogisticDistribution dist in Dists) {
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
            ddouble[] expected_dist_sigma1gamma1 = [        
            ];
            ddouble[] expected_dist_sigma2gamma1 = [        
            ];
            ddouble[] expected_dist_sigma1gamma2 = [        
            ];
            ddouble[] expected_dist_sigma2gamma2 = [        
            ];
            ddouble[] expected_dist_sigma3gamma4 = [        
            ];
            ddouble[] expected_dist_sigma4gamma5 = [        
            ];

            foreach ((LogLogisticDistribution dist, ddouble[] expecteds) in new[]{
                (dist_sigma1gamma1, expected_dist_sigma1gamma1), 
                (dist_sigma2gamma1, expected_dist_sigma2gamma1),
                (dist_sigma1gamma2, expected_dist_sigma1gamma2), 
                (dist_sigma2gamma2, expected_dist_sigma2gamma2),
                (dist_sigma3gamma4, expected_dist_sigma3gamma4),
                (dist_sigma4gamma5, expected_dist_sigma4gamma5),
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
            ddouble[] expected_dist_sigma1gamma1 = [        
            ];
            ddouble[] expected_dist_sigma2gamma1 = [        
            ];
            ddouble[] expected_dist_sigma1gamma2 = [        
            ];
            ddouble[] expected_dist_sigma2gamma2 = [        
            ];
            ddouble[] expected_dist_sigma3gamma4 = [        
            ];
            ddouble[] expected_dist_sigma4gamma5 = [        
            ];

            foreach ((LogLogisticDistribution dist, ddouble[] expecteds) in new[]{
                (dist_sigma1gamma1, expected_dist_sigma1gamma1), 
                (dist_sigma2gamma1, expected_dist_sigma2gamma1),
                (dist_sigma1gamma2, expected_dist_sigma1gamma2), 
                (dist_sigma2gamma2, expected_dist_sigma2gamma2),
                (dist_sigma3gamma4, expected_dist_sigma3gamma4),
                (dist_sigma4gamma5, expected_dist_sigma4gamma5),
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