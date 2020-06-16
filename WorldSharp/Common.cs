using System;

namespace WorldSharp
{
    public static class Common
    {
        /// <summary>
        /// Calculates the suitable FFT size.
        /// </summary>
        /// <param name="sample">Length of the input signal.</param>
        /// <returns>Suitable FFT size</returns>
        public static int GetSuitableFftSize(int sample) { }
        
        //My(Max|Min)(Int|Double)() replaced with standard Math.Max/Min()
        
        
        // docs paraphrased from common.h
        /// <summary>
        /// DCCorrection interpolates the power under f0 Hz and is used in CheapTrick() and D4C().
        /// </summary>
        public static double[] DCCorrection(double[] input, double currentF0, int fs, int fftSize) { }
        
        /// <summary>
        /// LinearSmoothing() carries out the spectral smoothing by rectangular window whose length is
        /// <paramref name="width"/> Hz and is used in CheapTrick() and D4C().
        /// </summary>
        public static double[] LinearSmoothing(double[] input, double width, int fs, int fft_size) { }
        
        
        /// <summary>
        /// NuttallWindow() calculates the coefficients of Nuttall window whose length is <paramref name="yLength"/>
        /// and is used in Dio(), Harvest() and D4C().
        /// </summary>
        public static double[] NuttalWindow(int yLength) { }


        /// <summary>
        /// GetSafeAperiodicity() limits the range of aperiodicity from 0.001 to 0.999999999999 (1 - world::kMySafeGuardMinimum).
        /// </summary>
        //[[MethodImpl(MethodImplOptions.AggressiveInlining)] :when:
        public static double GetSafeAperiodicity(double x) => Math.Clamp(x, 0.001, 0.999999999999); //TODO: some nicer way?
        
        //Initialize[SomeSortOfFFT]() functions will be implemented as constructors in their respective classes.
    }
}