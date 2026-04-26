using TMPro;

using UnityEngine;

namespace _Project.Scripts.Gameplay.Gem
{
    public class GemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;

        public void SetAmount(int amount) => _amountText.text = amount.ToString();
    }
}
