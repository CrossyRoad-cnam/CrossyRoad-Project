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

    private string skinPriceKey { get { return "Skin_Price_" + skin.name; } }
    private string skinOwnedKey { get { return "Skin_Owned_" + skin.name; } }

    private void Start()
    {
        LoadSkin();
    }

    private void LoadSkin()
    {
        if (PlayerPrefs.HasKey(skinPriceKey) || PlayerPrefs.HasKey(skinOwnedKey))
        {
            price = PlayerPrefs.GetInt(skinPriceKey);
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
        PlayerPrefs.SetInt(skinPriceKey, price);
        PlayerPrefs.SetInt(skinOwnedKey, owned);
    }

    internal GameObject GetSkin()
    {
        return skin;
    }
}
