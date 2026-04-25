namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldModel
    {
        public int Amount { get; private set; }
        public float Multiplier { get; private set; } = 1f;

        public void Add(int amount) => Amount += amount;
        public void SetMultiplier(float multiplier) => Multiplier = multiplier;
    }
}
