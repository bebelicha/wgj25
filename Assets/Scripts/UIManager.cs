using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject telaInicialPanel;
    public GameObject roxaPanel;
    public GameObject rosaPanel;
    public GameObject painelMenuFixo;
    [Header("Roxa UI Elements")]
    public TextMeshProUGUI roxaNameText;
    public TextMeshProUGUI roxaFocoText;
    public TextMeshProUGUI roxaResilienciaText;
    public TextMeshProUGUI roxaAnaliseText;
    public Slider roxaEnergiaSlider;
    public TextMeshProUGUI roxaTurnText;
    public Image roxaSprite; 
    [Header("Rosa UI Elements")]
    public TextMeshProUGUI rosaNameText;
    public TextMeshProUGUI rosaExpressaoText;
    public TextMeshProUGUI rosaIniciativaText;
    public TextMeshProUGUI rosaIntuicaoText;
    public Slider rosaEnergiaSlider;
    public TextMeshProUGUI rosaTurnText;
    public Image rosaSprite;
    [Header("Overlay Panels")]
    public GameObject consequencePanel;
    public TextMeshProUGUI consequenceText;
    public Image consequenceCharacterImage; 
    public Button continueButton;
    public GameObject npcDialoguePanel;
    public TextMeshProUGUI npcDialogueText;
    public GameObject finalScenePanel;
    public LocalizedText finalNarrativeText;
    public GameObject creditsPanel;
    public LocalizedText creditsText;
    public float creditsScrollSpeed = 50f;
    [Header("Expression Sprites")]
    public Sprite roxaNeutra;
    public Sprite roxaFeliz;
    public Sprite roxaTriste;
    public Sprite rosaNeutra;
    public Sprite rosaFeliz;
    public Sprite rosaTriste;
    public bool hasContinued = false;
    public void ShowMainMenuPanel()
    {
        telaInicialPanel.SetActive(true);
        painelMenuFixo.SetActive(false);
        roxaPanel.SetActive(false);
        rosaPanel.SetActive(false);
        consequencePanel.SetActive(false);
        npcDialoguePanel.SetActive(false);
        finalScenePanel.SetActive(false);
        creditsPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }
    public void SetupPhase(GameManager.GamePhase phase, CharacterStats character)
    {
        painelMenuFixo.SetActive(true);
        creditsPanel.SetActive(false);
        consequencePanel.SetActive(false);
        finalScenePanel.SetActive(false);
        npcDialoguePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        roxaPanel.SetActive(phase == GameManager.GamePhase.RoxaTurn);
        rosaPanel.SetActive(phase == GameManager.GamePhase.RosaTurn);
        if (character.characterName == "Roxa") roxaSprite.sprite = roxaNeutra;
        else rosaSprite.sprite = rosaNeutra;
        UpdateUI(character, 1);
    }
    public void UpdateUI(CharacterStats character, int currentTurn)
    {
        if (character.characterName == "Roxa")
        {
            roxaNameText.text = LocalizationManager.Instance.GetLocalizedValue("char_roxa");
            roxaFocoText.text = character.foco.ToString();
            roxaResilienciaText.text = character.resiliencia.ToString();
            roxaAnaliseText.text = character.analise.ToString();
            roxaEnergiaSlider.value = character.energia / character.maxEnergia;
            roxaTurnText.text =  currentTurn + " / " + GameManager.Instance.maxTurnsPerCharacter;
        }
        else
        {
            rosaNameText.text = LocalizationManager.Instance.GetLocalizedValue("char_rosa");
            rosaExpressaoText.text = character.expressao.ToString();
            rosaIniciativaText.text = character.iniciativa.ToString();
            rosaIntuicaoText.text = character.intuicao.ToString();
            rosaEnergiaSlider.value = character.energia / character.maxEnergia;
            rosaTurnText.text = currentTurn + " / " + GameManager.Instance.maxTurnsPerCharacter;
        }
    }
    public IEnumerator ShowNpcDialogue(string dialogueKey, float duration)
    {
        Image activeSpriteImage = (GameManager.Instance.currentPhase == GameManager.GamePhase.RoxaTurn) ? roxaSprite : rosaSprite;
        Sprite sadSprite = (GameManager.Instance.currentPhase == GameManager.GamePhase.RoxaTurn) ? roxaTriste : rosaTriste;
        Sprite neutralSprite = (GameManager.Instance.currentPhase == GameManager.GamePhase.RoxaTurn) ? roxaNeutra : rosaNeutra;
        activeSpriteImage.sprite = sadSprite;
        npcDialoguePanel.SetActive(true);
        npcDialogueText.text = LocalizationManager.Instance.GetLocalizedValue(dialogueKey);
        yield return new WaitForSeconds(duration);
        npcDialoguePanel.SetActive(false);
        activeSpriteImage.sprite = neutralSprite;
    }
    public IEnumerator ShowNarrativeSequence(string[] textKeys, CharacterStats character = null, bool showFinalImage = false)
    {
        roxaPanel.SetActive(false);
        rosaPanel.SetActive(false);
        if (character != null)
        {
            consequenceCharacterImage.gameObject.SetActive(true);
        }
        else
        {
            consequenceCharacterImage.gameObject.SetActive(false);
        }
        if (showFinalImage)
        {
            finalScenePanel.SetActive(true);
        }
        else
        {
            consequencePanel.SetActive(true);
        }
        continueButton.gameObject.SetActive(true);
        foreach (string key in textKeys)
        {
            hasContinued = false;
            if (showFinalImage)
            {
                finalNarrativeText.localizationKey = key;
                finalNarrativeText.UpdateText();
            }
            else
            {
                consequenceText.text = LocalizationManager.Instance.GetLocalizedValue(key);
            }
            if (character != null)
            {
                Sprite spriteToShow = (character.characterName == "Roxa") ? roxaNeutra : rosaNeutra;
                if (key.Contains("positive"))
                {
                    spriteToShow = (character.characterName == "Roxa") ? roxaFeliz : rosaFeliz;
                }
                else if (key.Contains("negative") || key.Contains("shutdown") || key.Contains("meltdown"))
                {
                    spriteToShow = (character.characterName == "Roxa") ? roxaTriste : rosaTriste;
                }
                consequenceCharacterImage.sprite = spriteToShow;
            }
            yield return new WaitUntil(() => hasContinued);
        }
        consequencePanel.SetActive(false);
        finalScenePanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }
    public void OnContinueButtonPressed()
    {
        hasContinued = true;
    }
    public IEnumerator ShowCreditsSequence()
    {
        roxaPanel.SetActive(false);
        rosaPanel.SetActive(false);
        consequencePanel.SetActive(false);
        npcDialoguePanel.SetActive(false);
        finalScenePanel.SetActive(false);
        painelMenuFixo.SetActive(false);
        creditsPanel.SetActive(true);
        creditsText.localizationKey = "credits_full_text";
        creditsText.UpdateText();
         yield return null; 
        RectTransform creditsRect = creditsText.GetComponent<RectTransform>();
        float screenHeight = ((RectTransform)creditsPanel.transform).rect.height;
        float initialY = -screenHeight / 2 - creditsRect.rect.height / 2;
        creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, initialY);
        float endY = screenHeight / 2 + creditsRect.rect.height / 2;
        while (creditsRect.anchoredPosition.y < endY)
        {
            creditsRect.anchoredPosition += new Vector2(0, creditsScrollSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GoToMainMenu();
    }
}
