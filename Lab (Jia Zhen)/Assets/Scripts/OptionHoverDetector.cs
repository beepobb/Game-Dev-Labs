using UnityEngine;
using UnityEngine.EventSystems;

public class OptionHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject cursor;
    public void OnPointerEnter(PointerEventData eventData)
    {
        cursor.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursor.SetActive(false);
    }
}
