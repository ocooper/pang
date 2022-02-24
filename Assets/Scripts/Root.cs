using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Root : MonoBehaviour
{
  [SerializeField]
  string mainMenu;
  [SerializeField]
  string inGameMenu;
  [SerializeField]
  string[] levels;
  int cur_level;

  int balls_left;

  private void Start()
  {
    EventBus.Instance.Register("start", OnStart);
    EventBus.Instance.Register("highscores", OnHighscore);
    EventBus.Instance.Register("quit", OnQuit);
    EventBus.Instance.Register("end-level", OnBackToMainMenu);
    EventBus.Instance.Register("large-ball-start", (sender, args) => balls_left += 4);
    EventBus.Instance.Register("small-ball-destroy", (sender, args) =>
    {
      balls_left--;
      if (balls_left == 0)
        NextLevel();
    });

    OnBackToMainMenu(null, null);
  }

  void OnBackToMainMenu(object sender, EventArgs args)
  {
    IEnumerator worker()
    {
      yield return UnloadAll();
      yield return SceneManager.LoadSceneAsync(mainMenu, LoadSceneMode.Additive);
    }
    StartCoroutine(worker());
  }

  void OnStart(object sender, EventArgs args)
  {
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
    //todo: freeze animations, cover the screen
    balls_left = 0;
    yield return UnloadAll();
    index = Mathf.Clamp(index, 0, levels.Length - 1);
    yield return SceneManager.LoadSceneAsync(levels[index], LoadSceneMode.Additive);
    yield return SceneManager.LoadSceneAsync(inGameMenu, LoadSceneMode.Additive);
    SceneManager.SetActiveScene(SceneManager.GetSceneByName(levels[index]));
    EventBus.Instance.Send("level-start");
    //todo: uncover the screen, start animations
    cur_level = index;
  }

  void NextLevel()
  {
    if (cur_level+1 < levels.Length)
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

