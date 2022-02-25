using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  public void OnPointerDown(PointerEventData eventData)
  {
    InputSystem.Instance.fire_touch[0] = true;
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    InputSystem.Instance.fire_touch[0] = false;
  }

}
