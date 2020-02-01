using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public ItemType Type;
    public string ItemName;
}

public enum ItemType
{
    Wheel,
    Frame,
    Seat,
    Handlebars
}
