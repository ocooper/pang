using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileTouch : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
  RectTransform rt;

  private void Start()
  {
    rt = transform as RectTransform;
  }
  public void OnPointerDown(PointerEventData eventData)
  {
    RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out var localpoint);
    InputSystem.Instance.horizontal_touch[0] = Mathf.Sign(localpoint.x);
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out var localpoint);
    InputSystem.Instance.horizontal_touch[0] = 0;
  }
}
