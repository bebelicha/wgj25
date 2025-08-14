using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GamePhase { MainMenu, Intro, RoxaTurn, RosaTurn, RoxaConsequence, RosaConsequence, FinalScene }
    public GamePhase currentPhase;
    public int currentTurn;
    public int maxTurnsPerCharacter = 7;
    public CharacterStats roxaStats;
    public CharacterStats rosaStats;
    public UIManager uiManager;
    [Tooltip("Chance de 0.0 a 1.0 para um evento de NPC ocorrer a cada turno.")]
    public float npcEventChance = 0.4f;

    [Tooltip("Duração (s) que o diálogo do NPC fica visível.")]
    public float npcDialogueDuration = 3f;
    private List<string> availableRoxaDialogues;
    private List<string> availableRosaDialogues;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        GoToMainMenu();
    }
    public void GoToMainMenu()
    {
        StopAllCoroutines();
        currentPhase = GamePhase.MainMenu;
        if (uiManager != null)
        {
            uiManager.ShowMainMenuPanel();
        }
    }
    public void StartGame()
    {
        StopAllCoroutines(); 
        StartCoroutine(StartGameCoroutine());
    }
    IEnumerator StartGameCoroutine()
    {
        uiManager.telaInicialPanel.SetActive(false);
        uiManager.painelMenuFixo.SetActive(true);
        currentPhase = GamePhase.Intro;
        roxaStats.InitializeStats(15, 15, 15, 100f);
        rosaStats.InitializeStats(15, 15, 15, 100f);
        ResetAvailableDialogues();
        currentTurn = 1;
        string[] roxaIntroKeys = { "intro_roxa_1", "intro_roxa_2a", "intro_roxa_2b", "intro_roxa_2c", "intro_roxa_2d" };
        yield return StartCoroutine(uiManager.ShowNarrativeSequence(roxaIntroKeys));
        currentPhase = GamePhase.RoxaTurn;
        if(uiManager != null) uiManager.SetupPhase(currentPhase, roxaStats);
    }

    void ResetAvailableDialogues()
    {
        availableRoxaDialogues = new List<string> { "npc_roxa_1", "npc_roxa_2", "npc_roxa_3" };
        availableRosaDialogues = new List<string> { "npc_rosa_1", "npc_rosa_2", "npc_rosa_3" };
    }
    public void OnActivitySelected(string activityName)
    {
        StartCoroutine(SelectActivityCoroutine(activityName));
    }
    IEnumerator SelectActivityCoroutine(string activityName)
    {
        CharacterStats activeCharacter = (currentPhase == GamePhase.RoxaTurn) ? roxaStats : rosaStats;
        ApplyActivityEffects(activityName, activeCharacter);
        uiManager.UpdateUI(activeCharacter, currentTurn);
        yield return new WaitForSeconds(0.5f);
        if (Random.Range(0f, 1f) <= npcEventChance)
        {
            yield return StartCoroutine(TriggerNpcEvent(activeCharacter));
        }
        if (activeCharacter.energia <= 0)
        {
            activeCharacter.energia = 0;
            EndTurnBlock();
        }
        else
        {
            NextTurn();
        }
    }
    void ApplyActivityEffects(string activityName, CharacterStats activeCharacter)
    {
        switch (activityName)
        {
            case "LerHiperfoco":
                activeCharacter.foco += 20; activeCharacter.analise += 10;
                activeCharacter.ChangeEnergy(-15);
                break;
            case "DesenharSilencio":
                activeCharacter.analise += 15; activeCharacter.resiliencia += 10;
                activeCharacter.ChangeEnergy(-20);
                break;
            case "OuvirFones":
                activeCharacter.foco += 5;
                activeCharacter.ChangeEnergy(35);
                break;
            case "Brincar":
                activeCharacter.iniciativa += 20; activeCharacter.intuicao += 5;
                activeCharacter.ChangeEnergy(-30);
                break;
            case "DancarQuarto":
                activeCharacter.expressao += 20;
                activeCharacter.ChangeEnergy(-15);
                break;
            case "ConversarOnline":
                activeCharacter.intuicao += 15;
                activeCharacter.ChangeEnergy(35);
                break;
        }
    }
    IEnumerator TriggerNpcEvent(CharacterStats character)
    {
        List<string> targetList = (character.characterName == "Roxa") ? availableRoxaDialogues : availableRosaDialogues;
        if (targetList.Count == 0) yield break;
        int randomIndex = Random.Range(0, targetList.Count);
        string dialogueKey = targetList[randomIndex];
        targetList.RemoveAt(randomIndex);
        character.ChangeEnergy(-10);
        character.fadiga += 5;
        yield return StartCoroutine(uiManager.ShowNpcDialogue(dialogueKey, npcDialogueDuration));
        uiManager.UpdateUI(character, currentTurn);
    }
    void NextTurn()
    {
        currentTurn++;
        if (currentTurn > maxTurnsPerCharacter) EndTurnBlock();
        else uiManager.UpdateUI((currentPhase == GamePhase.RoxaTurn) ? roxaStats : rosaStats, currentTurn);
    }
    void EndTurnBlock()
    {
        if (currentPhase == GamePhase.RoxaTurn)
        {
            currentPhase = GamePhase.RoxaConsequence;
            StartCoroutine(ShowConsequence(roxaStats));
        }
        else if (currentPhase == GamePhase.RosaTurn)
        {
            currentPhase = GamePhase.RosaConsequence;
            StartCoroutine(ShowConsequence(rosaStats));
        }
    }
    IEnumerator ShowConsequence(CharacterStats character)
    {
        string resultTextKey;
        float fadigaGained;
        string preConsequenceKey;
        if (character == roxaStats)
        {
            preConsequenceKey = "pre_consequence_roxa";
            if (character.energia <= 0) { resultTextKey = "roxa_consequence_shutdown"; fadigaGained = 30; }
            else if (character.analise > 55 && character.resiliencia > 45) { resultTextKey = "roxa_consequence_positive"; fadigaGained = 5; }
            else if (character.foco > 65 && character.analise < 35) { resultTextKey = "roxa_consequence_neutral"; fadigaGained = 15; }
            else { resultTextKey = "roxa_consequence_negative"; fadigaGained = 25; }
        }
        else 
        {
            preConsequenceKey = "pre_consequence_rosa";
            if (character.energia <= 0) { resultTextKey = "rosa_consequence_meltdown"; fadigaGained = 30; }
            else if (character.expressao > 65 && character.intuicao > 45) { resultTextKey = "rosa_consequence_positive"; fadigaGained = 5; }
            else if (character.iniciativa > 70 && character.expressao < 35) { resultTextKey = "rosa_consequence_neutral"; fadigaGained = 15; }
            else { resultTextKey = "rosa_consequence_negative"; fadigaGained = 25; }
        }
        character.fadiga += fadigaGained;
        string[] consequenceNarrativeKeys = { preConsequenceKey, resultTextKey };
        yield return StartCoroutine(uiManager.ShowNarrativeSequence(consequenceNarrativeKeys, character));
        if (currentPhase == GamePhase.RoxaConsequence)
        {
            StartCoroutine(StartRosaPhaseCoroutine());
        }
        else if (currentPhase == GamePhase.RosaConsequence)
        {
            currentPhase = GamePhase.FinalScene;
            string[] finalNarrativeKeys = { "final_narrative_1", "final_narrative_2", "final_narrative_3", "final_narrative_4" };
            yield return StartCoroutine(uiManager.ShowNarrativeSequence(finalNarrativeKeys, null, true));
            StartCoroutine(uiManager.ShowCreditsSequence());
        }
    }
    IEnumerator StartRosaPhaseCoroutine()
    {
        currentPhase = GamePhase.Intro; 
        currentTurn = 1;
        string[] rosaIntroKeys = { "intro_rosa_1", "intro_rosa_2a", "intro_rosa_2b", "intro_rosa_2c", "intro_rosa_2d" };
        yield return StartCoroutine(uiManager.ShowNarrativeSequence(rosaIntroKeys));
        currentPhase = GamePhase.RosaTurn;
        uiManager.SetupPhase(currentPhase, rosaStats);
    }
}
