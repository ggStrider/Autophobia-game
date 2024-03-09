using UnityEngine;

namespace Autophobia.Utilities
{
    public static class DoRandom
    {
        public static int RandomInt(int min, int max)
        {
            return Random.Range(min, max);
        }

        public static float RandomFloat(float min, float max)
        {
            return Random.Range(min, max);
        }
    }
}