using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.Currencies
{
    public class CurrencyIconView : MonoBehaviour
    {
        [SerializeField] private CurrencyDefinition _currency;
        [SerializeField] private Image _image;

        private void Awake() => _image.sprite = _currency.Icon;
    }
}
