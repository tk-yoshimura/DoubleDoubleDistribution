﻿using DoubleDouble;
using DoubleDoubleDistribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleDistributionTest.StableDistribution {
    [TestClass()]
    public class CauchyDistributionTests {
        readonly CauchyDistribution dist1 = new();
        readonly CauchyDistribution dist2 = new(mu: 1, gamma: 3);

        CauchyDistribution[] Dists => new[]{
            dist1,
            dist2,
        };

        [TestMethod()]
        public void InfoTest() {
            foreach (CauchyDistribution dist in Dists) {
                Console.WriteLine(dist);
                Console.WriteLine($"Support={dist.Support}");
                Console.WriteLine($"Mu={dist.Mu}");
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
            foreach (CauchyDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = -4; x <= 4; x += 0.125) {
                    ddouble pdf = dist.PDF(x);

                    Console.WriteLine($"pdf({x})={pdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFLowerTest() {
            foreach (CauchyDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (ddouble x = -4; x <= 4; x += 0.125) {
                    ddouble cdf = dist.CDF(x, Interval.Lower);

                    Console.WriteLine($"cdf({x})={cdf}");
                }
            }
        }

        [TestMethod()]
        public void CDFUpperTest() {
            foreach (CauchyDistribution dist in Dists) {
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
            foreach (CauchyDistribution dist in Dists) {
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
            foreach (CauchyDistribution dist in Dists) {
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
                "0.01872411095198768656104514863206051318052",
                "0.02402338763651250351228434164113424332596",
                "0.03183098861837906715377675267450287240689",
                "0.04390481188741940297072655541310741021640",
                "0.06366197723675813430755350534900574481378",
                "0.09794150344116636047315923899847037663659",
                "0.1591549430918953357688837633725143620345",
                "0.2546479089470325372302140213960229792551",
                "0.3183098861837906715377675267450287240689",
                "0.2546479089470325372302140213960229792551",
                "0.1591549430918953357688837633725143620345",
                "0.09794150344116636047315923899847037663659",
                "0.06366197723675813430755350534900574481378",
                "0.04390481188741940297072655541310741021640",
                "0.03183098861837906715377675267450287240689",
                "0.02402338763651250351228434164113424332596",
                "0.01872411095198768656104514863206051318052"
            ];
            ddouble[] expected_dist2 = [
                "0.02808616642798152984156772294809076977079",
                "0.03264716781372212015771974633282345887886",
                "0.03819718634205488058453210320940344688827",
                "0.04493786628477044774650835671694523163326",
                "0.05305164769729844525629458779083812067815",
                "0.06261833826566373866316738231049745391520",
                "0.07345612758087477035486942924885278247744",
                "0.08488263631567751241007134046534099308505",
                "0.09549296585513720146133025802350861722068",
                "0.1032356387623104880663029816470363429413",
                "0.1061032953945968905125891755816762413563",
                "0.1032356387623104880663029816470363429413",
                "0.09549296585513720146133025802350861722068",
                "0.08488263631567751241007134046534099308505",
                "0.07345612758087477035486942924885278247744",
                "0.06261833826566373866316738231049745391520",
                "0.05305164769729844525629458779083812067815"
            ];

            foreach ((CauchyDistribution dist, ddouble[] expecteds) in new[]{
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
                "0.07797913037736932546051288977313013511652",
                "0.08858553278290474887587605290700607921396",
                "0.1024163823495667258245989237752594740489",
                "0.1211189415908433987235825893719092475981",
                "0.1475836176504332741754010762247405259511",
                "0.1871670418109988161862527475647785523348",
                "0.2500000000000000000000000000000000000000",
                "0.3524163823495667258245989237752594740489",
                "0.5000000000000000000000000000000000000000",
                "0.6475836176504332741754010762247405259511",
                "0.7500000000000000000000000000000000000000",
                "0.8128329581890011838137472524352214476652",
                "0.8524163823495667258245989237752594740489",
                "0.8788810584091566012764174106280907524019",
                "0.8975836176504332741754010762247405259511",
                "0.9114144672170952511241239470929939207860",
                "0.9220208696226306745394871102268698648835"
            ];
            ddouble[] expected_dist2 = [
                "0.1720208696226306745394871102268698648835",
                "0.1871670418109988161862527475647785523348",
                "0.2048327646991334516491978475505189480977",
                "0.2255627480278025996359139659978706610677",
                "0.2500000000000000000000000000000000000000",
                "0.2788579383763044777428917232261243353092",
                "0.3128329581890011838137472524352214476652",
                "0.3524163823495667258245989237752594740489",
                "0.3975836176504332741754010762247405259511",
                "0.4474315432887465700492218303236554454681",
                "0.5000000000000000000000000000000000000000",
                "0.5525684567112534299507781696763445545319",
                "0.6024163823495667258245989237752594740489",
                "0.6475836176504332741754010762247405259511",
                "0.6871670418109988161862527475647785523348",
                "0.7211420616236955222571082767738756646908",
                "0.7500000000000000000000000000000000000000"
            ];

            foreach ((CauchyDistribution dist, ddouble[] expecteds) in new[]{
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