using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Video;

public class CutSceneManager : MonoBehaviour
{
    private Button buttonSkip;
    private float audioFadeDuration = 0.5f;

    [SerializeField] private GameObject blackFadeIn;
    [SerializeField] private VideoPlayer videoPlayer;

    // Sprites untuk kondisi normal, highlighted
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay(42f));
        buttonSkip = GameObject.Find("SkipButton").GetComponent<Button>();

        buttonSkip.image.sprite = normalSprite;

        if (buttonSkip != null)
        {
            buttonSkip.onClick.AddListener(ClickSkipButton);
            AddEventTrigger(buttonSkip.gameObject, EventTriggerType.PointerEnter, eventData => OnPointerEnter(eventData));
            AddEventTrigger(buttonSkip.gameObject, EventTriggerType.PointerExit, eventData => OnPointerExit((PointerEventData)eventData));
        }
    }

    private void ClickSkipButton()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        FadeOutMusic(audioFadeDuration);
        blackFadeIn.SetActive(true);
        StartCoroutine(LoadSceneAfterDelay(0.5f));
    }

    private IEnumerator LoadSceneAfterDelay(float delayTime)
    {   
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("InGame");
    }

    public void OnHighlightButton()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
        buttonSkip.image.sprite = highlightedSprite;
    }

    public void OnUnhighlightButton()
    {
        buttonSkip.image.sprite = normalSprite;
    }

    // highlight ---------------------------------------------------------------------------------------------

    // Implementasi IPointerEnterHandler untuk menangani event hover
    private void OnPointerEnter(BaseEventData eventData)
    {
        OnHighlightButton();
    }

    // Implementasi IPointerExitHandler untuk menangani saat pointer keluar dari button
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == buttonSkip.gameObject)
        {
            OnUnhighlightButton();
        }
    }

    // event trigger
    private void AddEventTrigger(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>() ?? obj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }


    // Audio ---------------------------------------------------------------------------------------------
    public void FadeOutMusic(float fadeDuration)
    {
        StartCoroutine(FadeOutCoroutine(fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(float fadeDuration)
    {
        float startVolume = (float)videoPlayer.GetDirectAudioVolume(0);

        while (videoPlayer.GetDirectAudioVolume(0) > 0)
        {
            float newVolume = videoPlayer.GetDirectAudioVolume(0) - startVolume * Time.deltaTime / fadeDuration;
            videoPlayer.SetDirectAudioVolume(0, newVolume);
            yield return null;
        }

        videoPlayer.SetDirectAudioVolume(0, 0); // Set volume ke 0 setelah fade out
        videoPlayer.Pause(); // Optional: menghentikan video setelah fade out
    }
}