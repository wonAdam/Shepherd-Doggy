using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDetection : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] CameraWork camWork;
    private Vector2 screenCenter;
    // Start is called before the first frame update
    void Start()
    {
        this.screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.touchCount >= 1)
        {
            lastMousePositionOneTouch = Input.GetTouch(0).position;
        }
        if(Input.touchCount == 2)
        {
            lastMousePositionTwoTouch = Input.GetTouch(1).position;
        }
    }

    public Vector2 lastMousePositionOneTouch;
    public Vector2 lastMousePositionTwoTouch;
    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount == 2)
        {
            Vector2 currentMousePositionOne = Input.GetTouch(0).position;
            Vector2 currentMousePositionTwo = Input.GetTouch(1).position;

            Vector2 lastPointOneFromCenter = lastMousePositionOneTouch - screenCenter;
            Vector2 lastPointTwoFromCenter = lastMousePositionTwoTouch - screenCenter;

            Vector2 currPointOneFromLastPoint = currentMousePositionOne - lastMousePositionOneTouch;
            Vector2 currPointTwoFromLastPoint = currentMousePositionTwo - lastMousePositionTwoTouch;

            float amount = Vector2.Dot(lastPointOneFromCenter, currPointOneFromLastPoint) + Vector2.Dot(lastPointTwoFromCenter, currPointTwoFromLastPoint);

            Debug.Log(amount);
            if (Mathf.Abs(amount) > 10000f)
            {
                camWork.CloseUp((-1f) * amount / 1000000f);
                lastMousePositionOneTouch = Input.GetTouch(0).position;
                lastMousePositionTwoTouch = Input.GetTouch(1).position;
            }
            

        }

        else if (Input.touchCount == 1)
        {
            Vector2 currentMousePositionOne = Input.GetTouch(0).position;
            Vector2 currPointFromLastPoint = currentMousePositionOne - lastMousePositionOneTouch;

            float h = currPointFromLastPoint.x;
            float v = currPointFromLastPoint.y;

            camWork.RotateCamera(h / 100f, v / 100f);

            Debug.Log("OnDrag touch 1 " + currPointFromLastPoint);
            Debug.Log("hv " + h + " " + v);

            lastMousePositionOneTouch = Input.GetTouch(0).position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    private void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag");

        if (Input.touchCount == 2)
        {
            Debug.Log("touch 2");
        }

        else if (Input.touchCount == 1)
        {
            Debug.Log("touch 1");
        }
    }
}
