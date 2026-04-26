namespace _Project.Scripts.Gameplay.Gem
{
    public class GemModel
    {
        public int Amount { get; private set; }

        public void Add(int amount) => Amount += amount;
        public void Spend(int amount) => Amount -= amount;
    }
}
