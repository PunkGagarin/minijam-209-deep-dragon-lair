using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.Dragon
{
    public class AnnoyanceView : MonoBehaviour
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
