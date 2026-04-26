using System;
using System.Collections.Generic;

using UnityEngine;

namespace _Project.Scripts.Gameplay.Gold
{
    public class CaveGoldView : MonoBehaviour
    {
        [SerializeField] private List<GoldPile> _goldPile;

        public event Action<GoldPile> OnActiveGoldPileChanged = delegate { };

        public GoldPile ActiveGoldPile { get; private set; }
        public Transform ActiveGatherPoint => ActiveGoldPile != null ? ActiveGoldPile.GatherPoint : null;
        public int GoldPileCount => _goldPile.Count;

        private void Awake()
        {
            if (_goldPile.Count > 0)
                SetStage(0);
        }

        public void SetStage(int stageIndex)
        {
            if (_goldPile.Count == 0)
                throw new InvalidOperationException($"{nameof(CaveGoldView)} on {name} has no gold piles configured.");

            if (stageIndex < 0 || stageIndex >= _goldPile.Count)
                throw new ArgumentOutOfRangeException(nameof(stageIndex), stageIndex,
                    $"{nameof(CaveGoldView)} on {name} cannot switch to stage {stageIndex}.");

            GoldPile nextPile = _goldPile[stageIndex];
            if (nextPile == null)
                throw new InvalidOperationException($"{nameof(CaveGoldView)} on {name} has a missing gold pile at index {stageIndex}.");
            if (nextPile.GatherPoint == null)
                throw new InvalidOperationException(
                    $"{nameof(CaveGoldView)} on {name} has no gather point on gold pile at index {stageIndex}.");

            for (int i = 0; i < _goldPile.Count; i++)
            {
                GoldPile pile = _goldPile[i];
                if (pile == null)
                    continue;

                pile.gameObject.SetActive(i == stageIndex);
            }

            if (ActiveGoldPile == nextPile)
                return;

            ActiveGoldPile = nextPile;
            OnActiveGoldPileChanged.Invoke(ActiveGoldPile);
        }
    }
}
