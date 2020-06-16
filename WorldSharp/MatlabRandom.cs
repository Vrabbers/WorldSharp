namespace WorldSharp
{
    // Implementation of the random function in matlabfunctions.cpp (wrapped into a class to avoid the global statics used in that version)
    public class MatlabRandom
    {
        int gRandomX = 123456789; //These seeds (and the same ones in Reseed() are the same ones in matlabfunctions.cpp
        int gRandomY = 362436069;
        int gRandomZ = 521288629;
        int gRandomW = 88675123;

        /// <summary>
        /// Forces the pseudorandom generator to use initial values.
        /// </summary>
        public void Reseed()
        {
            gRandomX = 123456789;
            gRandomY = 362436069;
            gRandomZ = 521288629;
            gRandomW = 88675123;
        }

        /// <summary>
        /// Generates a pseudorandom number.
        /// </summary>
        public double Random()
        {
            var t = gRandomX ^ (gRandomX << 11);
            gRandomX = gRandomY;
            gRandomY = gRandomZ;
            gRandomZ = gRandomW;
            gRandomW = gRandomW ^ (gRandomW >> 19) ^ (t ^ (t >> 8));

            var tmp = gRandomW >> 4;
            for (var i = 0; i < 11; ++i) {
                t = gRandomX ^ (gRandomX << 11);
                gRandomX = gRandomY;
                gRandomY = gRandomZ;
                gRandomZ = gRandomW;
                gRandomW = gRandomW ^ (gRandomW >> 19) ^ (t ^ (t >> 8));
                tmp += gRandomW >> 4;
            }
            return tmp / 268435456.0 - 6.0;
        }
    }
}