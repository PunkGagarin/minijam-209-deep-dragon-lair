using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Dragon
{
    public class DragonAttackEvent : MonoBehaviour
    {

        public event Action OnAttack = delegate { };

        public void Attack()
        {
            OnAttack.Invoke();
        }
        
    }
}