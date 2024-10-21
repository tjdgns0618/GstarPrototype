using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmithShop : MonoBehaviour
{
    public Inventory inventory;
    private InvenSlot selectedSlot;
    [SerializeField] private ShopItemDB shopitemDB;

    private void Start()
    {
        
    }

    public void SelectItemToUpgrade(InvenSlot item)
    {
        selectedSlot = item;
    }

    public void TryItemUpgrade()
    {
        if(selectedSlot != null && shopitemDB != null)
        {
            float successRate = 0;

            if (selectedSlot.itemCount <= 0)
                return;
                
            successRate = shopitemDB.entities3[selectedSlot.itemCount - 1].SuccessRate;

            if (successRate <= CreateRandomNumber())
                UpgradeSuccess();
            else
                UpgradeFail();
        }
    }

    public float CreateRandomNumber()
    {
        float rand = Random.Range(0.00f, 100.00f);

        return rand;
    }

    public void UpgradeFail()
    {

    }

    public void UpgradeSuccess()
    {
        inventory.EnchantItem(selectedSlot.item, 1);
    }
}
