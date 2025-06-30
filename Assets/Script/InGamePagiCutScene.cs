using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class InGamePagiCutScene : MonoBehaviour
{

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float delayTime;

    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay(delayTime));
    }

    private IEnumerator LoadSceneAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("InGamePagi");
    }
}
