using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Lurkers.Taste;
using Lurkers.Inventory;

namespace Lurkers.Environment.Taste
{
    public class RefillStation : MonoBehaviour
    {
        [SerializeField] private Vector2 sweetRange;
        [SerializeField] private Vector2 saltyRange;
        [SerializeField] private Vector2 bitterRange;
        [SerializeField] private Vector2 sourRange;
        [SerializeField] private Vector2 umamiRange;

        private Flavor flav;
        private int[] compositions = new int[5];
        private bool interacting = false;

        public static Action<Vector3> onApproachRefill;
        public static Action<Flavor> onDisplayFlavorData; 
        public static Action onLeaveRefill;
        public static Action onFillFlask;

        private void Update()
        {
            if (interacting)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    for (int i = 0; i < InventorySystem.Instance.GetInventoryList().Count; i++)
                    {
                        ItemData item = InventorySystem.Instance.GetInventoryList()[i].data;
                        if (item is Flask someFlask && !someFlask.full)
                        {
                            someFlask.SetFlav(flav);
                            InventorySystem.Instance.Refresh();
                            onFillFlask?.Invoke();
                            return;
                        }
                    }
                }
            }
        }

        public void InstantiateFlavor()
        {
            flav = ScriptableObject.CreateInstance<Flavor>();

            flav.sweet = (int)UnityEngine.Random.Range(sweetRange.x, sweetRange.y) * 5;
            compositions[0] = flav.sweet;
            flav.bitter = (int)UnityEngine.Random.Range(bitterRange.x, bitterRange.y) * 5;
            compositions[1] = flav.bitter;
            flav.sour = (int)UnityEngine.Random.Range(sourRange.x, sourRange.y) * 5;
            compositions[2] = flav.sour;
            flav.salty = (int)UnityEngine.Random.Range(saltyRange.x, saltyRange.y) * 5;
            compositions[3] = flav.salty;
            flav.umami = (int)UnityEngine.Random.Range(umamiRange.x, umamiRange.y) * 5;
            compositions[4] = flav.umami;
        }

        public Flavor GetFlavor()
        {
            return flav;
        }

        public void MoreThanThree()
        {
            if (compositions.Count(c => c == 0) > 3)
            {
                if (flav.sweet == 0)
                {
                    flav.sweet = UnityEngine.Random.Range(1, 20) * 5;
                    compositions[0] = flav.sweet;
                }
                else if (flav.bitter == 0)
                {
                    flav.bitter = UnityEngine.Random.Range(1, 20) * 5;
                    compositions[1] = flav.bitter;
                }
                else if (flav.sour == 0)
                {
                    flav.sour = UnityEngine.Random.Range(1, 20) * 5;
                    compositions[2] = flav.sour;
                }
                else if (flav.salty == 0)
                {
                    flav.salty = UnityEngine.Random.Range(1, 20) * 5;
                    compositions[3] = flav.salty;
                }
                else
                {
                    flav.umami = UnityEngine.Random.Range(1, 20) * 5;
                    compositions[4] = flav.umami;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            interacting = true;
            onApproachRefill?.Invoke(transform.position);
            onDisplayFlavorData?.Invoke(flav);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != null)
            {
                interacting = false;
                onLeaveRefill?.Invoke();
            }
        }
    }
}
