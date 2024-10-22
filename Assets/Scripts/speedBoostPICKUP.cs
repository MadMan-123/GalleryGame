using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpeedBoost Object", menuName = "Inventory System/Items/SpeedBoost")] //GM: this allows the developer to rightclick, go to the inventory system and go to itmes & default and create new items.
public class speedBoostPICKUP : itemObject
{
    public int increaseSpeedValue;
    public void Awake()
    {
        type = itemType.speedBoost;//GM: this allows us to not need to manually set an item type everytime we create a scriptable object
    }
}
