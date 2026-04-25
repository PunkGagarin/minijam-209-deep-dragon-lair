using UnityEngine;

namespace _Project.Scripts.Gameplay.Dragon
{
    public class AnnoyanceModel
    {
        public float Current { get; private set; }
        public float Max { get; private set; }

        public float Normalized => Max > 0f ? Current / Max : 0f;

        public void SetMax(float max) => Max = max;

        public void Add(float amount)
        {
            Current = Mathf.Clamp(Current + amount, 0f, Max);
        }

        public void Reset() => Current = 0f;
    }
}
