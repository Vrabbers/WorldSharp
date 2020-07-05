
using System;

namespace WorldSharp
{
    public static class CheapTrick
    {
        /// <summary>
        /// Calculates the spectrogram that consists of spectral envelopes estimated by CheapTrick.
        /// </summary>
        /// <param name="x">Input signal</param>
        /// <param name="fs">Sampling frequency</param>
        /// <param name="temporalPositions">Time axis</param>
        /// <param name="f0">F0 contour</param>
        /// <param name="options">Options for CheapTrick</param>
        /// <returns>Spectrogram estimated by CheapTrick</returns>
        public static double[][] Calculate(double[] x, int fs, double[] temporalPositions, double[] f0,
            CheapTrickOptions options)
        {
            var fftSize = options.FftSize;
            var r = new MatlabRandom();
            
            var f0Floor = GetF0Floor(fs, fftSize);
            var spectralEnvelope = new double[fftSize];
            
            var forwardRealFft = new ForwardRealFft(fftSize);
            var inverseRealFft = new InverseRealFft(fftSize);

            var spectrogram = new double[f0.Length][];
            for(var i = 0; i < spectrogram.Length; i++)
                spectrogram[i] = new double[(fftSize / 2) + 1];
                
            
                
            for (var i = 0; i < f0.Length; i++)
            {
                var currentF0 = f0[i] <= f0Floor ? Consts.KDefaultF0 : f0[i];
                //CheapTrickGeneralBody() Goes Here
                for (var j = 0; j <= fftSize / 2; j++)
                    spectrogram[i][j] = spectralEnvelope[j];
            }

            return spectrogram;
        }

        /// <summary>
        /// Calculates the FFT size based on the sampling frequency and lower limit of F0.
        /// </summary>
        /// <param name="fs">Sampling frequency</param>
        /// <param name="options">Option struct containing lower F0 limit</param>
        public static int GetFftSize(int fs, CheapTrickOptions options)
            => (int) Math.Pow(2.0, 1.0 + (int) (Math.Log(3.0 * fs / options.F0Floor + 1) / Consts.KLog2));

        /// <summary>
        /// Calculates the actual lower F0 for CheapTrick based on the sampling frequency and FFT size used.
        /// Whenever F0 is below this threshold the spectrum will be analyzed as if the frame is unvoiced.
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="fftSize"></param>
        public static double GetF0Floor(int fs, int fftSize) => 3.0 * fs / (fftSize - 3.0);
    }

    public struct CheapTrickOptions
    {
        public double Q1 { get; set; }
        public double F0Floor { get; set; }
        public int FftSize { get; set; }

        public CheapTrickOptions(int fs)
        {
            Q1 = -0.15;

            F0Floor = Consts.KFloorF0;
            FftSize = 0; //Can't use this until everything has been assigned;
            //TODO: Organize this better
            FftSize = CheapTrick.GetFftSize(fs, this);
        }
    }
}