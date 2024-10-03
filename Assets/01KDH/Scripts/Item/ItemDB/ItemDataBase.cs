using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[System.Serializable]
public struct ItemSheetDB
{
    public int itemID_index;
    public string itemText_index;
    public float itemVariable_index;
    public float itemVariable2_index;
    public float itemVariable3_index;
    public float itemVariable4_index;
}

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;

    [SerializeField]
    private ItemSheetDB[] _itemSheets;

    public ItemDB _itemDB;

    public InvenSlot[] _slots;

    public int _itemCount;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        _itemSheets = new ItemSheetDB[_itemDB.Sheet1.Count];

        for (int i = 0; i < _itemDB.Sheet1.Count; ++i)
        {
            _itemSheets[i].itemID_index = _itemDB.Sheet1[i].itemID;
            _itemSheets[i].itemText_index = _itemDB.Sheet1[i].itemText;
            _itemSheets[i].itemVariable_index = _itemDB.Sheet1[i].itemVariable;
            _itemSheets[i].itemVariable2_index = _itemDB.Sheet1[i].itemVariable2;
            _itemSheets[i].itemVariable3_index = _itemDB.Sheet1[i].itemVariable3;
            _itemSheets[i].itemVariable4_index = _itemDB.Sheet1[i].itemVariable4;
        }
    }

    public ItemSheetDB? GetItem(int itemID)
    {
        foreach (var item in _itemSheets)
        {
            if (item.itemID_index == itemID)
                return item;
        }
        return null;
    }

    public float Variable(int itemid)
    {
        return _itemDB.Sheet1[itemid].itemVariable;
    }
    public float Variable2(int itemid)
    {
        return _itemDB.Sheet1[itemid].itemVariable2;
    }
    public float Variable3(int itemid)
    {
        return _itemDB.Sheet1[itemid].itemVariable3;
    }
    public float Variable4(int itemid)
    {
        return _itemDB.Sheet1[itemid].itemVariable4;
    }
}
