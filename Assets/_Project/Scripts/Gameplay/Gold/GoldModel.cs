namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldModel
    {
        public int Amount { get; private set; }
        public float Multiplier { get; private set; } = 1f;
        public int BonusPerClick { get; private set; } = 0;

        public void Add(int amount) => Amount += amount;
        public void Spend(int amount) => Amount -= amount;
        public void SetMultiplier(float multiplier) => Multiplier = multiplier;
        public void AddBonusPerClick(int bonus) => BonusPerClick += bonus;
    }
}
