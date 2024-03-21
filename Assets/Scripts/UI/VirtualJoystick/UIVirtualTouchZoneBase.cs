using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class UIVirtualTouchZoneBase : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    [Header("Rect References")]
    public RectTransform containerRect;
    public RectTransform handleRect;

    [Header("Settings")]
    public bool clampToMagnitude;
    public bool multiplyMagnitude;
    public float magnitudeClamp = 1;
    public float magnitudeMultiplier = 1f;
    public bool invertXOutputValue;
    public bool invertYOutputValue;

    //Stored Pointer Values
    protected Vector2 pointerDownPosition;
    [SerializeField] protected Vector2 currentPointerPosition;

    [Header("Output")]
    public UnityEvent<Vector2> touchZoneOutputEvent;

    void Start()
    {
        SetupHandle();
    }

    private void SetupHandle()
    {
        if (handleRect)
        {
            SetObjectActiveState(handleRect.gameObject, false);
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out pointerDownPosition);

        if (handleRect)
        {
            SetObjectActiveState(handleRect.gameObject, true);
            UpdateHandleRectPosition(pointerDownPosition);
        }
    }

    [Header("Drag")]
    [SerializeField] protected Vector2 positionDelta;
    [SerializeField] protected Vector2 clampedPosition;
    [SerializeField] protected Vector2 outputPosition;
    [SerializeField] private bool normalizeOutput;
    public virtual void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out currentPointerPosition);
        positionDelta = GetDeltaBetweenPositions(pointerDownPosition, currentPointerPosition);
        clampedPosition = ClampValuesToMagnitude(positionDelta);
        outputPosition = ApplyInversionFilter(clampedPosition);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        pointerDownPosition = Vector2.zero;
        currentPointerPosition = Vector2.zero;

        OutputPointerEventValue(Vector2.zero);

        if (handleRect)
        {
            SetObjectActiveState(handleRect.gameObject, false);
            UpdateHandleRectPosition(Vector2.zero);
        }
    }

    protected void OutputPointerEventValue(Vector2 pointerPosition)
    {
        if (normalizeOutput)
        {
            touchZoneOutputEvent.Invoke(pointerPosition.normalized);
        }
        else
        {
            touchZoneOutputEvent.Invoke(pointerPosition);
        }

    }

    protected void UpdateHandleRectPosition(Vector2 newPosition)
    {
        handleRect.anchoredPosition = newPosition;
    }

    void SetObjectActiveState(GameObject targetObject, bool newState)
    {
        targetObject.SetActive(newState);
    }

    #region Function
    protected Vector2 GetDeltaBetweenPositions(Vector2 firstPosition, Vector2 secondPosition)
    {
        return secondPosition - firstPosition;
    }

    protected Vector2 ClampValuesToMagnitude(Vector2 position)
    {
        if (clampToMagnitude)
        {
            return Vector2.ClampMagnitude(position, magnitudeClamp);
        }
        else
        {
            return position;
        }
    }

    protected Vector2 ApplyInversionFilter(Vector2 position)
    {
        if (invertXOutputValue)
        {
            position.x = InvertValue(position.x);
        }

        if (invertYOutputValue)
        {
            position.y = InvertValue(position.y);
        }

        return position;
    }

    float InvertValue(float value)
    {
        return -value;
    }
    #endregion
}
