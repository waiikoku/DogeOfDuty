using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIVirtualTouchJoy : UIVirtualTouchZoneBase
{
    [SerializeField] private RectTransform originPoint;
    [Header("Joy Settings")]
    public float joystickRange = 50f;
    public float sizeMultipier = 1;
    public UnityEvent<Vector2> joystickOutputEvent;
    public bool applySize;
    public override void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out pointerDownPosition);
        UpdateCircleRectPosition(pointerDownPosition);
        UpdateHandleRectPosition(Vector2.zero);
        originPoint.gameObject.SetActive(true);
        handleRect.gameObject.SetActive(true);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(originPoint, eventData.position, eventData.pressEventCamera, out currentPointerPosition);

        if(applySize) currentPointerPosition = ApplySizeDelta(currentPointerPosition);

        clampedPosition = ClampValuesToMagnitude(currentPointerPosition);

        outputPosition = ApplyInversionFilter(clampedPosition);

        OutputPointerEventValue(outputPosition * magnitudeMultiplier);

        if (handleRect)
        {
            UpdateHandleRectPosition(ApplyInversionFilter(clampedPosition) * joystickRange);
        }

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        originPoint.gameObject.SetActive(false);
    }

    Vector2 ApplySizeDelta(Vector2 position)
    {
        float x = (position.x / containerRect.sizeDelta.x) * sizeMultipier;
        float y = (position.y / containerRect.sizeDelta.y) * sizeMultipier;
        return new Vector2(x, y);
    }

    void UpdateCircleRectPosition(Vector2 newPosition)
    {
        originPoint.anchoredPosition = newPosition;
    }
}
