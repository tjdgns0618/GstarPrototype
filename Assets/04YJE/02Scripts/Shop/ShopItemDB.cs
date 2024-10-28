using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ShopItemDB : ScriptableObject
{
	public List<ShopItemDBEntity> entities; // Replace 'EntityType' to an actual type that is serializable.
	public List<ShopItemDBEntity2> entities3; // Replace 'EntityType' to an actual type that is serializable.
}
