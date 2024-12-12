using Lurkers.Control;
using Lurkers.Environment.Hearing;
using Lurkers.Environment.Taste;
using Lurkers.Inventory;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Lurkers.UI
{
    public class Hotbar : MonoBehaviour
    {
        [SerializeField] Image[] hotbarSlots = new Image[4];
        [SerializeField] private float scaleRate = 5f;
        [SerializeField] private float finalScaleSize = 1.5f;
        [SerializeField] private AudioSource hotbarAudioSource;
        [SerializeField] private AudioClip solutionMixSFX;
        [SerializeField] private AudioClip flaskFillSFX;
        [SerializeField] private AudioClip pickupSFX;

        private Coroutine activeCoroutine;
        private ModifierStation curModifierStation;
        private PlayerThrow playerThrow;
        
        private void Start()
        {
            UpdateHotbar();
            playerThrow = FindObjectOfType<PlayerThrow>();
        }

        private void Update()
        {
            InventoryItemData item;
            // check for number input for throw
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                item = InventorySystem.Instance.GetIndexInventory(0);
                if (item != null)
                {
                    Flask flask = item.data as Flask;
                    GameObject obj = Instantiate(flask.prefab);
                    obj.GetComponent<Spillage>();
                    playerThrow.Equip(obj);
                    flask.full = false;
                    InventorySystem.Instance.SetIndexInventory(0, flask);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                item = InventorySystem.Instance.GetIndexInventory(1);
                if (item != null)
                {
                    Flask flask = item.data as Flask;
                    playerThrow.Equip(flask.prefab);
                    flask.full = false;
                    InventorySystem.Instance.SetIndexInventory(1, flask);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                item = InventorySystem.Instance.GetIndexInventory(2);
                if (item != null)
                {
                    Flask flask = item.data as Flask;
                    playerThrow.Equip(flask.prefab);
                    flask.full = false;
                    InventorySystem.Instance.SetIndexInventory(2, flask);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                item = InventorySystem.Instance.GetIndexInventory(3);
                if (item != null)
                {
                    Flask flask = item.data as Flask;
                    playerThrow.Equip(flask.prefab);
                    flask.full = false;
                    InventorySystem.Instance.SetIndexInventory(3, flask);
                }
            }
        }

        private void OnEnable()
        {
            InventorySystem.onUpdateInventory += UpdateHotbar;
            ModifierStation.onBeginSelectHotbarItem += AnimateItems;
            ModifierStation.onLeaveStation += LeaveModifierStation;
            ItemClick.onClick += SelectHotbarItem;
            ItemClick.onSolutionMix += PlaySolutionMixSFX;
            RefillStation.onFillFlask += PlayFlaskFillSFX;
            ItemObject.onPickup += PlayPickupSFX;
        }

        private void OnDisable()
        {
            InventorySystem.onUpdateInventory -= UpdateHotbar;
            ModifierStation.onBeginSelectHotbarItem -= AnimateItems;
            ModifierStation.onLeaveStation -= LeaveModifierStation;
            ItemClick.onClick -= SelectHotbarItem;
            ItemClick.onSolutionMix -= PlaySolutionMixSFX;
            RefillStation.onFillFlask -= PlayFlaskFillSFX;
            ItemObject.onPickup -= PlayPickupSFX;
        }

        private void PlayPickupSFX()
        {
            hotbarAudioSource.PlayOneShot(pickupSFX);
        }

        private void PlaySolutionMixSFX()
        {
            hotbarAudioSource.PlayOneShot(solutionMixSFX);
        }

        private void PlayFlaskFillSFX()
        {
            hotbarAudioSource.PlayOneShot(flaskFillSFX);
        }

        private void LeaveModifierStation()
        {
            curModifierStation = null;
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }

            for (int i = 0; i < hotbarSlots.Length; ++i)
            {
                hotbarSlots[i].transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        private void SelectHotbarItem(ItemData item)
        {
            if (curModifierStation != null)
            {
                curModifierStation.MultiplyComponents((item as Flask).GetFlavor());
            }
        }

        private void AnimateItems(ModifierStation station)
        {
            curModifierStation = station;

            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
            activeCoroutine = StartCoroutine(BounceCoroutine());
        }

        private IEnumerator BounceCoroutine()
        {
            float finalScale = finalScaleSize;
            while (true)
            {
                float scale = hotbarSlots[0].transform.localScale.x;
                float newScale = Mathf.Lerp(scale, finalScale, Time.deltaTime * scaleRate);
                if ((scale / finalScaleSize) >= 0.9f)
                {
                    // scale down
                    finalScale = 1f;
                    newScale = Mathf.Lerp(scale, finalScale, Time.deltaTime * scaleRate);
                }
                else if ((1.0f / scale) >= 0.9f)
                {
                    // scale up
                    finalScale = finalScaleSize;
                    newScale = Mathf.Lerp(scale, finalScale, Time.deltaTime * scaleRate);
                }

                for (int i = 0; i < hotbarSlots.Length; ++i)
                {
                    hotbarSlots[i].transform.localScale = new Vector3(newScale, newScale, 1f);
                }

                yield return null;
            }
        }

        private void UpdateHotbar()
        {
            for (int i = 0; i < hotbarSlots.Length; ++i)
            {
                if (i < InventorySystem.Instance.GetInventoryList().Count)
                {
                    InventoryItemData item = InventorySystem.Instance.GetIndexInventory(i);
                    hotbarSlots[i].sprite = item.data.GetIcon();
                    
                    Flask flask = item.data as Flask;
                    hotbarSlots[i].color = flask.GetColor();
                }
                else
                {
                    //if empty:
                    Color color = hotbarSlots[i].color;
                    color.a = 0f;
                    hotbarSlots[i].color = color;
                }
            }
        }
    }
}
