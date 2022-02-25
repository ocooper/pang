using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
  const int NUM_PLAYERS = 2;
  static InputSystem instance;
  public static InputSystem Instance => (instance ?? (instance = new GameObject("Input system").AddComponent<InputSystem>()));

  [Serializable]
  struct InputAxisData
  {
    public string horizontal;
    public string fireButton;
  }

  [Header("Keyboard controls")]
  [SerializeField]
  InputAxisData[] players = new InputAxisData[NUM_PLAYERS] {
      new InputAxisData{
          horizontal = "Horizontal",
          fireButton = "Fire1"
      },
      new InputAxisData{
          horizontal = "Horizontal2",
          fireButton = "Fire2"
      }
  };

  [HideInInspector] float[] horizontal_keyboard = new float[NUM_PLAYERS];
  [HideInInspector] public float[] horizontal_touch = new float[NUM_PLAYERS];
  [HideInInspector] bool[] fire_keyboard = new bool[NUM_PLAYERS];
  [HideInInspector] public bool[] fire_touch = new bool[NUM_PLAYERS];

  public float horizontal(int player)
  {
    return horizontal_keyboard[player] + horizontal_touch[player];
  }

  public bool fire(int player)
  {
    return fire_keyboard[player] || fire_touch[player];
  }

  private void Awake()
  {
    if (instance == null)
      instance = this;
    else if (instance != this)
      Destroy(this);
  }

  private void OnDestroy()
  {
    if (instance == this)
      instance = null;
  }

  private void OnValidate()
  {
    if (players == null)
      players = new InputAxisData[NUM_PLAYERS];
    if (players.Length != NUM_PLAYERS)
      Array.Resize(ref players, NUM_PLAYERS);
  }

#if UNITY_STANDALONE || UNITY_EDITOR
  private void Update()
  {
    for (int i = 0; i < NUM_PLAYERS; i++)
    {
      horizontal_keyboard[i] = Input.GetAxisRaw(players[i].horizontal);
      fire_keyboard[i] = Input.GetButton(players[i].fireButton);
    }
  }
#endif
}
