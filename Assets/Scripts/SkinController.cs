using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    public GameObject[] skins;
    public Transform previewPosition;
    private GameObject currentPreview;
    private int currentIndex = 0;

    void Start()
    {
        PreviewSkin(currentIndex);
    }

    public void ChangeSkin(int change)
    {
        currentIndex += change;
        if (currentIndex >= skins.Length) currentIndex = 0;
        else if (currentIndex < 0) currentIndex = skins.Length - 1;

        PreviewSkin(currentIndex);
    }

    private void PreviewSkin(int index)
    {
        if (currentPreview != null) Destroy(currentPreview);
        currentPreview = Instantiate(skins[index], previewPosition.position, Quaternion.identity);
        currentPreview.transform.SetParent(previewPosition, false);
    }

    public void SelectSkin()
    {
        Player.Instance.ApplySkin(skins[currentIndex]);
    }

}
