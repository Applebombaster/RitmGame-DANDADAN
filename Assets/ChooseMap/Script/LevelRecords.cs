using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SupportClass<T>
{
    public T[] Item;
    public SupportClass(T[] array) => Item = array;
}

[System.Serializable]
public class LevelRecords
{
    public string[] Keys;
    public SupportClass<int>[] Values;

    public LevelRecords() 
    {
        Keys = new string[0];
        Values = new SupportClass<int>[0];
    }

    public LevelRecords(Dictionary<string, int[]> dict)
    {
        if (dict == null)
        {
            Keys = new string[0];
            Values = new SupportClass<int>[0];
            return;
        }
        
        Keys = dict.Keys.ToArray();
        Values = dict.Values.Select(v => new SupportClass<int>(v)).ToArray();
    }

    public Dictionary<string, int[]> ToDictionarySafe()
    {
        var result = new Dictionary<string, int[]>();
        if (Keys == null || Values == null) 
            return result;
        
        int count = Mathf.Min(Keys.Length, Values.Length);
        for (int i = 0; i < count; i++)
        {
            if (Keys[i] != null && Values[i] != null)
            {
                result[Keys[i]] = Values[i].Item;
            }
        }
        return result;
    }
}