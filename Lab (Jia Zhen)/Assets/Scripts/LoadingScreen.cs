using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBarFill;
    public RectTransform marioIcon;
    public RectTransform barBackground;

    void Start()
    {
        Debug.Log("Mario position at start: " + marioIcon.anchoredPosition);
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

            loadingBarFill.value = progress;
            float startPosX = barBackground.rect.min.x;
            float endPosX = barBackground.rect.max.x;

            float barWidth = barBackground.rect.width;
            float yOffset = marioIcon.anchoredPosition.y;

            Vector2 startPos = new Vector2(0, yOffset);        // left edge
            Vector2 endPos = new Vector2(barWidth, yOffset);   // right edge

            marioIcon.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);

            // marioIcon.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);

            yield return null;
        }

        operation.allowSceneActivation = true;

        yield return null;
    }
}



// IEnumerator LoadAsync(string sceneName)
// {
//     AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
//     operation.allowSceneActivation = false;

//     while (!operation.isDone)
//     {
//         float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normalize 0–1
//         loadingBarFill.fillAmount = progress;

//         // Move Mario along the bar
// Vector3 startPos = barBackground.rect.min; // left edge
// Vector3 endPos = barBackground.rect.max;   // right edge
// float width = barBackground.rect.width;
// marioIcon.anchoredPosition = new Vector2(progress * width - width / 2f, marioIcon.anchoredPosition.y);

//         if (progress >= 1f)
//             operation.allowSceneActivation = true;

//         yield return null;
//     }
// }

