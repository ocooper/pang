using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerHealthMsg : EventArgs
{
  public int player;
  public int lives_left;
}

public class PlayerLivesSystem : MonoBehaviour
{
  [SerializeField] int livesPerRun;
  [SerializeField] GameObject playerPrefab;
  int lives_left_player_1;
  int lives_left_player_2;
  GameObject spawn_point;

  void Start()
  {
    EventBus.Instance.Register("game-start", OnGameStart);
    EventBus.Instance.Register("level-start", OnLevelStart);
    EventBus.Instance.Register("player-1-died", OnPlayer1Died);
    EventBus.Instance.Register("player-2-died", OnPlayer2Died);
  }

  private void OnPlayer1Died(object sender, EventArgs e)
  {
    //TODO:
    if (lives_left_player_1 == 0)
      EventBus.Instance.Send("game-over");
    else
    {
      lives_left_player_1--;
      EventBus.Instance.Send("set-lives", null, new PlayerHealthMsg { player = 1, lives_left = lives_left_player_1 });
      Respawn();
    }
  }

  private void OnPlayer2Died(object sender, EventArgs e)
  {
    //TODO:
    if (lives_left_player_2 == 0)
      EventBus.Instance.Send("game-over");
    else
    {
      lives_left_player_2--;
      EventBus.Instance.Send("set-lives", null, new PlayerHealthMsg { player = 2, lives_left = lives_left_player_2 });
      Respawn();
    }
  }

  private void OnLevelStart(object sender, EventArgs e)
  {
    spawn_point = GameObject.Find("Spawnpoint");
    if (spawn_point == null)
      Debug.LogError("No spawn point set for level !!!");
    Respawn();
    EventBus.Instance.Send("set-lives", null, new PlayerHealthMsg { player = 1, lives_left = lives_left_player_1 });
    EventBus.Instance.Send("set-lives", null, new PlayerHealthMsg { player = 2, lives_left = lives_left_player_2 });
  }

  private void OnGameStart(object sender, EventArgs e)
  {
    lives_left_player_1 = livesPerRun;
    lives_left_player_2 = livesPerRun;
  }

  void Respawn()
  {
    if (spawn_point != null)
    {
      var new_player = Instantiate(playerPrefab);
      new_player.transform.position = spawn_point.transform.position;
    }
    //TODO: respawn at right place
  }
}
