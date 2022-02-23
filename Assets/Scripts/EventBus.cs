using System;
using System.Collections.Generic;
using UnityEngine;

using Key = System.String;

class EventBus : MonoBehaviour
{
  static EventBus instance;
  public static EventBus Instance
  {
    get
    {
      return instance ?? (instance = new GameObject("Event bus").AddComponent<EventBus>());
    }
  }
  List<(Key, object, EventArgs)> pending = new List<(Key, object, EventArgs)>();

  List<(Key, object, EventArgs)> work = new List<(Key, object, EventArgs)>();
  Dictionary<Key, List<EventHandler>> db = new Dictionary<Key, List<EventHandler>>();
  
  public bool debugSend;
  public bool debugDispatch;

  private void Awake()
  {
    if (instance != null && instance != this)
    {
      Destroy(this);
      return;
    }
    instance = this;
  }

  public void Send(Key key, object sender = null, EventArgs args = null)
  {
    if (debugSend)
      print($"Queued event {key}");
    pending.Add((key, sender, args));
  }

  public void Register(Key key, EventHandler handler) {
    if (!db.TryGetValue(key, out var list))
      db[key] = list = new List<EventHandler>();
    list.Add(handler);
  }
  public void Remove(Key key, EventHandler handler) {
    if (db.TryGetValue(key, out var list))
      list.Remove(handler);
  }

  private void LateUpdate()
  {
    var temp = pending;
    pending = work;
    work = temp;

    foreach (var item in work)
    {
      if (db.TryGetValue(item.Item1, out var listeners))
      {
        if(debugDispatch)
          print($"Sending {item.Item1} to {listeners.Count} handlers");
        foreach (var listener in listeners)
        {
          try
          {
            listener.Invoke(item.Item2, item.Item3);
          }
          catch { }
        }
      }
    }
    work.Clear();
  }
}