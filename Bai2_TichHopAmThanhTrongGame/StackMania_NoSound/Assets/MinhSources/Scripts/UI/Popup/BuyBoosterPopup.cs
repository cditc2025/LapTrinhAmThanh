using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyBoosterPopup : MonoBehaviour
{
    UIAnimationController controller;
    UnlockBoosterLevelMetadata unlockMetadata;
    public int boosterCountPerBuy = 1;
    [Header("Popup settings")]
    public TMP_Text boosterName;
    public Image boosterIcon;
    public TMP_Text boosterDescription;
    public TMP_Text boosterPrice;
    public TMP_Text boosterCount;
    public Button buyButton;


    private void Awake()
    {
        controller = GetComponent<UIAnimationController>();
        buyButton.onClick.AddListener(delegate {
            bool isBought = PlayerResourceController.instance.BuyBooster(unlockMetadata.newBooster, unlockMetadata.boosterPrice, boosterCountPerBuy);
            if(isBought)
            {

               
                ClosePopup();
            }
        });
    }

    public void OpenPopup(UnlockBoosterLevelMetadata metadata, ParticleSystem gainFX = null)
    {
        unlockMetadata = metadata;
        boosterName.text = unlockMetadata.boosterName;
        boosterDescription.text = unlockMetadata.boosterDescription;
        boosterIcon.sprite = unlockMetadata.boosterIcon;
        boosterPrice.text = unlockMetadata.boosterPrice.ToString();
        boosterCount.text = $"X{boosterCountPerBuy}";

        buyButton.interactable = (PlayerResourceController.instance.gold >= unlockMetadata.boosterPrice);

        controller.Activate();
    }

    public void ClosePopup()
    {
        controller.UpdateObjectChange();
        controller.Deactivate();
    }
}
