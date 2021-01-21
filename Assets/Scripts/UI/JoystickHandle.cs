using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Player player;
    [SerializeField] RectTransform joystickCenter;
    [SerializeField] float maxDistanceFromCenter;
    private Vector2 lastMousePosition;
    public bool isDragging = false;

    void Start()
    {

    }

    void Update()
    {
        if(!isDragging)
        {
            player.JoystickInput(Vector2.zero);
        }
        else
        {
            player.JoystickInput(inputVector);
        }
    }

    public Vector2 inputVector = Vector2.zero;
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        RectTransform rect = GetComponent<RectTransform>();

        Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
        Vector3 oldPos = rect.position;


        if(Vector3.Distance(joystickCenter.position, newPosition) < maxDistanceFromCenter)
        {
            rect.position = newPosition;
        }

        if (!IsRectTransformInsideSreen(rect))
        {
            rect.position = oldPos;
        }

        float inputX = (rect.position.x - joystickCenter.position.x) / maxDistanceFromCenter;
        float inputY = (rect.position.y - joystickCenter.position.y) / maxDistanceFromCenter;
        inputVector = new Vector2(inputX, inputY);

        Debug.Log($"x: {inputX}, " +
            $"y: {inputY}");

        lastMousePosition = currentMousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        RectTransform rect = GetComponent<RectTransform>();
        rect.position = joystickCenter.position;
    }
    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        foreach (Vector3 corner in corners)
        {
            if (rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
        if (visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }

    
}
