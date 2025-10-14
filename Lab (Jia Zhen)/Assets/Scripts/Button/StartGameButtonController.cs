using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonController : MonoBehaviour, IInteractiveButton
{
    public void ButtonClick()
    {
        SceneManager.LoadScene("Loading");
    }
}
