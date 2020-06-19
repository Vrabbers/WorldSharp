using System.Dynamic;
using System.Numerics;
using FFTW.NET;

namespace WorldSharp
{
    //These were originally in the common.h files. However, I thought it would be more sensible to put them here. 
    //They were also originally structs -- but always used as references, so I made them classes, for now.
    public class ForwardRealFft
    {
        public int FftSize { get; set; }
        public AlignedArrayDouble Waveform { get; set; }
        public AlignedArrayComplex Spectrum { get; set; }

        public FftwPlanRC ForwardFft { get; set; }

        public ForwardRealFft(int fftSize)
        {
            FftSize = fftSize;
            Waveform = new AlignedArrayDouble(16, fftSize);
            Spectrum = new AlignedArrayComplex(16, fftSize);
            ForwardFft = FftwPlanRC.Create(Waveform, Spectrum, DftDirection.Forwards, PlannerFlags.Estimate);
        }
    }

    public class InverseRealFft
    {
        public int FftSize { get; set; }
        public AlignedArrayDouble Waveform { get; set; }
        public AlignedArrayComplex Spectrum { get; set; }

        public FftwPlanRC InverseFft { get; set; }

        public InverseRealFft(int fftSize)
        {
            FftSize = fftSize;
            Waveform = new AlignedArrayDouble(16, fftSize);
            Spectrum = new AlignedArrayComplex(16, fftSize);
            InverseFft = FftwPlanRC.Create(Waveform, Spectrum, DftDirection.Backwards, PlannerFlags.Estimate);
        }
    }

    public class InverseComplexFft
    {
        public int FftSize { get; set; }
        public AlignedArrayComplex Input { get; set; }
        public AlignedArrayComplex Output { get; set; }
        public FftwPlanC2C InverseFft { get; set; }

        public InverseComplexFft(int fftSize)
        {
            FftSize = fftSize;
            Input = new AlignedArrayComplex(16, fftSize);
            Output = new AlignedArrayComplex(16, fftSize);
            InverseFft = FftwPlanC2C.Create(Input, Output, DftDirection.Backwards, PlannerFlags.Estimate);
        }

        //TODO: MinimumPhaseAnalysis and friends?    
    }
}