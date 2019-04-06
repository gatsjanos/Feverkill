﻿// <copyright file="DoubleExponentialTransformation.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2013 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Linq;

namespace MathNet.Numerics.Integration
{
    /// <summary>
    /// Analytic integration algorithm for smooth functions with no discontinuities
    /// or derivative discontinuities and no poles inside the interval.
    /// </summary>
    public static class DoubleExponentialTransformation
    {
        /// <summary>
        /// Maximum number of iterations, until the asked
        /// maximum error is (likely to be) satisfied.
        /// </summary>
        const int NumberOfMaximumLevels = 10;

        /// <summary>
        /// Approximate the integral by the double exponential transformation
        /// </summary>
        /// <param name="f">The analytic smooth function to integrate.</param>
        /// <param name="intervalBegin">Where the interval starts, inclusive and finite.</param>
        /// <param name="intervalEnd">Where the interval stops, inclusive and finite.</param>
        /// <param name="targetRelativeError">The expected relative accuracy of the approximation.</param>
        /// <returns>Approximation of the finite integral in the given interval.</returns>
        public static double Integrate(Func<double, double> f, double intervalBegin, double intervalEnd, double targetRelativeError)
        {
            return NewtonCotesTrapeziumRule.IntegrateAdaptiveTransformedOdd(
                f,
                intervalBegin, intervalEnd,
                Enumerable.Range(0, NumberOfMaximumLevels).Select(EvaluateAbcissas),
                Enumerable.Range(0, NumberOfMaximumLevels).Select(EvaluateWeights),
                1.0,
                targetRelativeError);
        }

        /// <summary>
        /// Compute the abscissa vector for a single level.
        /// </summary>
        /// <param name="level">The level to evaluate the abscissa vector for.</param>
        /// <returns>Abscissa Vector.</returns>
        static double[] EvaluateAbcissas(int level)
        {
            if (level < PrecomputedAbscissas.Length)
            {
                return PrecomputedAbscissas[level];
            }

            double step = level <= 1 ? 1.0 : (1.0/(2 << (level - 2)));
            double offset = level == 0 ? 0.0 : (1.0/(2 << (level - 1)));
            int length = level == 0 ? 4 : (3 << (level - 1));

            double t = 0;
            var abcissas = new double[length];
            for (int i = 0; i < abcissas.Length; i++)
            {
                double arg = offset + t;
                t += step;

                abcissas[i] = Math.Tanh(Constants.PiOver2*Math.Sinh(arg));
            }

            return abcissas;
        }

        /// <summary>
        /// Compute the weight vector for a single level.
        /// </summary>
        /// <param name="level">The level to evaluate the weight vector for.</param>
        /// <returns>Weight Vector.</returns>
        static double[] EvaluateWeights(int level)
        {
            if (level < PrecomputedWeights.Length)
            {
                return PrecomputedWeights[level];
            }

            double step = level <= 1 ? 1.0 : (1.0/(2 << (level - 2)));
            double offset = level == 0 ? 0.0 : (1.0/(2 << (level - 1)));
            int length = level == 0 ? 4 : (3 << (level - 1));

            double t = 0;
            var weights = new double[length];
            for (int i = 0; i < weights.Length; i++)
            {
                double arg = offset + t;
                t += step;

                // TODO: reuse abcissas as computed in EvaluateAbcissas
                double abcissa = Math.Tanh(Constants.PiOver2*Math.Sinh(arg));
                weights[i] = Constants.PiOver2*(1 - (abcissa*abcissa))*Math.Cosh(arg);
            }

            return weights;
        }

        #region Precomputed Abcissas and Weights

        /// <summary>
        /// Precomputed abscissa vector per level.
        /// </summary>
        static readonly double[][] PrecomputedAbscissas =
        {
            new[]
            {
                0.00000000000000000000,
                0.95136796407274694574,
                0.99997747719246159286,
                0.99999999999995705839
            },
            new[]
            {
                0.67427149224843582608,
                0.99751485645722438683,
                0.99999998887566488198
            },
            new[]
            {
                0.37720973816403417379,
                0.85956905868989663517,
                0.98704056050737689169,
                0.99968826402835320905,
                0.99999920473711471266,
                0.99999999995285644818
            },
            new[]
            {
                0.19435700332493543161,
                0.53914670538796776905,
                0.78060743898320029925,
                0.91487926326457461091,
                0.97396686819567744856,
                0.99405550663140214329,
                0.99906519645578584642,
                0.99990938469514399984,
                0.99999531604122052843,
                0.99999989278161241838,
                0.99999999914270509218,
                0.99999999999823216531
            },
            new[]
            {
                0.097923885287832333262,
                0.28787993274271591456,
                0.46125354393958570440,
                0.61027365750063894488,
                0.73101803479256151149,
                0.82331700550640237006,
                0.88989140278426019808,
                0.93516085752198468323,
                0.96411216422354729193,
                0.98145482667733517003,
                0.99112699244169880223,
                0.99610866543750854254,
                0.99845420876769773751,
                0.99945143443527460584,
                0.99982882207287494166,
                0.99995387100562796075,
                0.99998948201481850361,
                0.99999801714059543208,
                0.99999969889415261122,
                0.99999996423908091534,
                0.99999999678719909830,
                0.99999999978973286224,
                0.99999999999039393352,
                0.99999999999970809734
            },
            new[]
            {
                0.049055967305077886315,
                0.14641798429058794053,
                0.24156631953888365838,
                0.33314226457763809244,
                0.41995211127844715849,
                0.50101338937930910152,
                0.57558449063515165995,
                0.64317675898520470128,
                0.70355000514714201566,
                0.75669390863372994941,
                0.80279874134324126576,
                0.84221924635075686382,
                0.87543539763040867837,
                0.90301328151357387064,
                0.92556863406861266645,
                0.94373478605275715685,
                0.95813602271021369012,
                0.96936673289691733517,
                0.97797623518666497298,
                0.98445883116743083087,
                0.98924843109013389601,
                0.99271699719682728538,
                0.99517602615532735426,
                0.99688031812819187372,
                0.99803333631543375402,
                0.99879353429880589929,
                0.99928111192179195541,
                0.99958475035151758732,
                0.99976797159956083506,
                0.99987486504878034648,
                0.99993501992508242369,
                0.99996759306794345976,
                0.99998451990227082442,
                0.99999293787666288565,
                0.99999693244919035751,
                0.99999873547186590954,
                0.99999950700571943689,
                0.99999981889371276701,
                0.99999993755407837378,
                0.99999997987450320175,
                0.99999999396413420165,
                0.99999999832336194826,
                0.99999999957078777261,
                0.99999999989927772326,
                0.99999999997845533741,
                0.99999999999582460688,
                0.99999999999927152627,
                0.99999999999988636130
            },
            new[]
            {
                0.024539763574649160379,
                0.073525122985671294475,
                0.12222912220155764235,
                0.17046797238201051811,
                0.21806347346971200463,
                0.26484507658344795046,
                0.31065178055284596083,
                0.35533382516507453330,
                0.39875415046723775644,
                0.44078959903390086627,
                0.48133184611690504422,
                0.52028805069123015958,
                0.55758122826077823080,
                0.59315035359195315880,
                0.62695020805104287950,
                0.65895099174335012438,
                0.68913772506166767176,
                0.71750946748732412721,
                0.74407838354734739913,
                0.76886868676824658459,
                0.79191549237614211447,
                0.81326360850297385168,
                0.83296629391941087564,
                0.85108400798784873261,
                0.86768317577564598669,
                0.88283498824466895513,
                0.89661425428007602579,
                0.90909831816302043511,
                0.92036605303195280235,
                0.93049693799715340631,
                0.93957022393327475539,
                0.94766419061515309734,
                0.95485549580502268541,
                0.96121861515111640753,
                0.96682537031235585284,
                0.97174454156548730892,
                0.97604156025657673933,
                0.97977827580061576265,
                0.98301279148110110558,
                0.98579936302528343597,
                0.98818835380074264243,
                0.99022624046752774694,
                0.99195566300267761562,
                0.99341551316926403900,
                0.99464105571251119672,
                0.99566407681695316965,
                0.99651305464025377317,
                0.99721334704346870224,
                0.99778739195890653083,
                0.99825491617199629344,
                0.99863314864067747762,
                0.99893703483351217373,
                0.99917944893488591716,
                0.99937140114093768690,
                0.99952223765121720422,
                0.99963983134560036519,
                0.99973076151980848263,
                0.99980048143113838630,
                0.99985347277311141171,
                0.99989338654759256426,
                0.99992317012928932869,
                0.99994518061445869309,
                0.99996128480785666613,
                0.99997294642523223656,
                0.99998130127012072679,
                0.99998722128200062811,
                0.99999136844834487344,
                0.99999423962761663478,
                0.99999620334716617675,
                0.99999752962380516793,
                0.99999841381096473542,
                0.99999899541068996962,
                0.99999937270733536947,
                0.99999961398855024275,
                0.99999976602333243312,
                0.99999986037121459941,
                0.99999991800479471056,
                0.99999995264266446185,
                0.99999997311323594362,
                0.99999998500307631173,
                0.99999999178645609907,
                0.99999999558563361584,
                0.99999999767323673790,
                0.99999999879798350040,
                0.99999999939177687583,
                0.99999999969875436925,
                0.99999999985405611550,
                0.99999999993088839501,
                0.99999999996803321674,
                0.99999999998556879008,
                0.99999999999364632387,
                0.99999999999727404948,
                0.99999999999886126543,
                0.99999999999953722654,
                0.99999999999981720098,
                0.99999999999992987953
            }
        };

        /// <summary>
        /// Precomputed weight vector per level.
        /// </summary>
        static readonly double[][] PrecomputedWeights =
        {
            new[]
            {
                1.5707963267948966192,
                0.230022394514788685,
                0.00026620051375271690866,
                1.3581784274539090834e-12
            },
            new[]
            {
                0.96597657941230114801,
                0.018343166989927842087,
                2.1431204556943039358e-7
            },
            new[]
            {
                1.3896147592472563229,
                0.53107827542805397476,
                0.076385743570832304188,
                0.0029025177479013135936,
                0.000011983701363170720047,
                1.1631165814255782766e-9
            },
            new[]
            {
                1.5232837186347052132,
                1.1934630258491569639,
                0.73743784836154784136,
                0.36046141846934367417,
                0.13742210773316772341,
                0.039175005493600779072,
                0.0077426010260642407123,
                0.00094994680428346871691,
                0.000062482559240744082891,
                1.8263320593710659699e-6,
                1.8687282268736410132e-8,
                4.9378538776631926964e-11
            },
            new[]
            {
                1.5587733555333301451,
                1.466014426716965781,
                1.297475750424977998,
                1.0816349854900704074,
                0.85017285645662006895,
                0.63040513516474369106,
                0.44083323627385823707,
                0.290240679312454185,
                0.17932441211072829296,
                0.10343215422333290062,
                0.055289683742240583845,
                0.027133510013712003219,
                0.012083543599157953493,
                0.0048162981439284630173,
                0.0016908739981426396472,
                0.00051339382406790336017,
                0.00013205234125609974879,
                0.000028110164327940134749,
                4.8237182032615502124e-6,
                6.4777566035929719908e-7,
                6.5835185127183396672e-8,
                4.8760060974240625869e-9,
                2.5216347918530148572e-10,
                8.6759314149796046502e-12
            },
            new[]
            {
                1.5677814313072218572,
                1.5438811161769592204,
                1.4972262225410362896,
                1.4300083548722996676,
                1.3452788847662516615,
                1.2467012074518577048,
                1.1382722433763053734,
                1.0240449331118114483,
                0.90787937915489531693,
                0.79324270082051671787,
                0.68306851634426375464,
                0.57967810308778764708,
                0.48475809121475539287,
                0.39938474152571713515,
                0.32408253961152890402,
                0.258904639514053516,
                0.20352399885860174519,
                0.15732620348436615027,
                0.11949741128869592428,
                0.089103139240941462841,
                0.065155533432536205042,
                0.046668208054846613644,
                0.032698732726609031113,
                0.022379471063648476483,
                0.014937835096050129696,
                0.0097072237393916892692,
                0.0061300376320830301252,
                0.0037542509774318343023,
                0.0022250827064786427022,
                0.0012733279447082382027,
                0.0007018595156842422708,
                0.00037166693621677760301,
                0.00018856442976700318572,
                0.000091390817490710122732,
                0.000042183183841757600604,
                0.000018481813599879217116,
                7.6595758525203162562e-6,
                2.9916615878138787094e-6,
                1.0968835125901264732e-6,
                3.7595411862360630091e-7,
                1.1992442782902770219e-7,
                3.5434777171421953043e-8,
                9.6498888961089633609e-9,
                2.4091773256475940779e-9,
                5.482835779709497755e-10,
                1.1306055347494680536e-10,
                2.0989335404511469109e-11,
                3.4841937670261059685e-12
            },
            new[]
            {
                1.5700420292795931467,
                1.5640214037732320999,
                1.5520531698454121192,
                1.5342817381543034316,
                1.5109197230741697127,
                1.48224329788553807,
                1.4485862549613225916,
                1.4103329714462590129,
                1.3679105116808964881,
                1.3217801174437728579,
                1.2724283455378627082,
                1.2203581095793582207,
                1.1660798699324345766,
                1.1101031939653403796,
                1.0529288799552666556,
                0.99504180404613271514,
                0.93690461274566793366,
                0.87895234555278212039,
                0.82158803526696470334,
                0.7651792989089561367,
                0.71005590120546898385,
                0.65650824613162753076,
                0.60478673057840362158,
                0.55510187800363350959,
                0.5076251588319080997,
                0.4624903980553677613,
                0.41979566844501548066,
                0.37960556938665160999,
                0.3419537959230168323,
                0.30684590941791694932,
                0.27426222968906810637,
                0.24416077786983990868,
                0.21648020911729617038,
                0.19114268413342749532,
                0.16805663794826916233,
                0.14711941325785693248,
                0.12821973363120098675,
                0.11123999898874453035,
                0.096058391865189467849,
                0.082550788110701737654,
                0.070592469906866999352,
                0.060059642358636300319,
                0.05083075757257047107,
                0.042787652157725676034,
                0.035816505604196436523,
                0.029808628117310126969,
                0.024661087314753282511,
                0.020277183817500123926,
                0.016566786254247575375,
                0.013446536605285730674,
                0.010839937168255907211,
                0.0086773307495391815854,
                0.0068957859690660035329,
                0.0054388997976239984331,
                0.0042565295990178580165,
                0.0033044669940348302363,
                0.0025440657675291729678,
                0.0019418357759843675814,
                0.0014690143599429791058,
                0.0011011261134519383862,
                0.00081754101332469493115,
                0.00060103987991147422573,
                0.00043739495615911687786,
                0.00031497209186021200274,
                0.00022435965205008549104,
                0.00015802788400701191949,
                0.00011002112846666697224,
                0.000075683996586201477788,
                0.000051421497447658802092,
                0.0000344921247593431977,
                0.000022832118109036146591,
                0.000014908514031870608449,
                9.5981941283784710776e-6,
                6.0899100320949039256e-6,
                3.8061983264644899045e-6,
                2.3421667208528096843e-6,
                1.4183067155493917523e-6,
                8.4473756384859863469e-7,
                4.9458288702754198508e-7,
                2.8449923659159806339e-7,
                1.6069394579076224911e-7,
                8.9071395140242387124e-8,
                4.8420950198072369669e-8,
                2.579956822953589238e-8,
                1.3464645522302038796e-8,
                6.8784610955899001111e-9,
                3.4371856744650090511e-9,
                1.6788897682161906807e-9,
                8.0099784479729665356e-10,
                3.7299501843052790038e-10,
                1.6939457789411646876e-10,
                7.4967397573818224522e-11,
                3.230446433325236576e-11,
                1.3542512912336274432e-11,
                5.5182369468174885821e-12,
                2.1835922099233609052e-12
            }
        };

        #endregion
    }
}
