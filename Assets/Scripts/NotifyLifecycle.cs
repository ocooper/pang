using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyLifecycle : MonoBehaviour
{
  [SerializeField] string eventOnStart;
  [SerializeField] string eventOnDestroy;
  private void Start()
  {
    if (!string.IsNullOrEmpty(eventOnStart))
      EventBus.Instance.Send(eventOnStart);
  }

  private void OnDestroy()
  {
    if (!string.IsNullOrEmpty(eventOnDestroy))
      EventBus.Instance.Send(eventOnDestroy);
  }
}
