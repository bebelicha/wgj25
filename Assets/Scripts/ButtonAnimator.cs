using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{
    [Header("Animation Frames")]
    [Tooltip("Os sprites para a animação em Português.")]
    public Sprite[] animationFramesPT;

    [Tooltip("Os sprites para a animação em Inglês.")]
    public Sprite[] animationFramesEN;

    [Header("Animation Settings")]
    [Tooltip("A velocidade da animação em frames por segundo.")]
    public float animationSpeed = 2.0f;

    private Image targetImage;
    private Sprite[] activeAnimationFrames; 
    private float timer;
    private int currentFrameIndex;

    void Awake()
    {
        targetImage = GetComponent<Image>();
        if (targetImage == null)
        {
            Debug.LogError("SpriteAnimator: Nenhum componente 'Image' encontrado neste GameObject. O script será desativado.", this.gameObject);
            this.enabled = false;
        }
    }
    void OnEnable()
    {
        
        LocalizationManager.OnLanguageChanged += UpdateLanguage;
        UpdateLanguage();
    }
    void OnDisable()
    {
        LocalizationManager.OnLanguageChanged -= UpdateLanguage;
    }
    void UpdateLanguage()
    {
        if (LocalizationManager.Instance == null) return;
        if (LocalizationManager.Instance.currentLanguage == LocalizationManager.Language.Portuguese)
        {
            activeAnimationFrames = animationFramesPT;
        }
        else
        {
            activeAnimationFrames = animationFramesEN;
        }
        timer = 0f;
        currentFrameIndex = 0;
        if (targetImage != null && activeAnimationFrames != null && activeAnimationFrames.Length > 0)
        {
            targetImage.sprite = activeAnimationFrames[0];
        }
    }
    void Update()
    {
        if (activeAnimationFrames == null || activeAnimationFrames.Length <= 1 || animationSpeed <= 0)
        {
            return;
        }
        timer += Time.deltaTime;
        float frameInterval = 1f / animationSpeed;

        if (timer >= frameInterval)
        {
            timer -= frameInterval;
            currentFrameIndex = (currentFrameIndex + 1) % activeAnimationFrames.Length;
            targetImage.sprite = activeAnimationFrames[currentFrameIndex];
        }
    }
}
