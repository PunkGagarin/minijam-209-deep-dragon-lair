using _Project.Scripts.Utils;
using TMPro;

using UnityEngine;

namespace _Project.Scripts.Gameplay.Gem
{
    public class GemView : ContentUi
    {
        [SerializeField] private TMP_Text _amountText;

        public void SetAmount(int amount)
        {
            if (amount > 0 && !content.gameObject.activeSelf)
            {
                Show();
            }
            
            _amountText.text = amount.ToString();
        }
    }
}
