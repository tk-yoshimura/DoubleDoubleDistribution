﻿using DoubleDouble;
using DoubleDoubleDistribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleDistributionTest.StableDistribution {
    [TestClass()]
    public class NormalDistributionTests {
        readonly NormalDistribution dist1 = new();
        readonly NormalDistribution dist2 = new(mu: 1, sigma: 3);

        NormalDistribution[] Dists => new[]{
            dist1,
            dist2,
        };

        [TestMethod()]
        public void InfoTest() {
            foreach (NormalDistribution dist in Dists) {
                Console.WriteLine(dist);
                Console.WriteLine($"Support={dist.Support}");
                Console.WriteLine($"Mu={dist.Mu}");
                Console.WriteLine($"Sigma={dist.Sigma}");
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
            foreach (NormalDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = -4; x <= 4; x += 0.125) {
                    ddouble pdf = dist.PDF(x);

                    Console.WriteLine($"pdf({x})={pdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFLowerTest() {
            foreach (NormalDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = -4; x <= 4; x += 0.125) {
                    ddouble cdf = dist.CDF(x, Interval.Lower);

                    Console.WriteLine($"cdf({x})={cdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFUpperTest() {
            foreach (NormalDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = -4; x <= 4; x += 0.125) {
                    ddouble cdf = dist.CDF(x, Interval.Lower);
                    ddouble ccdf = dist.CDF(x, Interval.Upper);

                    Console.WriteLine($"ccdf({x})={ccdf}");

                    Assert.IsTrue(ddouble.Abs(cdf + ccdf - 1) < 1e-28);
                }
            }
        }

        [TestMethod()]
        public void QuantileLowerTest() {
            foreach (NormalDistribution dist in Dists) {
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
            foreach (NormalDistribution dist in Dists) {
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
            ddouble[] expected_dist1 = [
                "0.0001338302257648853517740744884406858731923",
                "0.0008726826950457600656011929613112427263034",
                "0.004431848411938007175602352696121011243169",
                "0.01752830049356853736215832216674858614851",
                "0.05399096651318805195056420041071358173981",
                "0.1295175956658917276140995579547414911838",
                "0.2419707245191433497978301929355606548287",
                "0.3520653267642994777746804415965176531103",
                "0.3989422804014326779399460599343818684759",
                "0.3520653267642994777746804415965176531103",
                "0.2419707245191433497978301929355606548287",
                "0.1295175956658917276140995579547414911838",
                "0.05399096651318805195056420041071358173981",
                "0.01752830049356853736215832216674858614851",
                "0.004431848411938007175602352696121011243169",
                "0.0008726826950457600656011929613112427263034",
                "0.0001338302257648853517740744884406858731923"
            ];
            ddouble[] expected_dist2 = [
                "0.03315904626424956090386652077667751259278",
                "0.04317253188863057587136651931824716372794",
                "0.05467002489199787326262287993766497372985",
                "0.06733289518468629307240961543731172921740",
                "0.08065690817304778326594339764518688494289",
                "0.09397062513676751754666289610010602963638",
                "0.1064826685074507366966609128148166332109",
                "0.1173551089214331592582268138655058843701",
                "0.1257944092309977213393073414497281190758",
                "0.1311465720339799586474428350624029228944",
                "0.1329807601338108926466486866447939561586",
                "0.1311465720339799586474428350624029228944",
                "0.1257944092309977213393073414497281190758",
                "0.1173551089214331592582268138655058843701",
                "0.1064826685074507366966609128148166332109",
                "0.09397062513676751754666289610010602963638",
                "0.08065690817304778326594339764518688494289"
            ];

            foreach ((NormalDistribution dist, ddouble[] expecteds) in new[]{
                (dist1, expected_dist1), (dist2, expected_dist2)
            }) {
                for ((ddouble x, int i) = (-4, 0); i < expecteds.Length; x += 0.5, i++) {
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
            ddouble[] expected_dist1 = [
                "0.00003167124183311992125377075672215129844383",
                "0.0002326290790355250363499258867279847735487",
                "0.001349898031630094526651814767594977377829",
                "0.006209665325776135166978104574192221127898",
                "0.02275013194817920720028263716653343747178",
                "0.06680720126885806600449404097988607952290",
                "0.1586552539314570514147674543679620775221",
                "0.3085375387259868963622953893916622601164",
                "0.5000000000000000000000000000000000000000",
                "0.6914624612740131036377046106083377398836",
                "0.8413447460685429485852325456320379224779",
                "0.9331927987311419339955059590201139204771",
                "0.9772498680518207927997173628334665625282",
                "0.9937903346742238648330218954258077788721",
                "0.9986501019683699054733481852324050226222",
                "0.9997673709209644749636500741132720152265",
                "0.9999683287581668800787462292432778487016"
            ];
            ddouble[] expected_dist2 = [
                "0.04779035227281470785895860646203371450939",
                "0.06680720126885806600449404097988607952290",
                "0.09121121972586786983385267001382299018818",
                "0.1216725045743812568204959445618528576978",
                "0.1586552539314570514147674543679620775221",
                "0.2023283809636430253685335997122385484848",
                "0.2524925375469229130640618243894173252324",
                "0.3085375387259868963622953893916622601164",
                "0.3694413401817636382727922820695735833283",
                "0.4338161673890963463825581650621288993852",
                "0.5000000000000000000000000000000000000000",
                "0.5661838326109036536174418349378711006148",
                "0.6305586598182363617272077179304264166717",
                "0.6914624612740131036377046106083377398836",
                "0.7475074624530770869359381756105826747676",
                "0.7976716190363569746314664002877614515152",
                "0.8413447460685429485852325456320379224779"
            ];

            foreach ((NormalDistribution dist, ddouble[] expecteds) in new[]{
                (dist1, expected_dist1), (dist2, expected_dist2)
            }) {
                for ((ddouble x, int i) = (-4, 0); i < expecteds.Length; x += 0.5, i++) {
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