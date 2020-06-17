using System;
using System.Linq;

namespace WorldSharp
{
    public static class Common
    {
        /// <summary>
        /// Calculates the suitable FFT size.
        /// </summary>
        /// <param name="sample">Length of the input signal.</param>
        /// <returns>Suitable FFT size</returns>
        public static int GetSuitableFftSize(int sample) =>
            (int) Math.Pow(2, (int) Math.Log(sample) / Consts.KLog2 + 1);

        //My(Max|Min)(Int|Double)() replaced with standard Math.Max/Min()


        // docs paraphrased from common.h
        /// <summary>
        /// DCCorrection interpolates the power under f0 Hz and is used in CheapTrick() and D4C().
        /// </summary>
        public static double[] DCCorrection(double[] input, double f0, int fs, int fftSize)
        {
            var upperLimit = 2 + (int) (f0 * fftSize / fs);
            var lowFreqAxis = new double[upperLimit];

            for (var i = 0; i < upperLimit; i++)
                lowFreqAxis[i] = ((double) i) * fs / fftSize;

            var upperLimitReplica = upperLimit - 1;
            var lowFreqReplica = Matlab.Interpolate1Q(f0 - lowFreqAxis[0], -(double) fs / fftSize,
                input[..(upperLimit + 1)], lowFreqAxis[..upperLimitReplica]);

            var output = new double[upperLimitReplica];
            for (var i = 0; i < upperLimitReplica; i++)
                output[i] = input[i] + lowFreqReplica[i];
            return output;
        }

        /// <summary>
        /// LinearSmoothing() carries out the spectral smoothing by rectangular window whose length is
        /// <paramref name="width"/> Hz and is used in CheapTrick() and D4C().
        /// </summary>
        public static double[] LinearSmoothing(double[] input, double width, int fs, int fft_size)
        {
        }


        /// <summary>
        /// NuttallWindow() calculates the coefficients of Nuttall window whose length is <paramref name="yLength"/>
        /// and is used in Dio(), Harvest() and D4C().
        /// </summary>
        public static double[] NuttalWindow(int yLength)
        {
            var y = new double[yLength];
            for (var i = 0; i < yLength; i++)
            {
                var tmp = i / (yLength - 1.0);
                y[i] = 0.355768 - 0.487396 * Math.Cos(2.0 * Consts.KPi * tmp) +
                       0.144232 * Math.Cos(4.0 * Consts.KPi * tmp) - 0.012604 * Math.Cos(6.0 * Consts.KPi * tmp);
            }

            return y;
        }


        /// <summary>
        /// GetSafeAperiodicity() limits the range of aperiodicity from 0.001 to 0.999999999999 (1 - world::kMySafeGuardMinimum).
        /// </summary>
        //[[MethodImpl(MethodImplOptions.AggressiveInlining)] :when:
        public static double GetSafeAperiodicity(double x) => Math.Clamp(x, 0.001, 1 - Consts.KMySafeGuardMinimum);

        //Initialize[SomeSortOfFFT]() functions will be implemented as constructors in their respective classes.
    }
}