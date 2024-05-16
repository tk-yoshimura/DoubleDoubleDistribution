﻿using Algebra;
using DoubleDouble;
using DoubleDoubleStatistic.MultiVariateDistributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleStatisticTest.MultiVariateDistributions {
    [TestClass()]
    public class MultiVariateNormalDistribution3DTests {
        readonly MultiVariateNormalDistribution dist1 = new(
            mu: new Vector(0.5, -0.25, 0.125),
            cov: new Matrix(new double[,] { { 2.0, 0.25, 1 }, { 0.25, 0.5, 0.375 }, { 1, 0.375, 1.5 } })
        );
        readonly MultiVariateNormalDistribution dist2 = new(
            mu: new Vector(0.25, 1, -0.5),
            cov: new Matrix(new double[,] { { 0.328125, -0.28125, -0.21875 }, { -0.28125, 1.078125, 0.34375 }, { -0.21875, 0.34375, 0.328125 } })
        );

        MultiVariateNormalDistribution[] Dists => [
            dist1,
            dist2,
        ];

        [TestMethod()]
        public void InfoTest() {
            foreach (MultiVariateNormalDistribution dist in Dists) {
                Console.WriteLine(dist);
                Console.WriteLine($"Mu={dist.Mu}");
                Console.WriteLine($"Mean={dist.Mean}");
                Console.WriteLine($"Mode={dist.Mode}");
                Console.WriteLine($"Covariance={dist.Covariance}");
                Console.WriteLine($"Entropy={dist.Entropy}");
                Console.WriteLine(dist.Formula);
            }
        }

        [TestMethod()]
        public void MeanTest() {
            Assert.AreEqual(new Vector(0.5, -0.25, 0.125), dist1.Mean);
            Assert.AreEqual(new Vector(0.25, 1, -0.5), dist2.Mean);
        }

        [TestMethod()]
        public void ModeTest() {
            Assert.AreEqual(new Vector(0.5, -0.25, 0.125), dist1.Mode);
            Assert.AreEqual(new Vector(0.25, 1, -0.5), dist2.Mode);
        }

        [TestMethod()]
        public void CovarianceTest() {
            Assert.AreEqual(new Matrix(new double[,] { { 2.0, 0.25, 1 }, { 0.25, 0.5, 0.375 }, { 1, 0.375, 1.5 } }), dist1.Covariance);
            Assert.AreEqual(new Matrix(new double[,] { { 0.328125, -0.28125, -0.21875 }, { -0.28125, 1.078125, 0.34375 }, { -0.21875, 0.34375, 0.328125 } }), dist2.Covariance);
        }

        [TestMethod()]
        public void EntropyTest() {
            Assert.AreEqual(4.152995917224896, (double)dist1.Entropy, 1e-10);
            Assert.AreEqual(2.672451324732033, (double)dist2.Entropy, 1e-10);
        }

        [TestMethod()]
        public void PDFTest() {
            foreach (MultiVariateNormalDistribution dist in Dists) {
                Console.WriteLine(dist);
                for (int z = -5; z <= 5; z++) {
                    for (int y = -5; y <= 5; y++) {
                        for (int x = -5; x <= 5; x++) {
                            ddouble pdf = dist.PDF((x, y, z));

                            Console.WriteLine($"pdf({x})={pdf}");
                        }
                    }
                }
            }
        }

        [TestMethod()]
        public void RandomGenerateAndFitTest() {
            Random random = new(1234);

            foreach (MultiVariateNormalDistribution dist in Dists) {
                double[][] samples = dist.Sample(random, count: 100000).ToArray();

                MultiVariateNormalDistribution? dist_fit = MultiVariateNormalDistribution.Fit(samples);

                Assert.IsNotNull(dist_fit);

                Console.WriteLine(dist_fit);

                Assert.IsTrue((dist.Mu - dist_fit.Mu).Norm < 1e-2, $"{dist},mu");
                Assert.IsTrue((dist.Covariance - dist_fit.Covariance).Norm < 1e-1, $"{dist},cov");
            }
        }

        [TestMethod()]
        public void PDFExpectedTest() {
            ddouble[] expected_dist1 = [
                1.085113245672586675e-05,
                2.158005370409127717e-05,
                2.027258434583967896e-05,
                8.995904771858197869e-06,
                1.885644077908377093e-06,
                1.867040471147728458e-07,
                8.732264485161865408e-09,
                4.032892570122690863e-04,
                8.020364565003026231e-04,
                7.534435240890169925e-04,
                3.343385371125102805e-04,
                7.008116454222220923e-05,
                6.938974963431539457e-06,
                3.245401777464009596e-07,
                1.278573417325454438e-03,
                2.542746862647010057e-03,
                2.388689618198581999e-03,
                1.059974592694223330e-03,
                2.221827447195560642e-04,
                2.199907083431317759e-05,
                1.028910234789627058e-06,
                3.457817895176228031e-04,
                6.876692011129132342e-04,
                6.460054304199784733e-04,
                2.866631720466351202e-04,
                6.008786513782899996e-05,
                5.949504328602384409e-06,
                2.782620203247915281e-07,
                7.977117580333973300e-06,
                1.586439263127379932e-05,
                1.490321767142040461e-05,
                6.613262753245642679e-06,
                1.386215179302502640e-06,
                1.372538896284133866e-07,
                6.419449842540106955e-09,
                1.569846895724620055e-08,
                3.122013343035019352e-08,
                2.932860116725055573e-08,
                1.301448787640825302e-08,
                2.727984856835069728e-09,
                2.701070786401449866e-10,
                1.263307617831244510e-11,
                2.635336230673961602e-12,
                5.240991906889069680e-12,
                4.923456259431300048e-12,
                2.184770471424486822e-12,
                4.579527691220462560e-13,
                4.534346454042590618e-14,
                2.120742057600701889e-15,
                7.176472090221207740e-06,
                2.353074608297034057e-05,
                3.644513248277090939e-05,
                2.666382852342621916e-05,
                9.214772565994232623e-06,
                1.504270269682411226e-06,
                1.159968677924143540e-07,
                4.935267888278580591e-04,
                1.618212041662675862e-03,
                2.506335839741752423e-03,
                1.833674471744171919e-03,
                6.337009406712546021e-04,
                1.034488348024345257e-04,
                7.977117580333973300e-06,
                2.895195476892254566e-03,
                9.492980502236712109e-03,
                1.470301582620730206e-02,
                1.075695617110308290e-02,
                3.717504578609044739e-03,
                6.068659399849696875e-04,
                4.679647642243027852e-05,
                1.448813489974704440e-03,
                4.750476546913742597e-03,
                7.357682008810857363e-03,
                5.382995150465546066e-03,
                1.860313345167590631e-03,
                3.036878053568206419e-04,
                2.341788900446778517e-05,
                6.184641063803235697e-05,
                2.027865734822080035e-04,
                3.140819891654402212e-04,
                2.297872920440858959e-04,
                7.941236319014067287e-05,
                1.296371192415437654e-05,
                9.996541236453169828e-07,
                2.252080693191581759e-07,
                7.384288308187022029e-07,
                1.143700946556984297e-06,
                8.367494873421575995e-07,
                2.891728850489728223e-07,
                4.720617580760917401e-08,
                3.640149409651472563e-09,
                6.995533237711474652e-11,
                2.293747042587508649e-10,
                3.552624916961983653e-10,
                2.599155913034652829e-10,
                8.982442480505389402e-11,
                1.466343425818296931e-11,
                1.130722636290831137e-12,
                1.497055558031689375e-06,
                8.093005884818020392e-06,
                2.066621451151252802e-05,
                2.492821308480933497e-05,
                1.420366766695696880e-05,
                3.822865291124247713e-06,
                4.860227806875207944e-07,
                1.905003560923699689e-04,
                1.029835195256559831e-03,
                2.629776298149329346e-03,
                3.172115720038048807e-03,
                1.807417055336646905e-03,
                4.864597010747113721e-04,
                6.184641063803235697e-05,
                2.067859820828587408e-03,
                1.117874457575092871e-02,
                2.854592430301569617e-02,
                3.443295739197907435e-02,
                1.961930772664135375e-02,
                5.280465039219787819e-03,
                6.713357929831964771e-04,
                1.914757710175547280e-03,
                1.035108238522927139e-02,
                2.643241485846232336e-02,
                3.188357836752186286e-02,
                1.816671534477636366e-02,
                4.889505103448986943e-03,
                6.216308150019147745e-04,
                1.512425490241211688e-04,
                8.176094953325588896e-04,
                2.087838988093373895e-03,
                2.518414543358718765e-03,
                1.434949352358365609e-03,
                3.862113787985612767e-04,
                4.910126671024711174e-05,
                1.019064279331110293e-06,
                5.509009445499773540e-06,
                1.406774844439731349e-05,
                1.696894371487692245e-05,
                9.668612682562651066e-06,
                2.602271800788407930e-06,
                3.308417326816023299e-07,
                5.857288267414539764e-10,
                3.166420121347148856e-09,
                8.085737041670441911e-09,
                9.753260608526564591e-09,
                5.557240379863088135e-09,
                1.495710957250532846e-09,
                1.901583088045230223e-10,
                9.850444267798652722e-08,
                8.779605408059524928e-07,
                3.696351941482256443e-06,
                7.351073606789014564e-06,
                6.905694571920972687e-06,
                3.064383390531445766e-06,
                6.423296532520806782e-07,
                2.319379609113649012e-05,
                2.067240543260060229e-04,
                8.703407773401340393e-04,
                1.730879315201704636e-03,
                1.626010638854126283e-03,
                7.215378471547361861e-04,
                1.512425490241208978e-04,
                4.658598477774089836e-04,
                4.152163626076065504e-03,
                1.748126181902118215e-02,
                3.476564039506573056e-02,
                3.265929672420061353e-02,
                1.449247507050384354e-02,
                3.037787802781012365e-03,
                7.981897662587746772e-04,
                7.114187947250162104e-03,
                2.995185000768718978e-02,
                5.956636639359619984e-02,
                5.595742269447787770e-02,
                2.483095579974451891e-02,
                5.204851089472041677e-03,
                1.166606681261217270e-04,
                1.039785216729946808e-03,
                4.377659275046806402e-03,
                8.706015029350035531e-03,
                8.178544243621868604e-03,
                3.629207008486297781e-03,
                7.607231153072982564e-04,
                1.454488167491618099e-06,
                1.296371192415433080e-05,
                5.457926582403843402e-05,
                1.085438310065572621e-04,
                1.019674927353782777e-04,
                4.524780061703616394e-05,
                9.484454252873215644e-06,
                1.546903973038459598e-09,
                1.378740503292260865e-08,
                5.804714334275508446e-08,
                1.154405289679513239e-07,
                1.084463408905909346e-07,
                4.812277205833537113e-08,
                1.008708100470376812e-08,
                2.044394485399413911e-09,
                3.004215765827226942e-08,
                2.085339067857197357e-07,
                6.837563566854674404e-07,
                1.059022562118139330e-06,
                7.747974578527465711e-07,
                2.677628365540574072e-07,
                8.907151927780863188e-07,
                1.308896420977653994e-05,
                9.085541969025639445e-05,
                2.979034522974411152e-04,
                4.614018929859849171e-04,
                3.375688361341127436e-04,
                1.166606681261213069e-04,
                3.310399810688515777e-05,
                4.864597010747118058e-04,
                3.376699607026744775e-03,
                1.107177176368883201e-02,
                1.714829556716313333e-02,
                1.254596104673296603e-02,
                4.335768119941852718e-03,
                1.049516972017132242e-04,
                1.542253932083518381e-03,
                1.070536415431060789e-02,
                3.510153770180003685e-02,
                5.436632512119623367e-02,
                3.977525314706159554e-02,
                1.374595966903267852e-02,
                2.838349771672322891e-05,
                4.170924542150744444e-04,
                2.895195476892254566e-03,
                9.492980502236712109e-03,
                1.470301582620728818e-02,
                1.075695617110307423e-02,
                3.717504578609038234e-03,
                6.548016856043930917e-07,
                9.622240528586704588e-06,
                6.679158775087404396e-05,
                2.190011849953153440e-04,
                3.391956707575226933e-04,
                2.481608539972430215e-04,
                8.576209629305174868e-05,
                1.288608802752872370e-09,
                1.893596812582660668e-08,
                1.314416712995705571e-07,
                4.309806480381795138e-07,
                6.675158858065057952e-07,
                4.883650546262014291e-07,
                1.687744467605642270e-07,
                1.338334168322499687e-11,
                3.242486875781359330e-10,
                3.710829723061493560e-09,
                2.006055595276631696e-08,
                5.122642420386537974e-08,
                6.179086244437372316e-08,
                3.520737214611504125e-08,
                1.078938940774824538e-08,
                2.614029767779752979e-07,
                2.991598649696070021e-06,
                1.617242950477823675e-05,
                4.129774549466754522e-05,
                4.981458984855701599e-05,
                2.838349771672317809e-05,
                7.419875171081657714e-07,
                1.797671196897266443e-05,
                2.057325739562301738e-04,
                1.112179787045207251e-03,
                2.840050579669314498e-03,
                3.425754919082717957e-03,
                1.951936314630807337e-03,
                4.352750351291201522e-06,
                1.054574875369491564e-04,
                1.206897033861387613e-03,
                6.524423722959036068e-03,
                1.666069963888388433e-02,
                2.009663988093893239e-02,
                1.145072023896151654e-02,
                2.178202984142584002e-06,
                5.277302751466584994e-05,
                6.039553175683236210e-04,
                3.264951599841771183e-03,
                8.337345986441643125e-03,
                1.005675892873450668e-02,
                5.730168510052278084e-03,
                9.298231769820292242e-08,
                2.252755342815863671e-06,
                2.578141964843651366e-05,
                1.393730378370789649e-04,
                3.559015201589696550e-04,
                4.292991794306601737e-04,
                2.446072991106685093e-04,
                3.385866379892311442e-10,
                8.203203325303217724e-09,
                9.388069062428145941e-08,
                5.075142185718447781e-07,
                1.295982958362106980e-06,
                1.563253847110464853e-06,
                8.907151927780799660e-07,
                2.763474193538606252e-14,
                1.103865894313500306e-12,
                2.082842140955881254e-11,
                1.856416992749209954e-10,
                7.815807472452036326e-10,
                1.554358917550668517e-09,
                1.460185071447707169e-09,
                4.122359454749693515e-11,
                1.646670707777462185e-09,
                3.107039686708979421e-08,
                2.769274328637124893e-07,
                1.165908040896482810e-06,
                2.318685007018118312e-06,
                2.178202984142576379e-06,
                5.245703406646306099e-09,
                2.095388875286099374e-07,
                3.953708755401161198e-06,
                3.523902255280661987e-05,
                1.483618265001197388e-04,
                2.950527234164551237e-04,
                2.771763825961094337e-04,
                5.694151721862348137e-08,
                2.274520926414617002e-06,
                4.291706139692475798e-05,
                3.825156044689399012e-04,
                1.610450847743200324e-03,
                3.202763943827471871e-03,
                3.008718285939534706e-03,
                5.272562870328643245e-08,
                2.106117850417398988e-06,
                3.973952846325488664e-05,
                3.541945617115771581e-04,
                1.491214804076726463e-03,
                2.965634756063202041e-03,
                2.785956029379427733e-03,
                4.164682790729334612e-09,
                1.663576705029309017e-07,
                3.138938963326197171e-06,
                2.797705844403093598e-05,
                1.177878156137007119e-04,
                2.342490423712167101e-04,
                2.200566103550679687e-04,
                2.806141191193751069e-11,
                1.120909166740060354e-09,
                2.115000436825612231e-08,
                1.885079369861962939e-07,
                7.936480587431627388e-07,
                1.578357632083769910e-06,
                1.482729777370844711e-06,
            ];
            ddouble[] expected_dist2 = [
                1.453062281208686506e-36,
                1.415852121253148144e-25,
                5.092307582057717319e-17,
                6.760431632775518429e-11,
                3.312815795429602916e-07,
                5.992157455451105531e-06,
                4.000665221856898894e-07,
                3.750472399854846875e-35,
                2.434133660840121411e-24,
                5.831307399135264837e-16,
                5.156446491260558931e-10,
                1.683054426460627834e-06,
                2.027724137363265194e-05,
                9.017427244035298393e-07,
                2.334664367406084197e-34,
                1.009270745954850772e-23,
                1.610474976281136151e-15,
                9.485568015870947876e-10,
                2.062223679787315805e-06,
                1.654898117250933040e-05,
                4.901960641144984179e-07,
                3.505094029135772553e-34,
                1.009270745954850772e-23,
                1.072701191599632461e-15,
                4.208360216685403100e-10,
                6.094107561255321803e-07,
                3.257394732138730749e-06,
                6.426784382846545871e-08,
                1.269146124453425711e-34,
                2.434133660840173574e-24,
                1.723218235799715332e-16,
                4.502971592239337473e-11,
                4.343313701584836021e-08,
                1.546344501350756010e-07,
                2.032143518093803576e-09,
                1.108307621246631348e-35,
                1.415852121253158246e-25,
                6.676341414560168847e-18,
                1.162043122725939083e-12,
                7.465673925757632856e-10,
                1.770430390460321189e-09,
                1.549714725616527527e-11,
                2.334241296961428383e-37,
                1.986223456185180699e-27,
                6.238404484195811493e-20,
                7.232393034066419616e-15,
                3.094949549184175347e-12,
                4.888641545803327177e-12,
                2.850269201744729317e-14,
                2.439438802302921022e-28,
                8.689826473053313377e-19,
                1.142602963831434046e-11,
                5.545524826921480525e-07,
                9.934660425416663482e-05,
                6.569411493120014809e-05,
                1.603477869159016944e-07,
                2.130675568296147690e-26,
                5.055490336189157252e-17,
                4.427640064915941394e-10,
                1.431345240671868859e-05,
                1.707967307286281612e-03,
                7.522769830866230314e-04,
                1.223035492608682512e-06,
                4.488295697370145370e-25,
                7.093363270624892377e-16,
                4.137957329135286424e-09,
                8.910106180176586644e-05,
                7.081786288171087324e-03,
                2.077618574306204849e-03,
                2.249841314290037405e-06,
                2.280249312728051640e-24,
                2.400368152183233229e-15,
                9.326881177296108951e-09,
                1.337698060890937758e-04,
                7.081786288171099467e-03,
                1.383855044736041718e-03,
                9.981629634695080737e-07,
                2.793958445191509946e-24,
                1.959026211979005982e-15,
                5.070182791436868746e-09,
                4.843619873122823245e-05,
                1.707967307286289201e-03,
                2.223064789586422468e-04,
                1.068040575782442498e-07,
                8.256467740895451213e-25,
                3.856020861044096687e-16,
                6.647334396910327999e-10,
                4.229789396485322567e-06,
                9.934660425416716337e-05,
                8.612919254001779524e-06,
                2.756200390025196268e-09,
                5.884443145328623897e-26,
                1.830523208237122873e-17,
                2.101881236802945797e-11,
                8.908491557262815632e-08,
                1.393680545446535800e-06,
                8.047951828676933245e-08,
                1.715420375669689170e-11,
                5.971545753835949962e-23,
                7.776701887652517170e-15,
                3.738240199245734292e-09,
                6.632878182926535780e-06,
                4.344100907115504772e-05,
                1.050173012958891755e-06,
                9.370976920398703762e-11,
                1.764980115008639050e-20,
                1.530993283971610410e-12,
                4.901960641144940769e-07,
                5.793345370461605061e-04,
                2.527272578279277440e-03,
                4.069469670968110428e-05,
                2.418725663320969359e-09,
                1.258141843015098845e-18,
                7.269220739569369553e-11,
                1.550276533784891757e-05,
                1.220375710245530897e-02,
                3.546018544095765057e-02,
                3.803220587894794104e-04,
                1.505653693359946441e-08,
                2.162998073191405357e-17,
                8.324135964204653720e-10,
                1.182456746454329876e-04,
                6.200039084296306291e-02,
                1.199959688452381934e-01,
                8.572390600691282567e-04,
                2.260478141620707817e-08,
                8.968491393670355628e-17,
                2.298937742760966537e-09,
                2.175194470325255282e-04,
                7.596823497936823488e-02,
                9.793299752199483010e-02,
                4.660034418675845585e-04,
                8.184879061737963045e-09,
                8.968491393670483818e-17,
                1.531270769427086585e-09,
                9.650451988910577444e-05,
                2.244948497782427166e-02,
                1.927649967725052205e-02,
                6.109603609236234720e-05,
                7.147611822092302625e-10,
                2.162998073191389950e-17,
                2.459877675617065644e-10,
                1.032604361813887989e-05,
                1.599990724115504673e-03,
                9.150904858753344546e-04,
                1.931851238975718449e-06,
                1.505380850039733790e-11,
                2.131447988483242141e-20,
                1.014777750425699669e-13,
                1.783323773029377191e-09,
                1.156784676901849301e-07,
                2.769733098349588009e-08,
                2.447858897664167202e-11,
                7.985422605165466555e-17,
                2.131834303578197323e-17,
                6.760431632775493872e-11,
                7.913315291301501795e-07,
                3.419051006629094454e-05,
                5.452752121950120223e-06,
                3.209881476810203436e-09,
                6.974696324123163143e-15,
                5.142435933553381290e-15,
                1.086212135944818407e-08,
                8.468831211511145202e-05,
                2.437223568845626066e-03,
                2.588989725009498599e-04,
                1.015145631313623651e-07,
                1.469228819620752206e-13,
                2.991720864296631436e-13,
                4.209122962520810739e-07,
                2.185874489246327889e-03,
                4.190075954167647720e-02,
                2.964706019119243627e-03,
                7.742914081591744372e-07,
                7.464321052072114945e-13,
                4.197686373338141225e-12,
                3.933736924554017812e-06,
                1.360704129395218231e-02,
                1.737341359640325322e-01,
                8.187846273598258992e-03,
                1.424351795106648243e-06,
                9.145930984233757950e-13,
                1.420481695212370414e-11,
                8.866572069201237406e-06,
                2.042861486194089832e-02,
                1.737341359640325045e-01,
                5.453740408065955482e-03,
                6.319268829301434363e-07,
                2.702727532034078186e-13,
                1.159305863988551851e-11,
                4.819954309456643692e-06,
                7.396919216565731557e-03,
                4.190075954167640088e-02,
                8.761046410773929392e-04,
                6.761656929757764386e-08,
                1.926253090143352443e-14,
                1.109312363555434535e-20,
                1.930801202306989716e-15,
                1.240464481048743594e-12,
                2.941671491917498664e-12,
                2.574939773692884014e-14,
                8.319611470899322746e-19,
                9.922065279377556312e-26,
                3.754552783052109506e-17,
                4.352779859493418879e-12,
                1.862680028640930624e-09,
                2.942204656277053737e-09,
                1.715420375669713726e-11,
                3.691741772640578453e-16,
                2.932615547100080244e-23,
                3.064780049579802418e-14,
                2.366642084717766482e-09,
                6.745729461879618826e-07,
                7.097220887623279939e-07,
                2.756200390025215707e-09,
                3.950902598730576284e-14,
                2.090474729946243498e-21,
                6.033608772128423798e-12,
                3.103381733818082552e-07,
                5.891912902751931006e-05,
                4.128958354052402477e-05,
                1.068040575782450174e-07,
                1.019760222440569310e-12,
                3.593945180372467390e-20,
                2.864782914463592775e-10,
                9.814644036372964793e-06,
                1.241139088662301937e-03,
                5.793345370461640840e-04,
                9.981629634695080737e-07,
                6.347994601219257887e-12,
                1.490166210455094311e-19,
                3.280522540485970991e-09,
                7.486014141311404563e-05,
                6.305526072135574089e-03,
                1.960446855928359242e-03,
                2.249841314290037405e-06,
                9.530414000553283528e-12,
                1.490166210455104903e-19,
                9.060059947040790955e-09,
                1.377093632708716552e-04,
                7.726075268296406576e-03,
                1.599990724115504673e-03,
                1.223035492608676159e-06,
                3.450831245237998018e-12,
                3.593945180372390954e-20,
                8.418294326265629816e-24,
                5.356675315489146103e-20,
                1.258141843015089794e-18,
                1.090754352936575094e-19,
                3.490497802575640129e-23,
                4.122976397074251967e-29,
                1.797617940642861208e-37,
                9.641711243083121345e-20,
                4.086486986163898215e-16,
                6.393065958730035218e-15,
                3.691741772640526191e-16,
                7.868945035099377168e-20,
                6.191056584623343191e-26,
                1.797943750584815145e-34,
                2.663304805016626260e-16,
                7.518680911381499694e-13,
                7.834756455854263753e-12,
                3.013508448745637615e-13,
                4.278409908964308022e-17,
                2.242102355793703108e-23,
                4.337021190622575972e-32,
                1.774289005055957251e-13,
                3.336337098528193902e-10,
                2.315680046767706997e-09,
                5.932670768242531967e-11,
                5.610286087196571961e-15,
                1.958316276103763125e-21,
                2.523153803477068184e-30,
                2.850785799861722677e-11,
                3.570548463195503268e-08,
                1.650700728636433476e-07,
                2.816857124132553935e-09,
                1.774289005055988805e-13,
                4.125218614655201593e-20,
                3.540239487276525897e-29,
                1.104692865633366282e-09,
                9.215877142182509577e-07,
                2.837885501764833265e-06,
                3.225641720491582364e-08,
                1.353319848727895422e-12,
                2.095790372375000416e-19,
                1.198004076789833887e-28,
                1.032417240961482222e-08,
                5.736871968202722126e-06,
                1.176679351417411664e-05,
                8.908491557262847396e-08,
                2.489506580567350829e-12,
                2.567943416346270838e-19,
                9.777339306699083810e-29,
                9.315056572780760027e-30,
                2.166926718443120366e-27,
                1.860655700117683800e-27,
                5.897268161398073061e-30,
                6.899201048777594862e-35,
                2.979269500996836368e-42,
                4.748799637924907687e-52,
                3.610282331445430049e-25,
                5.594028512143772908e-23,
                3.199419966642656093e-23,
                6.754308477647468057e-26,
                5.263245136257257538e-31,
                1.513872742922292654e-38,
                1.607267662606992048e-48,
                3.374687570461300478e-21,
                3.482906882647321601e-19,
                1.326823700897996552e-19,
                1.865725053318520643e-22,
                9.683784843041207849e-28,
                1.855263862834996269e-35,
                1.311986314036649818e-45,
                7.607863549719856249e-18,
                5.229928942776659197e-16,
                1.327064181560330681e-16,
                1.242942768820559916e-19,
                4.297079634952527069e-25,
                5.483511239773034294e-33,
                2.582897305915763654e-43,
                4.136457765556044165e-15,
                1.894028239437681794e-13,
                3.201159922200778343e-14,
                1.997061124370055294e-17,
                4.598735269759459727e-23,
                3.908845702416363938e-31,
                1.226370543940854402e-41,
                5.424144003535424904e-13,
                1.654298395033756776e-11,
                1.862342487674053807e-12,
                7.738705505100855373e-16,
                1.186971124811285872e-21,
                6.720089447519929465e-30,
                1.404342434490956313e-40,
                1.715420375669713726e-11,
                3.484801008223840517e-10,
                2.613054505282502610e-11,
                7.232393034066419616e-15,
                7.388880372360617121e-21,
                2.786366993191705570e-29,
                3.878475604309252141e-40,
            ];

            foreach ((MultiVariateNormalDistribution dist, ddouble[] expecteds) in new[]{
                (dist1,  expected_dist1),
                (dist2,  expected_dist2),
            }) {
                for (int z = -3, i = 0; z <= 3; z++) {
                    for (int y = -3; y <= 3; y++) {
                        for (int x = -3; x <= 3; x++, i++) {
                            ddouble expected = expecteds[i];
                            ddouble actual = dist.PDF((x, y, z));

                            Console.WriteLine($"{dist} pdf({x},{y},{z})");
                            Console.WriteLine(expected);
                            Console.WriteLine(actual);

                            if (expected > 0) {
                                Assert.IsTrue(ddouble.Abs(expected - actual) / expected < 1e-10, $"{dist} pdf({x},{y},{z})\n{expected}\n{actual}");
                            }
                            else {
                                Assert.AreEqual(0, actual);
                            }
                        }
                    }
                }
            }
        }
    }
}
