using static UnityEditor.Progress;

[System.Serializable]
public class ShopItemDBEntity
{
    public int ItemID;
    public string ItemName;
    public string ItemInfo;
    public int Price;
    public int MaxLevel;
    public float AttackDamage;
    public float Deffence;
    public bool IsAuto;
    public float RecoveryThreshold;
    public int RecoveryCount;
    public float HP;
    public float HPRate;
    public float CriticalDamage;
    public float CriticalRate;
    public float DashCoolTime;
    public float ItemCoolTimeDropRate;
}


