using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MicrophoneController))]
public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private PlayerController player;
    private MicrophoneController micController;
    private GameState state;
    void Start()
    {
        ui.SetActive(false);
        state = GameState.WAIT;
        micController = GetComponent<MicrophoneController>();
        //player.AddForce(10);
    }

    public void SetGameOver()
    {
        ui.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        HandleState(state);
        if (Input.GetKeyDown(KeyCode.R))
        {   
            SceneManager.LoadScene("StartScene");
        }
    }

    private void HandleState(GameState state)
    {
        switch (state)
        {
            case GameState.WAIT:
                if (micController.DbValue >= 12 && player.GetComponent<Rigidbody2D>().velocity.y > -20)
                {
                    player.AddForce(7);
                    if (micController.PitchValue >= 250)
                    {
                        player.AddForce(5);
                    }
                }
                break;
            default:
                break;
        }
    }
}

enum GameState
{
    WAIT,
    START,
}