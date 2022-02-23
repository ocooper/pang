using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  [SerializeField] GameObject projectile;
  [Tooltip("Number of shots per seconds")]
  [SerializeField] float rateOfFire;
  [SerializeField] Transform launchPoint;
  [Tooltip("-1 is infinite")]
  [SerializeField] float ammo = -1;

  private float lastFireTime = float.MinValue;

  public void Fire()
  {
    var delay = 1f / rateOfFire;
    if (Time.time - lastFireTime > delay && (ammo == -1 || ammo > 0))
    {
      lastFireTime = Time.time;
      if (ammo>0)
        ammo--;
      var pr = Instantiate(projectile, launchPoint.position, Quaternion.identity);
    }
  }
}
