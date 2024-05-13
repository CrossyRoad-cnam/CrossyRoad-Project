using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin Data", menuName = "Skin Data")]

public class SkinData : ScriptableObject
{
    [SerializeField] public GameObject skin;
    [SerializeField] public int price = 10;
    /// <summary>
    /// 0 = NOT OWNED, 1 = OWNED
    /// </summary>
    [SerializeField] public int owned = 0;

    private string skinOwnedKey { get { return "Skin_Owned_" + skin.name; } }

    private void Start()
    {
        LoadSkin();
    }

    private void LoadSkin()
    {
        if (PlayerPrefs.HasKey(skinOwnedKey))
        {
            owned = PlayerPrefs.GetInt(skinOwnedKey);
        }
        else
        {
            SaveSkin();
            LoadSkin();
        }
    }

    private void SaveSkin()
    {
        PlayerPrefs.SetInt(skinOwnedKey, owned);
    }

    internal GameObject GetSkin()
    {
        return skin;
    }

    internal int GetPrice()
    {
        return price;
    }

    internal void Purchase()
    {
        owned = 1;
        SaveSkin();
    }

    internal bool IsOwned()
    {
        return owned == 1;
    }
}
