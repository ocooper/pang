using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendEvent : MonoBehaviour
{
  [Serializable]
  struct EventByKey
  {
    public string evt;
    public KeyCode code;
  }
  [SerializeField]
  EventByKey[] custom;

  private void Update()
  {
    if (custom != null)
    {
      foreach (var c in custom)
      {
        if (Input.GetKeyDown(c.code))
          EventBus.Instance.Send(c.evt);
      }
    }
  }
  
  public void Send(string evt)
  {
    EventBus.Instance.Send(evt, this, null);
  }
}
