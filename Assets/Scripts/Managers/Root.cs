using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Root : MonoBehaviour
{
  [SerializeField] string mainMenu;
  [SerializeField] string inGameMenu;
  [SerializeField] string[] levels;
  [Space]
  [SerializeField] int livesPerRun;

  int cur_level;
  int balls_left_in_cur_level;
  int lives;
  enum GameState { NotStarted, InProgress}
  GameState game_state;

  private void Start()
  {
    game_state = GameState.NotStarted;
    EventBus.Instance.Register("start", OnStart);
    EventBus.Instance.Register("highscores", OnHighscore);
    EventBus.Instance.Register("quit", OnQuit);
    EventBus.Instance.Register("large-ball-start", (sender, args) => balls_left_in_cur_level += 4);
    EventBus.Instance.Register("small-ball-destroy", (sender, args) =>
    {
      balls_left_in_cur_level--;
      if (game_state == GameState.InProgress)
        if (balls_left_in_cur_level == 0)
          NextLevel();
    });
    EventBus.Instance.Register("game-over", OnBackToMainMenu);

    OnBackToMainMenu(null, null);
  }

  void OnBackToMainMenu(object sender, EventArgs args)
  {
    IEnumerator worker()
    {
      EventBus.Instance.Send("loading-main-menu");
      yield return UnloadAll();
      yield return SceneManager.LoadSceneAsync(mainMenu, LoadSceneMode.Additive);
    }
    game_state = GameState.NotStarted;
    StartCoroutine(worker());
  }

  void OnStart(object sender, EventArgs args)
  {
    lives = livesPerRun;
    balls_left_in_cur_level = 0;
    game_state = GameState.InProgress;
    EventBus.Instance.Send("game-start");
    StartCoroutine(MoveToLevel(0));
  }

  // unload all scenes except this one
  IEnumerator UnloadAll()
  {
    int active_count = SceneManager.sceneCount;
    for (int i = active_count - 1; i >= 0; i--)
    {
      var scn = SceneManager.GetSceneAt(i);
      if (scn != gameObject.scene)
        yield return SceneManager.UnloadSceneAsync(scn);
    }
  }

  IEnumerator MoveToLevel(int index)
  {
    if(game_state == GameState.NotStarted){
      Debug.LogError("You forgot to change the game state before calling MoveToLevel !!");
      yield break;
    }
    //todo: loading screen on
    balls_left_in_cur_level = 0;
    yield return UnloadAll();
    index = Mathf.Clamp(index, 0, levels.Length - 1);
    yield return SceneManager.LoadSceneAsync(levels[index], LoadSceneMode.Additive);
    yield return SceneManager.LoadSceneAsync(inGameMenu, LoadSceneMode.Additive);
    SceneManager.SetActiveScene(SceneManager.GetSceneByName(levels[index]));
    yield return null; // a frame for scenes to wake up
    EventBus.Instance.Send("level-start");
    //todo: loading screen off
    cur_level = index;
  }

  void NextLevel()
  {
    if (cur_level + 1 < levels.Length)
      StartCoroutine(MoveToLevel(cur_level + 1));
    else
      OnBackToMainMenu(null, null);
  }

  void OnHighscore(object sender, EventArgs args)
  {
    //TODO
  }

  void OnQuit(object sender, EventArgs args)
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
  }
}

