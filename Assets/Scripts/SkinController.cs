using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.CompilerServices;



/// <summary>
///  Controlleur du skin 
/// </summary>
public class SkinController : MonoBehaviour
{
    public static SkinController Instance { get; private set; }
    public SkinData[] skins;
    public Transform previewPosition;
    public TextMeshProUGUI skinName;
    public Button BuyButton;
    public Button SelectSkinButton;
    public TextMeshProUGUI SelectSkinButtonText;
    public TextMeshProUGUI PriceText;
    /// <summary>
    /// Wallet amount (number of coins)
    /// </summary>
    public TextMeshProUGUI CoinText;
    private GameObject currentPreview;
    public int currentIndex = 0;
    CoinManager coinManager;

    #region Default Methods (Awake, Start, Update)
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (PlayerPrefs.HasKey("SelectedSkin"))
            currentIndex = PlayerPrefs.GetInt("SelectedSkin");
    }

    private void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
        PreviewSkin(currentIndex);
        DisplaySkinName();
    }

    private void Update()
    {
        DisplaySkinName();
        DisplaySkinPrice();
        DisplayCoinScore();
        UpdateButtonsStatus();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextSkin();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousSkin();
    }
    #endregion

    #region Skin Selection

    public void NextSkin()
    {
        currentIndex++;
        if (currentIndex >= skins.Length)
            currentIndex = 0;
        PreviewSkin(currentIndex);
    }

    public void PreviousSkin()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = skins.Length - 1;
        PreviewSkin(currentIndex);
    }

    private void PreviewSkin(int index)
    {
        if (currentPreview != null)
            Destroy(currentPreview);
        currentPreview = Instantiate(skins[index].GetSkin(), previewPosition.position, Quaternion.identity, previewPosition);
        currentPreview.transform.localPosition = Vector3.zero;
        currentPreview.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void SelectSkin()
    {
        if (skins[currentIndex].IsOwned())
        {
            PlayerPrefs.SetInt("SelectedSkin", currentIndex);
            PlayerPrefs.Save();
        }
    }
    #endregion

    #region Information Display
    private void DisplaySkinName()
    {
        skinName.text = skins[currentIndex].GetSkin().name;
    }
    private void DisplaySkinPrice()
    {
        SkinData skin = skins[currentIndex];
        int price = skin.GetPrice();
        if (!skin.IsOwned())
        {
            PriceText.text = price > 0 ? "Price: " + price : "FREE";
        }
        else
        {
            PriceText.text = "OWNED";
        }
    }
    private void DisplayCoinScore()
    {
        CoinText.text = coinManager.GetCoinScore().ToString();
    }
    private void UpdateButtonsStatus()
    {
        SkinData skin = skins[currentIndex];
        bool isBuyable = !skin.IsOwned() && coinManager.CanBuy(skin.GetPrice());
        BuyButton.gameObject.SetActive(isBuyable);
        SelectSkinButton.gameObject.SetActive(skin.IsOwned());
        bool isCurrentSkinSelected = PlayerPrefs.GetInt("SelectedSkin") == currentIndex;
        SelectSkinButtonText.text = isCurrentSkinSelected ? "SELECTED" : "SELECT";
        SelectSkinButton.interactable = !isCurrentSkinSelected;
    }
    #endregion

    #region Skin Purchase
    public void BuySkin()
    {
        SkinData skin = skins[currentIndex];
        if (coinManager.Buy(skin.GetPrice()))
        {
            skin.Purchase();
        }
    }
    #endregion
}