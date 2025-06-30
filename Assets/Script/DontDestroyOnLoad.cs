using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
            SceneManager.sceneLoaded += OnSceneLoaded; // Tambahkan listener
        }
        else
        {
            Destroy(gameObject); // Hancurkan jika instance sudah ada
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HomeScreen")
        {
            Destroy(gameObject); // Hancurkan object jika scene adalah HomeScreen
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Lepaskan listener ketika object dihancurkan
    }
}
