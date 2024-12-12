using UnityEngine;
using System;
using Lurkers.Taste;
using Lurkers.Inventory;

namespace Lurkers.Environment.Taste
{
    public class ModifierStation : MonoBehaviour
    {
        [SerializeField] private float multiplier = 1f;

        private bool interacting = false;

        public static Action<Vector3> onApproachStation;
        public static Action onLeaveStation;
        public static Action<ModifierStation> onBeginSelectHotbarItem;

        private void Update()
        {
            if (interacting && Input.GetKeyDown(KeyCode.Q))
            {
                onBeginSelectHotbarItem?.Invoke(this);
            }
        }

        public void MultiplyComponents(Flavor originalFlavor)
        {
            originalFlavor.sweet = (int)Math.Clamp(originalFlavor.sweet * multiplier, 0, 100);
            originalFlavor.bitter = (int)Math.Clamp(originalFlavor.bitter * multiplier, 0, 100);
            originalFlavor.sour = (int)Math.Clamp(originalFlavor.sour * multiplier, 0, 100);
            originalFlavor.salty = (int)Math.Clamp(originalFlavor.salty * multiplier, 0, 100);
            originalFlavor.umami = (int)Math.Clamp(originalFlavor.umami * multiplier, 0, 100);
            InventorySystem.Instance.Refresh();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            interacting = true;
            onApproachStation?.Invoke(transform.position);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            interacting = false;
            onLeaveStation?.Invoke();
        }
    }
}
