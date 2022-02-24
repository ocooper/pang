using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
  [SerializeField, Min(1)] int health;
  [SerializeField, Min(0)] float invincibleTimeAfterSpawn = 0;
  [SerializeField] string eventOnDeath;

  int cur_health;
  float allow_hits_on;

  void Start()
  {
    cur_health = health;
    allow_hits_on = Time.time + invincibleTimeAfterSpawn;
  }

  public void TakeDamage()
  {
    if (Time.time > allow_hits_on)
    {
      cur_health--;
      if (cur_health == 0)
      {
        if (!string.IsNullOrEmpty(eventOnDeath))
          EventBus.Instance.Send(eventOnDeath);
        Destroy(gameObject);
      }
    }
  }

}
