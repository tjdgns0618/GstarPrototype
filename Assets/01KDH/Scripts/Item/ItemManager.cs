using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)] // �޴��� �� �߰�
// ������ Ÿ���� �����ص� ��ũ��Ʈ
public class ItemManager : ScriptableObject
{
    public enum ItemType
    {
        normal,
        rare
    }

    public string itemName; // ������ �̸�
    public Sprite itemImage; // ������ �̹���
    public ItemType itemType;

    [TextArea]
    public string itemEffect;
    [TextArea]
    public string itemInfo;
}
