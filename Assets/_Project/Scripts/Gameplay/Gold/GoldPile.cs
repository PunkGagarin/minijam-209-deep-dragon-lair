using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldPile : ClickableView<GoldPile>
    {
        
        public override void Awake()
        {
            OnClicked += DoLog;
        }

        private void DoLog(GoldPile obj)
        {
            Debug.Log("GoldPile clicked");
        }

        private void OnDestroy()
        {
            OnClicked -= DoLog;
        }
    }
}