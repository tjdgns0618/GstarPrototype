using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)] // �޴��� �� �߰�
// ������ Ÿ���� �����ص� ��ũ��Ʈ
public class Item : ScriptableObject
{
    public enum ItemType
    {
        passive,
        active
    }
    public int itemID; // ������ ������ȣ(���� ������ ���� �ʿ��� ����)
    public string itemName; // ������ �̸�
    public Sprite itemImage; // ������ �̹���
    public ItemType itemType;

    [TextArea]
    public string itemEffect;
}
