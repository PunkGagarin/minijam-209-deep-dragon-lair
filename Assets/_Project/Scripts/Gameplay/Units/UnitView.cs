using TMPro;

using UnityEngine;

namespace _Project.Scripts.Gameplay.Units
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;

        public void SetAmount(int amount) => _amountText.text = amount.ToString();
    }
}
