using TMPro;

using UnityEngine;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;

        public void SetAmount(int amount) => _amountText.text = amount.ToString();
    }
}
