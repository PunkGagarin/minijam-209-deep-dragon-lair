using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Utils
{
    public abstract class ClickableView<T> : MonoBehaviour where T : ClickableView<T>
    {
        protected Collider2D _collider2D;

        public event Action<T> OnClicked = delegate { };

        public virtual void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        public void DisableInteract()
        {
            _collider2D.enabled = false;
        }

        public virtual void OnMouseDown()
        {
            if (IsPointerOverUI())
                return;

            Interact();
        }

        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        protected virtual void Interact()
        {
            OnClicked.Invoke(this as T);
        }
    }
}