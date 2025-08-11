using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string localizationKey;
    private TextMeshProUGUI textComponent;
    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }
    public void UpdateText()
    {
        if (textComponent != null && LocalizationManager.Instance != null)
        {
            textComponent.text = LocalizationManager.Instance.GetLocalizedValue(localizationKey);
        }
    }
}