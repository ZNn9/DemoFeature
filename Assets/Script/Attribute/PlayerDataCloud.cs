using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataCloud
{
    private string name;
    private long coin;
    private int level;
    private int experience;
    private Vector3 position;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public long Coin
    {
        get { return coin; }
        set { coin = value; }
    }
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    public int Experience
    {
        get { return experience; }
        set { experience = value; }
    }

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
}