using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] float hoverLift = 30f;
    [SerializeField] float hoverScale = 1.1f;
    [SerializeField] float moveSpeed = 10f;

    RectTransform rect;
    Vector2 basePos;
    Vector2 targetPos;

    Vector3 baseScale;
    Vector3 targetScale;

    private TextMeshProUGUI costText;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;

    void Awake()
    {
        Image img = GetComponentInChildren<Image>(true);
        rect = img.rectTransform;

        basePos = rect.anchoredPosition;
        targetPos = basePos;

        baseScale = rect.localScale;
        targetScale = baseScale;

        costText = GameObject.FindGameObjectWithTag("CostText").GetComponent<TextMeshProUGUI>();
        nameText = GameObject.FindGameObjectWithTag("NameText").GetComponent<TextMeshProUGUI>();
        descriptionText = GameObject.FindGameObjectWithTag("DescriptionText").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        rect.anchoredPosition = Vector2.Lerp(
            rect.anchoredPosition,
            targetPos,
            Time.deltaTime * moveSpeed
        );

        rect.localScale = Vector3.Lerp(
            rect.localScale,
            targetScale,
            Time.deltaTime * moveSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetPos = basePos + Vector2.up * hoverLift;
        targetScale = baseScale * hoverScale;

        Debug.Log(gameObject.GetComponent<CardData>().currentName + " is hover!");
        Debug.Log(gameObject.GetComponent<CardData>().currentDescription + " is hover!");

        costText.text = "Cost : " + gameObject.GetComponent<CardData>().cost.ToString();
        nameText.text = gameObject.GetComponent<CardData>().currentName.ToString();
        descriptionText.text = gameObject.GetComponent<CardData>().currentDescription;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetPos = basePos;
        targetScale = baseScale;

        costText.text = "";
        nameText.text = "";
        descriptionText.text = "";
    }
}