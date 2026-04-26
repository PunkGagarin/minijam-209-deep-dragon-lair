using System;
using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldPile : ClickableView<GoldPile>
    {
        [SerializeField] private Transform _gatherPoint;

        public Transform GatherPoint => _gatherPoint;
    }
}
