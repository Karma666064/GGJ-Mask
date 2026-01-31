using UnityEngine;
using UnityEngine.UI;

public class ArcLayoutGroup : LayoutGroup
{
    [SerializeField] float radius = 300f;
    [SerializeField] float arcAngle = 40f;

    [SerializeField] float offsetY;

    public override void CalculateLayoutInputVertical() {}

    public override void SetLayoutHorizontal()
    {
        SetCards();
    }

    public override void SetLayoutVertical()
    {
        SetCards();
    }

    void SetCards()
    {
        int count = rectChildren.Count;
        if (count == 0) return;

        float step = count > 1 ? arcAngle / (count - 1) : 0f;
        float startAngle = -arcAngle / 2f;

        for (int i = 0; i < count; i++)
        {
            RectTransform card = rectChildren[i];

            float angle = startAngle + step * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 pos = new Vector2(
                Mathf.Sin(rad) * radius,
                Mathf.Cos(rad) * radius - radius
            );

            SetChildAlongAxis(card, 0, pos.x - (card.rect.width / 2));
            SetChildAlongAxis(card, 1, -pos.y);

            card.localRotation = Quaternion.Euler(0, 0, -angle);
        }
    }
}
