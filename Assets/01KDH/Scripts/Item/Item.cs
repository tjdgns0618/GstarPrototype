using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)] // 메뉴에 탭 추가
// 아이템 타입을 정리해둔 스크립트
public class Item : ScriptableObject
{
    public enum ItemType
    {
        passive,
        active
    }
    public int itemID; // 아이템 고유번호(랜덤 생성을 위해 필요한 변수)
    public string itemName; // 아이템 이름
    public Sprite itemImage; // 아이템 이미지
    public ItemType itemType;

    [TextArea]
    public string itemEffect;
}
