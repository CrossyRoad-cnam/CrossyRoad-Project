using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SkinController : MonoBehaviour
{
    public static SkinController Instance { get; private set; }
    public GameObject[] skins;
    public Transform previewPosition;
    public TextMeshProUGUI skinName;
    private GameObject currentPreview;
    public int currentIndex = 0;

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
        PreviewSkin(currentIndex);
        DisplaySkinName();
    }
    private void Update()
    {
        DisplaySkinName();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextSkin();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousSkin();

        SelectSkin();
    }

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
        currentPreview = Instantiate(skins[index], previewPosition.position, Quaternion.identity, previewPosition);
        currentPreview.transform.localPosition = Vector3.zero;
        currentPreview.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void SelectSkin()
    {
        PlayerPrefs.SetInt("SelectedSkin", currentIndex);
        PlayerPrefs.Save();
    }

    private void DisplaySkinName()
    {
        skinName.text = skins[currentIndex].name;
    }
}