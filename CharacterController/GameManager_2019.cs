using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ISOCamera ISOCam;
    public Transform playerTransform;
    public GameObject[] pannels;
    public GameObject canvas;
    public GameObject QuitPanel;

    public GameObject player;
    PersonController player_cs;

    public GameObject enemy;
    Animator enemy_Ani;

    private float zoom;

    enum GameState
    {
        play,
        pauseMain,
        pauseSettings,
        Dead,
    }

    GameState _state;
    void Start()
    {
        zoom = 8f;
        ISOCam.Setup(() => playerTransform.position, () => zoom);

        player_cs = player.GetComponent<PersonController>();
        enemy_Ani = enemy.GetComponent<Animator>();

        _state = GameState.play;
    }

    private void Update()
    {
        //CHANGE TO WHEN PLAYER ENTERS CERTAIN AREAS
        if (Input.GetKeyDown("i"))
        {
            if (zoom == 10f)
            {
                zoom = 8f;
            }
            else
            {
                zoom = 5f;
            }
        }
        if (Input.GetKeyDown("o"))
        {
            if (zoom == 5f)
            {
                zoom = 8f;
            }
            else
            {
                zoom = 10f;
            }
        }

        //GameMode();


    }

    void GameMode()
    {
        switch (_state)
        {
            case GameState.play:
                Time.timeScale = 1;
                //canvas.SetActive(false);
                foreach (GameObject pannel in pannels)
                {
                        pannel.SetActive(false);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _state = GameState.pauseMain;
                }
                if (player_cs.dead)
                {
                    if(player_cs.deathDelay < 0)
                    {
                        _state = GameState.Dead;
                    }
                }
                break;
            case GameState.pauseMain:
                Time.timeScale = 0;
                //pannels[1].SetActive(true);
                foreach (GameObject pannel in pannels)
                {
                    if (pannel.name == "PausePanel")
                    {
                        pannel.SetActive(true);
                    }
                    else
                    {
                        pannel.SetActive(false);
                    }
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _state = GameState.play;
                }
                break;
            case GameState.pauseSettings:
                foreach (GameObject pannel in pannels)
                {
                    if (pannel.name == "SettingPanel")
                    {
                        pannel.SetActive(true);
                    }
                    else
                    {
                        pannel.SetActive(false);
                    }
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _state = GameState.play;
                }
                break;
            case GameState.Dead:
                Time.timeScale = 0;
                //canvas.SetActive(true);
                foreach (GameObject pannel in pannels)
                {
                    if (pannel.name == "DeathPanel")
                    {
                        pannel.SetActive(true);
                    }
                    else
                    {
                        pannel.SetActive(false);
                    }
                }
                break;
        }
    }

    public void LoadSettingsMenu()
    {
        _state = GameState.pauseSettings;
    }
    public void LoadPauseMain()
    {
        _state = GameState.pauseMain;
    }
    public void ResumeGame()
    {
        _state = GameState.play;
    }
    public void QuitMenu()
    {
        if (QuitPanel.activeSelf == false)
        {
            QuitPanel.SetActive(true);
        }
        else if(QuitPanel.activeSelf == true)
        {
            QuitPanel.SetActive(false);
        }
            
    }
    public void LoadScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
