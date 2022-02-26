using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  [SerializeField] GameObject projectile;
  [Tooltip("Number of shots per seconds")]
  [SerializeField, Min(0.1f)] float rateOfFire;
  [SerializeField] int maxProjectileAtOnce = 1;
  [SerializeField] Transform launchPoint;
  [Tooltip("-1 is infinite")]
  [SerializeField] float ammo = -1;

  private float lastFireTime = float.MinValue;
  private int player_id;
  
  // I allow a player to have only N projectiles in flight at the same time.
  // That information has to be kept in a static variable, because the player can die, 
  // and then the Player and Weapon scripts are destroyed and their local members are gone.
  // So I keep it in a static dictionary, where each player gets his own list of projectiles
  private static IDictionary<int, List<GameObject>> tracked_projectiles;

  public void BindUniquePlayer(int player_id)
  {
    this.player_id = player_id;
    tracked_projectiles = tracked_projectiles ?? new SortedList<int, List<GameObject>>();
  }

  public void Fire()
  {
    if (!tracked_projectiles.TryGetValue(player_id, out var players_projectiles))
      tracked_projectiles[player_id] = players_projectiles = new List<GameObject>();
    players_projectiles.RemoveAll(x => x == null); // remove dead objects
    var delay = 1f / rateOfFire;
    if (Time.time - lastFireTime > delay &&
        (ammo == -1 || ammo > 0) &&
        (players_projectiles.Count < maxProjectileAtOnce))
    {
      lastFireTime = Time.time;
      if (ammo > 0)
        ammo--;
      var pr = Instantiate(projectile, launchPoint.position, Quaternion.identity);
      players_projectiles.Add(pr);
    }
  }
}
