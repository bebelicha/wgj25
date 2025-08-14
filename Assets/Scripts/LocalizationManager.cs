using UnityEngine;
using System.Collections.Generic;

public class LocalizationManager : MonoBehaviour
{
    public static event System.Action OnLanguageChanged;
    public static LocalizationManager Instance { get; private set; }
    public enum Language { Portuguese, English }
    public Language currentLanguage = Language.Portuguese;
    private Dictionary<string, string> localizedPT;
    private Dictionary<string, string> localizedEN;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ToggleLanguage()
    {
        currentLanguage = (currentLanguage == Language.Portuguese) ? Language.English : Language.Portuguese;
        LocalizedText[] allTexts = FindObjectsOfType<LocalizedText>();
        foreach (LocalizedText text in allTexts)
        {
            text.UpdateText();
        }
        OnLanguageChanged?.Invoke();
    }
    #region Dictionaries and GetValue
    public string GetLocalizedValue(string key)
    {
        if (currentLanguage == Language.Portuguese)
        {
            return localizedPT.ContainsKey(key) ? localizedPT[key] : "KEY_NOT_FOUND";
        }
        else
        {
            return localizedEN.ContainsKey(key) ? localizedEN[key] : "KEY_NOT_FOUND";
        }
    }

    void InitializeDictionaries()
    {
        localizedPT = new Dictionary<string, string>
        {
            {"char_roxa", "Jana"},
            {"char_rosa", "Carol"},
            {"menu_home", "Tela Inicial"},
            {"menu_language_toggle", "Idioma PT -> EN"},
            {"menu_restart", "Recomeçar"},
            {"ui_turno", "Turno"},
            {"ui_continue", "Continuar..."},
            {"jogar", "Jogar"},
            {"btn_ler_hiperfoco", "Ler sobre Conchas"},
            {"btn_desenhar_silencio", "Desenhar em Silêncio"},
            {"btn_ouvir_fones", "Ouvir com Fones"},
            {"btn_brincar", "Brincar"},
            {"btn_dancar_quarto", "Dançar"},
            {"btn_conversar_online", "Conversar Online"},
            {"attr_foco", "Foco"},
            {"attr_resiliencia", "Resiliência"},
            {"attr_analise", "Análise"},
            {"attr_expressao", "Expressão"},
            {"attr_iniciativa", "Iniciativa"},
            {"attr_intuicao", "Intuição"},
            {"intro_roxa_1", "É primeiro dia de aula, hora de socializar!"},
            {"intro_roxa_2a", "Foco: ajuda a filtrar os estímulos e manter a intenção."},
            {"intro_roxa_2b", "Resiliência: evita o desgaste rápido."},
            {"intro_roxa_2c", "Análise: identifica quem parece mais receptivo."},
            {"intro_roxa_2d", "Cuidado para não deixar a energia esgotar!"},
            {"pre_consequence_roxa", "É hora do recreio, vamos brincar!"},
            {"intro_rosa_1", "Carol já está no pátio, mas percebe risadinhas e cochichos enquanto tenta organizar uma brincadeira."},
            {"intro_rosa_2a", "Expressão: facilita iniciar conversas."},
            {"intro_rosa_2b", "Iniciativa: mantém a interação mesmo após recusas."},
            {"intro_rosa_2c", "Intuição: detecta quem está desconfortável."},
            {"intro_rosa_2d", "Cuidado para não deixar a energia esgotar!"},
            {"pre_consequence_rosa", "O sinal toca, a aula começa e a professora pergunta para os alunos sua matéria favorita."},
            {"final_narrative_1", "O sinal toca novamente, a aula terminou! Mas Carol vê Jana no corredor."},
            {"final_narrative_2", "Elas se encaram por um instante, sorriem. Brincam sem precisar explicar nada."},
            {"final_narrative_3", "Quando uma se incomoda com o barulho, a outra entende. Quando uma fala sem parar, a outra escuta."},
            {"final_narrative_4", "Pela primeira vez, não precisam se encaixar no azul."},
            {"credits_full_text", "Time YasCamisas\n\nCamila\n<size=80%>Graduanda em Sistemas de Informação na UFMG\nDesenvolvedora e Tradutora</size>\n\nIsabel\n<size=80%>Mestranda em Ciência Da Computação na UFMG\nIlustradora e Designer de Interfaces</size>\n\nIsabela\n<size=80%>Graduanda em Sistemas de Informação na UFMG\nDesenvolvedora e Design de Níveis</size>\n\nIsadora\n<size=80%>Graduanda em Sistemas de Informação na UFMG\nDesenvolvedora e Designer de Acessibilidade</size>\n\nYasmin\n<size=80%>Graduanda em Relações Públicas na UFMG\nDesign de Narrativa</size>"},
            {"npc_roxa_1", "“Não quero ser seu amigo, você é muito calada.”"},
            {"npc_roxa_2", "“Ei, não conversa com ela, eu já vi ela falando com fantasmas.”"},
            {"npc_roxa_3", "“Quer ser minha amiga?... Nossa, que estranha...”"},
            {"npc_rosa_1", "“Nossa, ela fala muito.”"},
            {"npc_rosa_2", "“Ela me irrita, não sei por quê.”"},
            {"npc_rosa_3", "“Não sou amiga dela, apenas finjo.”"},
            {"roxa_consequence_positive", "Jana analisa o fluxo de pessoas e traça uma rota segura. Ela chega à mesa cansada, mas segura."},
            {"roxa_consequence_neutral", "Jana usa seu Foco para criar um 'túnel', mas esbarra em alguém, chegando ao destino tensa e com o coração acelerado."},
            {"roxa_consequence_negative", "O barulho é demais. Jana congela no meio do caminho, incapaz de se mover."},
            {"roxa_consequence_shutdown", "A energia acabou. O sistema desligou. Jana para, imóvel, perdida em um mundo branco e silencioso."},
            {"rosa_consequence_positive", "Carol espera sua vez e explica com paixão sua resposta, contagiando a turma. Ela se sente ouvida."},
            {"rosa_consequence_neutral", "Carol não consegue esperar e grita a resposta, interrompendo a todos. A professora a repreende, e ela fica confusa."},
            {"rosa_consequence_negative", "As palavras não se formam. Um som frustrado sai, e ela bate na mesa, sobrecarregada."},
            {"rosa_consequence_meltdown", "A energia acabou. A frustração transborda em uma explosão de emoções que ela não consegue controlar."}
        };

        localizedEN = new Dictionary<string, string>
        {
            {"char_roxa", "Jana"},
            {"char_rosa", "Carol"},
            {"menu_home", "Home Screen"},
            {"menu_language_toggle", "Language EN -> PT"},
            {"menu_restart", "Restart"},
            {"ui_turno", "Turn"},
            {"ui_continue", "Continue..."},
            {"jogar", "Play"},
            {"btn_ler_hiperfoco", "Read about Shells"},
            {"btn_desenhar_silencio", "Draw in Silence"},
            {"btn_ouvir_fones", "Listen with Headphones"},
            {"btn_brincar", "Play"},
            {"btn_dancar_quarto", "Dance"},
            {"btn_conversar_online", "Chat Online"},
            {"attr_foco", "Focus"},
            {"attr_resiliencia", "Resilience"},
            {"attr_analise", "Analysis"},
            {"attr_expressao", "Expression"},
            {"attr_iniciativa", "Initiative"},
            {"attr_intuicao", "Intuition"},
            {"intro_roxa_1", "It's the first day of school, time to blend in!"},
            {"intro_roxa_2a", "Focus: helps to filter the stimulus and keep the intention."},
            {"intro_roxa_2b", "Resilience: avoids extreme fatigue."},
            {"intro_roxa_2c", "Analysis: Identify who seems more willing to engage.."},
            {"intro_roxa_2d", "Be careful not to let your energy run out!"},
            {"pre_consequence_roxa", "It's break time, let's play!"},
            {"intro_rosa_1", "Carol is already in the playground, but hears giggling and whispers as she organizes a game."},
            {"intro_rosa_2a", "Expression: helps to initiate conversation."},
            {"intro_rosa_2b", "Initiative: keeps interactions even after rejection."},
            {"intro_rosa_2c", "Intuition: detects if someone is uncomfortable."},
            {"intro_rosa_2d", "Be careful not to let your energy run out!"},
            {"pre_consequence_rosa", "The bell rings, class begins and the teacher asks the students their favorite subject."},
            {"final_narrative_1", "The bell rings again, class is over! Carol spots Jana in the hallway."},
            {"final_narrative_2", "They stare each other for a moment and smile. They play with no explanation needed."},
            {"final_narrative_3", "If one of them is bothered by some noise, the other understands. Whenever one is passionately talking, the other listens patiently."},
            {"final_narrative_4", "For the first time, blue doesn't have to be their color."},
            {"credits_full_text", "Team YasCamisas\n\nCamila\n<size=80%>Information Systems undergraduate at UFMG.\nTranslator and Developer</size>\n\nIsabel\n<size=80%>Computer Science Master’s student at UFMG.\nIllustrator and Interface Designer</size>\n\nIsabela\n<size=80%>Information Systems undergraduate at UFMG.\nDeveloper and Level Designer</size>\n\nIsadora\n<size=80%>Information Systems undergraduate at UFMG.\nDeveloper and Accessibility Designer</size>\n\nYasmin\n<size=80%>Public Relations undergraduate at UFMG\nNarrative Designer</size>"},
            {"npc_roxa_1", "“I don't want to be your friend, you're too quiet.”"},
            {"npc_roxa_2", "“Hey, don't talk to her. I've seen her talking to ghosts.”"},
            {"npc_roxa_3", "“Do you want to be my friend?... Geez, what a freak...”"},
            {"npc_rosa_1", "“Wow, she talks too much.”"},
            {"npc_rosa_2", "“I'm not sure why, but she gets on my nerves.”"},
            {"npc_rosa_3", "“I'm not really her friend. It's just make-believe.”"},
            {"roxa_consequence_positive", "Purple analyses the flow of people and plans a safe route. She gets to the table tired, but safe."},
            {"roxa_consequence_neutral", "Purple uses her Focus to create a 'tunnel', but bumps into someone. She gets to her destination safe, but her heart is racing."},
            {"roxa_consequence_negative", "The noise is overwhelming. Purple freezes in place, unable to move."},
            {"roxa_consequence_shutdown", "The energy is over. Total shutdown. Purple stops, still, lost in a white silent world."},
            {"rosa_consequence_positive", "Pink waits for her turn and passionately gives her answer, uplifting the class. She feels heard."},
            {"rosa_consequence_neutral", "Pink can't wait and shouts the answer, interrupting the class. The teacher scolds her, but she doesn't understand why."},
            {"rosa_consequence_negative", "Her mind is blank, no words are formed. She makes a frustrated sound and slams the table."},
            {"rosa_consequence_meltdown", "The energy is over. Frustration overflows in a explosion of emotions she can't control ."}
        };
    }
    #endregion
}
