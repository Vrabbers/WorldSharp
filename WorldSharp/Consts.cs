namespace WorldSharp
{
    public static class Consts
    {
        // constantnumber.h consts
        public const double KCutOff = 50.0;

        // for StoneMask()
        public const double KFloorF0StoneMask = 40.0;

        public const double KPi = 3.1415926535897932384;
        public const double KMySafeGuardMinimum = 0.000000000001;
        public const double KEps = 0.00000000000000022204460492503131;
        public const double KFloorF0 = 71.0;
        public const double KCeilF0 = 800.0;
        public const double KDefaultF0 = 500.0;
        public const double KLog2 = 0.69314718055994529;

        // Maximum standard deviation not to be selected as a best f0.
        public const double KMaximumValue = 100000.0;

        // for D4C()
        public const int KHanning = 1;
        public const int KBlackman = 2;
        public const double KFrequencyInterval = 3000.0;
        public const double KUpperLimit = 15000.0;
        public const double KThreshold = 0.85;
        public const double KFloorF0D4C = 47.0;

        // for Codec (Mel scale)
        // S. Stevens & J. Volkmann,
        // The Relation of Pitch to Frequency: A Revised Scale,
        // American Journal of Psychology, vol. 53, no. 3, pp. 329-353, 1940.
        public const double KM0 = 1127.01048;
        public const double KF0 = 700.0;
        public const double KFloorFrequency = 40.0;
        public const double KCeilFrequency = 20000.0;
    }
}