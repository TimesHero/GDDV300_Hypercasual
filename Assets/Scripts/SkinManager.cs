using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;
    public Sprite selectedSkin;

    private Button currentSelectedButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectSkinFromButton(Button button)
    {
        if (button == null) return;

        Image img = button.GetComponent<Image>();

        if (img != null)
        {
            selectedSkin = img.sprite;
            Debug.Log("Selected skin: " + img.sprite.name);
            if (currentSelectedButton != null && currentSelectedButton != button)
            {
                currentSelectedButton.interactable = true;
            }
            button.interactable = false;

            currentSelectedButton = button;
        }
    }
}
