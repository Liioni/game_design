using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelAsync(levelName));
    }

    IEnumerator LoadLevelAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            progressBar.value = progress;

            yield return null;
        }
    }
}