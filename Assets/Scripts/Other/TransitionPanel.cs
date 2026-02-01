using System.Collections;
using UnityEngine;

public class TransitionPanel : MonoBehaviour
{
    [Header("Curtain Objects")]
    [SerializeField] RectTransform curtainLeft;
    [SerializeField] RectTransform curtainRight;

    [Header("Position Points")]
    [SerializeField] RectTransform curtainLeftPointA;
    [SerializeField] RectTransform curtainLeftPointB;
    [SerializeField] RectTransform curtainRightPointA;
    [SerializeField] RectTransform curtainRightPointB;

    [Header("Parameters")]
    [SerializeField] float transitionSpeed = 1f;

    [Header("Variables")]
    public bool canTransition;

    bool isClosed;
    bool isTransitioning;

    private void Update()
    {
        if (!canTransition) return;

        PlayTransition();

        canTransition = false;
    }

    public void PlayTransition()
    {
        if (isTransitioning) return;

        StartCoroutine(TransitionRoutine());
    }

    IEnumerator TransitionRoutine()
    {
        isTransitioning = true;

        Vector2 leftFrom = isClosed ? curtainLeftPointB.anchoredPosition : curtainLeftPointA.anchoredPosition;
        Vector2 leftTo = isClosed ? curtainLeftPointA.anchoredPosition : curtainLeftPointB.anchoredPosition;

        Vector2 rightFrom = isClosed ? curtainRightPointB.anchoredPosition : curtainRightPointA.anchoredPosition;
        Vector2 rightTo = isClosed ? curtainRightPointA.anchoredPosition : curtainRightPointB.anchoredPosition;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / transitionSpeed;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            curtainLeft.anchoredPosition = Vector2.Lerp(leftFrom, leftTo, smoothT);
            curtainRight.anchoredPosition = Vector2.Lerp(rightFrom, rightTo, smoothT);

            yield return null;
        }

        curtainLeft.anchoredPosition = leftTo;
        curtainRight.anchoredPosition = rightTo;

        isClosed = !isClosed;
        isTransitioning = false;
    }
}
