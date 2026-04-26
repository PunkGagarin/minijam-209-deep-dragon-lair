using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldReserveView : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        public void SetFill(float normalized)
        {
            if (_fill == null)
                return;

            _fill.fillAmount = Mathf.Clamp01(normalized);
        }
    }
}
