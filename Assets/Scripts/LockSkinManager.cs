using TMPro;
using UnityEngine;

public class LockSkinManager : MonoBehaviour
{
    public TextMeshProUGUI softCurrencyText;
    public int[] currencyNeeded; 
    public GameObject[] skinButtons;

    void Start()
    {
        RefreshCurrencyUI();
        LoadUnlockedSkins();
    }

    public void UnlockSkin(int skinIndex)
    {
        int softCurrency = PlayerPrefs.GetInt("SoftCurrency", 0);
        int cost = currencyNeeded[skinIndex];

        if (PlayerPrefs.GetInt("SkinUnlocked_" + skinIndex, 0) == 1)
        {
            skinButtons[skinIndex].SetActive(true);
            return;
        }

        // Not enough money
        if (softCurrency < cost)
        {
            Debug.Log("Not enough soft currency!");
            return;
        }

        softCurrency -= cost;
        PlayerPrefs.SetInt("SoftCurrency", softCurrency);
        PlayerPrefs.SetInt("SkinUnlocked_" + skinIndex, 1);
        skinButtons[skinIndex].SetActive(true);

        RefreshCurrencyUI();

        Debug.Log("Unlocked skin " + skinIndex);
    }

    private void RefreshCurrencyUI()
    {
        softCurrencyText.text = "Fireflies: "+ PlayerPrefs.GetInt("SoftCurrency", 0).ToString();
    }

    private void LoadUnlockedSkins()
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            bool unlocked = PlayerPrefs.GetInt("SkinUnlocked_" + i, 0) == 1;

            skinButtons[i].SetActive(unlocked);
        }
    }
}
