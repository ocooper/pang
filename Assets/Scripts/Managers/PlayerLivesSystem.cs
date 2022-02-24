using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerHealthMsg : EventArgs
{
  public int lives_left;
}

public class PlayerLivesSystem : MonoBehaviour
{
  [SerializeField] int livesPerRun;
  [SerializeField] GameObject playerPrefab;
  int lives_left;
  GameObject spawn_point;

  void Start()
  {
    EventBus.Instance.Register("game-start", OnGameStart);
    EventBus.Instance.Register("level-start", OnLevelStart);
    EventBus.Instance.Register("player-died", OnPlayerDied);
  }

  private void OnPlayerDied(object sender, EventArgs e)
  {
    if (lives_left == 0)
      EventBus.Instance.Send("game-over");
    else
    {
      lives_left--;
      EventBus.Instance.Send("set-lives", null, new PlayerHealthMsg { lives_left = lives_left });
      Respawn();
    }
  }

  private void OnLevelStart(object sender, EventArgs e)
  {
    spawn_point = GameObject.Find("Spawnpoint");
    if (spawn_point == null)
      Debug.LogError("No spawn point set for level !!!");
    Respawn();
    EventBus.Instance.Send("set-lives", null, new PlayerHealthMsg { lives_left = lives_left });
  }

  private void OnGameStart(object sender, EventArgs e)
  {
    lives_left = livesPerRun;
  }

  void Respawn()
  {
    if (spawn_point != null)
    {
      var new_player = Instantiate(playerPrefab);
      new_player.transform.position = spawn_point.transform.position;
    }
  }
}
