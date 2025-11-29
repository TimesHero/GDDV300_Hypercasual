using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;
    public Sprite selectedSkin;

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
        Image img = button.GetComponent<Image>();

        if (img != null)
        {
            selectedSkin = img.sprite;
            Debug.Log("Selected skin: " + img.sprite.name);
        }
        else
        {
            Debug.LogError("Button has no Image component!");
        }
    }
}
