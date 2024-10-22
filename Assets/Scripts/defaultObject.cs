using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")] //GM: this allows the developer to rightclick, go to the inventory system and go to itmes & default and create new items.
public class DefaultObject : itemObject //GM extension of the itemObject class
{
    public void Awake() //GM: this lets us set variables everytime a new default object.
    {
        type = itemType.Default;//GM: this allows us to not need to manually set an item type everytime we create a scriptable object
    }
}
