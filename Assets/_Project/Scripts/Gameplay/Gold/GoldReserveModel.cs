using UnityEngine;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldReserveModel
    {
        public int Current { get; private set; }
        public int Max { get; private set; }

        public float Normalized => Max > 0 ? (float)Current / Max : 0f;

        public void SetMax(int max)
        {
            Max = Mathf.Max(0, max);
            Current = Max;
        }

        public int Consume(int amount)
        {
            if (amount <= 0)
                return 0;

            int taken = Mathf.Min(amount, Current);
            Current -= taken;
            return taken;
        }
    }
}
