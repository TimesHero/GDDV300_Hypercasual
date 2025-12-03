using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkinButton : MonoBehaviour
{
    private void Awake()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            if (SkinManager.Instance != null)
            {
                button.onClick.AddListener(() => SkinManager.Instance.SelectSkinFromButton(button));
            }
        }
    }
}
