using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Environment.Vision;
using UnityEngine.UI;
using Lurkers.Inventory;
using Lurkers.Environment.Taste;
using Lurkers.Taste;
using TMPro;

namespace Lurkers.UI
{
    public class InteractivePopup : MonoBehaviour
    {
        [SerializeField] private Vector2 spawnOffset = new Vector2(0, 100);
        [SerializeField] private TextMeshProUGUI flavorBreakdownText;

        private Image popupImage;
        private Camera cam;
        private Vector3 spawnPos;

        private void Start()
        {
            popupImage = GetComponent<Image>();
            cam = Camera.main;
        }

        private void OnEnable()
        {
            PullLever.onApproachLever += ShowLeverPopup;
            PullLever.onLeaveLever += HidePopup;
            Vault.onApproachVault += ShowNormalPopup;
            Vault.onLeaveVault += HidePopup;
            DetectItem.onLeaveItem += HidePopup;
            DetectItem.onApproachItem += ShowNormalPopup;
            RefillStation.onApproachRefill += ShowNormalPopup;
            RefillStation.onDisplayFlavorData += ShowFlavorBreakdown;
            RefillStation.onLeaveRefill += HidePopup;
            RefillStation.onLeaveRefill += HideFlavorBreakdown;
            ModifierStation.onApproachStation += ShowNormalPopup;
            ModifierStation.onLeaveStation += HidePopup;
            ItemClick.onEnterHover += OnHoverHotbarItem;
            ItemClick.onExitHover += HideFlavorBreakdown;
        }

        private void OnDisable()
        {
            PullLever.onApproachLever -= ShowLeverPopup;
            PullLever.onLeaveLever -= HidePopup;
            Vault.onApproachVault -= ShowNormalPopup;
            Vault.onLeaveVault -= HidePopup;
            DetectItem.onLeaveItem -= HidePopup;
            DetectItem.onApproachItem -= ShowNormalPopup;
            RefillStation.onApproachRefill -= ShowNormalPopup;
            RefillStation.onDisplayFlavorData -= ShowFlavorBreakdown;
            RefillStation.onLeaveRefill -= HidePopup;
            RefillStation.onLeaveRefill -= HideFlavorBreakdown;
            ModifierStation.onApproachStation -= ShowNormalPopup;
            ModifierStation.onLeaveStation -= HidePopup;
            ItemClick.onEnterHover -= OnHoverHotbarItem;
            ItemClick.onExitHover -= HideFlavorBreakdown;
        }

        private void Update()
        {
            if (popupImage.enabled)
            {
                transform.position = cam.WorldToScreenPoint(this.spawnPos) + new Vector3(spawnOffset.x, spawnOffset.y, 0);
            }
        }

        private void ShowLeverPopup(bool unlocked, Vector3 spawnPos)
        {
            if (unlocked)
            {
                popupImage.enabled = true;
                this.spawnPos = spawnPos;
                transform.position = cam.WorldToScreenPoint(this.spawnPos) + new Vector3(spawnOffset.x, spawnOffset.y, 0);
            }
        }

        private void ShowNormalPopup(Vector3 spawnPos)
        {
            popupImage.enabled = true;
            this.spawnPos = spawnPos;
            transform.position = cam.WorldToScreenPoint(this.spawnPos) + new Vector3(spawnOffset.x, spawnOffset.y, 0);
        }

        private void OnHoverHotbarItem(ItemData item)
        {
            if ((item as Flask).GetFlavor() != null)
            {
                ShowFlavorBreakdown((item as Flask).GetFlavor());
            }
        }

        private void ShowFlavorBreakdown(Flavor flav)
        {
            if (flav == null)
            {
                return;
            }

            flavorBreakdownText.enabled = true;
            flavorBreakdownText.text = $"Sweet: {flav.sweet}\nBitter: {flav.bitter}\nSour: {flav.sour}\nSalty: {flav.salty}\nUmami: {flav.umami}";
        }

        private void HidePopup()
        {
            popupImage.enabled = false;
        }

        private void HideFlavorBreakdown()
        {
            flavorBreakdownText.enabled = false;
        }
    }
}
