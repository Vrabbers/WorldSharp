
using System;
using System.Linq;

namespace WorldSharp
{
    // These are the MatLab functions that were ported over to the C++ version.
    // However, unlike the C++ version, these follow C# naming rules and some have been renamed, mostly for stylistic purposes.
    // These are all pretty much directly ported from the C++ version.

    public static class Matlab
    {
        public static double[] FftShift(double[] x)
        {
            var y = new double[x.Length];
            for (var i = 0; i < x.Length / 2; i++)
            {
                y[i] = x[i + x.Length / 2];
                y[i + x.Length / 2] = x[i];
            }

            return y;
        }

        public static int[] HistCount(double[] x, double[] edges)
        {
            var count = 1;
            var i = 0;
            var index = new int[edges.Length];
            for (; i < edges.Length; i++)
            {
                index[i] = 1;
                if (edges[i] >= x[0]) break;
            }

            for (; i < edges.Length; i++)
            {
                if (edges[i] < x[count])
                    index[i] = count;
                else
                    index[i--] = count++;

                if (count == x.Length) break;
            }

            count--;
            for (i++; i < edges.Length; i++) index[i] = count;

            return index;
        }

        public static double[] Interpolate1(double[] x, double[] y, double[] xi)
        {
            var h = new double[x.Length - 1];
            var s = new double[xi.Length];
            var yi = new double[xi.Length];

            for (var i = 0; i < x.Length - 1; i++)
                h[i] = x[i + 1] - x[i];
            var k = HistCount(x, xi);

            for (var i = 0; i < xi.Length; i++)
                s[i] = (xi[i] - x[k[i] - 1]) / h[k[i] - 1];

            for (var i = 0; i < xi.Length; i++)
                yi[i] = y[k[i] - 1] + s[i] * (y[k[i]] - y[k[i] - 1]);

            return yi;
        }

        public static double[] Decimate(double[] x, int r)
        {
            const int kNFact = 9;
            var tmp1 = new double[x.Length + kNFact * 2];

            for (var i = 0; i < kNFact; i++)
                tmp1[i] = 2 * x[0] - x[kNFact - i];

            for (var i = kNFact; i < kNFact + x.Length; i++)
                tmp1[i] = x[i - kNFact];

            for (var i = kNFact + x.Length; i < 2 * kNFact + x.Length; i++)
                tmp1[i] = 2 * x[^1] - x[x.Length - 2 - (i - (kNFact + x.Length))];

            var tmp2 = FilterForDecimate(tmp1, r);
            for (var i = 0; i < 2 * kNFact + x.Length; i++)
                tmp1[i] = tmp2[2 * kNFact + x.Length - i - 1];

            tmp2 = FilterForDecimate(tmp1, r);
            for (var i = 0; i < 2 * kNFact + x.Length; i++)
                tmp1[i] = tmp2[2 * kNFact + x.Length - i - 1];

            var nOut = (x.Length - 1) / r + 1;
            var nBeg = r - r * nOut + x.Length;

            var y = new double [x.Length];
            var count = 0;
            for (var i = nBeg; i < x.Length + kNFact; i += r)
                y[count++] = tmp1[i + kNFact - 1];

            return y;
        }

        public static int Round(double x) => x > 0 ? (int) (x + 0.5) : (int) (x - 0.5);

        public static double[] Diff(double[] x)
        {
            var y = new double[x.Length];

            for (var i = 0; i < x.Length - 1; i++)
                y[i] = x[i + 1] - x[i];

            return y;
        }

        public static double[] Interpolate1Q(double x, double deltaX, double[] y,
            int xLength /* ??? */, double[] xi)
        {
            var xiFraction = new double[xi.Length];
            var xiBase = new int[xi.Length];

            for (var i = 0; i < xi.Length; i++)
            {
                xiBase[i] = (int) ((xi[i] - x) / deltaX);
                xiFraction[i] = (xi[i] - x) / deltaX - xiBase[i];
            }

            var deltaY = Diff(y[..xLength]); //???
            deltaY[^0] = 0.0;

            var yi = new double[xi.Length];
            
            for (var i = 0; i < xi.Length; i++)
                yi[i] = y[xiBase[i]] + deltaY[xiBase[i]] * xiFraction[i];

            return yi;
        }

        //TODO: Port fast_fftfilt and fft structs

        public static double Std(double[] x)
        {
            var avg = x.Average();
            var s = x.Average(i => Math.Pow(i - avg, 2.0));
            return Math.Sqrt(s);
        }
        
        static double[] FilterForDecimate(double[] x, int r)
        {
            var a = new double[3];
            var b = new double[2];

            switch (r)
            {
                // TODO: create some sort of nicer lookup table for this
                case 11: // fs : 44100 (default)
                    a[0] = 2.450743295230728;
                    a[1] = -2.06794904601978;
                    a[2] = 0.59574774438332101;
                    b[0] = 0.0026822508007163792;
                    b[1] = 0.0080467524021491377;
                    break;
                case 12: // fs : 48000
                    a[0] = 2.4981398605924205;
                    a[1] = -2.1368928194784025;
                    a[2] = 0.62187513816221485;
                    b[0] = 0.0021097275904709001;
                    b[1] = 0.0063291827714127002;
                    break;
                case 10:
                    a[0] = 2.3936475118069387;
                    a[1] = -1.9873904075111861;
                    a[2] = 0.5658879979027055;
                    b[0] = 0.0034818622251927556;
                    b[1] = 0.010445586675578267;
                    break;
                case 9:
                    a[0] = 2.3236003491759578;
                    a[1] = -1.8921545617463598;
                    a[2] = 0.53148928133729068;
                    b[0] = 0.0046331164041389372;
                    b[1] = 0.013899349212416812;
                    break;
                case 8: // fs : 32000
                    a[0] = 2.2357462340187593;
                    a[1] = -1.7780899984041358;
                    a[2] = 0.49152555365968692;
                    b[0] = 0.0063522763407111993;
                    b[1] = 0.019056829022133598;
                    break;
                case 7:
                    a[0] = 2.1225239019534703;
                    a[1] = -1.6395144861046302;
                    a[2] = 0.44469707800587366;
                    b[0] = 0.0090366882681608418;
                    b[1] = 0.027110064804482525;
                    break;
                case 6: // fs : 24000 and 22050
                    a[0] = 1.9715352749512141;
                    a[1] = -1.4686795689225347;
                    a[2] = 0.3893908434965701;
                    b[0] = 0.013469181309343825;
                    b[1] = 0.040407543928031475;
                    break;
                case 5:
                    a[0] = 1.7610939654280557;
                    a[1] = -1.2554914843859768;
                    a[2] = 0.3237186507788215;
                    b[0] = 0.021334858522387423;
                    b[1] = 0.06400457556716227;
                    break;
                case 4: // fs : 16000
                    a[0] = 1.4499664446880227;
                    a[1] = -0.98943497080950582;
                    a[2] = 0.24578252340690215;
                    b[0] = 0.036710750339322612;
                    b[1] = 0.11013225101796784;
                    break;
                case 3:
                    a[0] = 0.95039378983237421;
                    a[1] = -0.67429146741526791;
                    a[2] = 0.15412211621346475;
                    b[0] = 0.071221945171178636;
                    b[1] = 0.21366583551353591;
                    break;
                case 2: // fs : 8000
                    a[0] = 0.041156734567757189;
                    a[1] = -0.42599112459189636;
                    a[2] = 0.041037215479961225;
                    b[0] = 0.16797464681802227;
                    b[1] = 0.50392394045406674;
                    break;
                default:
                    a[0] = 0.0;
                    a[1] = 0.0;
                    a[2] = 0.0;
                    b[0] = 0.0;
                    b[1] = 0.0;
                    break;
            }

            double wt;
            var w = new double[3];
            var y = new double[x.Length];

            for (var i = 0; i < x.Length; i++)
            {
                wt = x[i] + a[0] * w[0] + a[1] * w[1] + a[2] * w[2];
                y[i] = b[0] * wt + b[1] * w[0] + b[1] * w[1] + b[0] * w[2];
                w[2] = w[1];
                w[1] = w[0];
                w[0] = wt;
            }

            return y;
        }
    }
}