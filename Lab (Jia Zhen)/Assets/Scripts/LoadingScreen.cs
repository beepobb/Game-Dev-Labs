using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;

    void Start()
    {
        StartCoroutine(LoadAsync("World-1-1"));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        float duration = 2f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);

            loadingBar.value = progress;
            yield return null;
        }

        operation.allowSceneActivation = true;

        yield return null;
    }
}