using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class BackButtonController : MonoBehaviour, IInteractiveButton, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform backButtonRect;

    void Start()
    {
        backButtonRect = GetComponent<RectTransform>();
    }
    public void ButtonClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // scale up the button and change color of text to yellow
        backButtonRect.localScale = new Vector3(1.1f, 1.1f, 1);
        backButtonRect.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // scale back the button and change color of text to white
        backButtonRect.localScale = Vector3.one;
        backButtonRect.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

}
