using System;

namespace utils
{
    [Serializable]
    public struct IntRange
    {
        public int Min;
        public int Max;

        public int Length
        {
            get
            {
                return (Max - Min) + 1;
            }
        }

        public IntRange(int min, int max) : this()
        {
            Min = min;
            Max = max;
        }

        public int RandomInRange()
        {
            return RandomInRange(new Random());
        }

        public int RandomInRange(Random random)
        {
            return random.Next(Min, Max + 1);
        }

        public bool IsInRange(int value)
        {
            return value >= Min && value <= Max;
        }

        public override bool Equals(object obj)
        {
            return obj is IntRange range &&
                   Min == range.Min &&
                   Max == range.Max &&
                   Length == range.Length;
        }

        public override int GetHashCode()
        {
            int hashCode = -1027186755;
            hashCode = hashCode * -1521134295 + Min.GetHashCode();
            hashCode = hashCode * -1521134295 + Max.GetHashCode();
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            return hashCode;
        }
    }
}