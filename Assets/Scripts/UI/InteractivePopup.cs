using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lurkers.Environment.Vision;
using UnityEngine.UI;
using Lurkers.Inventory;

namespace Lurkers.UI
{
    public class InteractivePopup : MonoBehaviour
    {
        [SerializeField] private Vector2 spawnOffset = new Vector2(0, 100);

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
            IngredientSupply.onApproachStation += ShowNormalPopup;
            IngredientSupply.onLeaveStation += HidePopup;
        }

        private void OnDisable()
        {
            PullLever.onApproachLever -= ShowLeverPopup;
            PullLever.onLeaveLever -= HidePopup;
            Vault.onApproachVault -= ShowNormalPopup;
            Vault.onLeaveVault -= HidePopup;
            DetectItem.onLeaveItem -= HidePopup;
            DetectItem.onApproachItem -= ShowNormalPopup;
            IngredientSupply.onApproachStation -= ShowNormalPopup;
            IngredientSupply.onLeaveStation -= HidePopup;
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

        private void HidePopup()
        {
            popupImage.enabled = false;
        }
    }
}
