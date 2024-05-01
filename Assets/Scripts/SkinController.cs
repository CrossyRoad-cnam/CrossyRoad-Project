using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SkinController : MonoBehaviour
{
    public GameObject[] skins;
    public Transform previewPosition;
    public TextMeshProUGUI skinName;
    private GameObject currentPreview;
    private int currentIndex = 0;

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
        if (Input.GetKeyDown(KeyCode.Space))
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
        currentPreview.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void SelectSkin()
    {
        Player.Instance.ApplySkin(skins[currentIndex]);
    }

    private void DisplaySkinName()
    {
        skinName.text = skins[currentIndex].name;
    }
}
