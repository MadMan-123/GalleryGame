using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType//GM: types of items
{
    speedBoost,//GM: increases player's speed (movement, interactions with objects can be done at a later point if people think it's a good idea)
    equipment,//GM: Equipment type
    megaSLAP,//GM: increases slap power, so basically knocks the other player back a little further.
    Default
}
public abstract class itemObject : ScriptableObject //GM: scriptable object is the base class for creating items | the absctract class is an extender so we can set up other types of objects
{
    public GameObject prefab; // GM: this will hold the display for the item once added into inventory
    public itemType type; //GM: this will store the type of item
    [TextArea(15,20)]//GM: This allows you to read the type of item in the unity editor
    public string description;//GM: string for the description of the item

}
