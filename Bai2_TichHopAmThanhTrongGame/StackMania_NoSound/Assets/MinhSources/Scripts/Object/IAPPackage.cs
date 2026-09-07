
using KienChi;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPPackage : MonoBehaviour
{
    public int goldCount = 0;
    public int mixupCount = 0;
    public int pickerCount = 0;
    public int dynamiteCount = 0;
    public int freeReviveTime = 0;
    public int slotCount = 0;
    public bool isOneTimeBuy = false;
    public bool isAds = true;

    Animator animator;
    string packageId = "";
    bool isBought = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        packageId = GetComponent<CodelessIAPButton>().productId;

        if (isOneTimeBuy && packageId != "")
        {
            if (PlayerPrefs.HasKey($"buy_{packageId}") && PlayerPrefs.GetInt($"buy_{packageId}") == 1)
            {
                isBought = true;
                gameObject.SetActive(false);
            }
        }

        // bool isDisableAds = PlayerPrefs.GetInt("ads_jelly", 1) == 1 ? false : true;
        // MaxMediationController.instance.isDisableAds = isDisableAds;
    }

    public void ApplyPackage()
    {
        if (isBought) return;

        if (goldCount > 0)
        {
            PlayerResourceController.instance.GainGold(goldCount);
        }

        if (mixupCount > 0)
        {
            PlayerResourceController.instance.GainBooster(mixupCount, Booster.MIXUP);
        }

        if (pickerCount > 0)
        {
            PlayerResourceController.instance.GainBooster(pickerCount, Booster.PICKER);
        }

        if (dynamiteCount > 0)
        {
            PlayerResourceController.instance.GainBooster(dynamiteCount, Booster.DYNAMITE);
        }

        if (freeReviveTime > 0)
        {
            PlayerResourceController.instance.GainInfiniteLifeDuration(freeReviveTime);
        }

        if (slotCount > 0)
        {
            RevivePopupController controller = FindObjectOfType<RevivePopupController>();
            
            if (SlotManager.Instance.Slots.Count + slotCount > Consts.MAX_SLOTS_PER_ROW) slotCount = Consts.MAX_SLOTS_PER_ROW - SlotManager.Instance.Slots.Count;

            for (int i = 0; i < slotCount; i++)
                SlotManager.Instance.SlotAds.AddSlot(false);

            if (controller != null)
            {
                controller.Revive();
            }
        }

        //
        if (isOneTimeBuy)
        {
            PlayerPrefs.SetInt($"buy_{packageId}", 1);
            PlayerPrefs.Save();
            isBought = true;
            animator.Play("IAPPackageVanish");
        }

        if (!isAds && PlayerPrefs.GetInt("ads_jelly", 1) == 1)
        {
            PlayerPrefs.SetInt("ads_jelly", 0);
            PlayerPrefs.Save();
        }
    }
}
