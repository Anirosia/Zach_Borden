using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuScript : MonoBehaviour
{
    public static MenuScript _instance;

    public static Scoring scoringRef;
    
    public GameObject Main;
    public GameObject LevelSelectionPanel;
    public GameObject GameSettingsPanel;
    public GameObject PausePanel;
    public GameObject HUD;

    public GameObject btnMainBack;
    public GameObject btnGameBack;

    public Dropdown resolutionDropdown;

    public AudioSource buttonEnterSource;
    public AudioSource Music;

    public AudioClip buttonEnterSound;


    public Slider AudioSlider;
    public Slider SFXSlider;
    public Slider BrightnessSlider;

    public Light directLight;
    private GameObject directionalLightCheck;

    private bool paused;
    public bool inGame;    

    Resolution[] resolution;

    enum GameState
    {
        MainMenu,
        GameSettings,
        LevelSelection,
        InGame,
        Pause,
        HUD,
    }

    GameState _state;

    void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        scoringRef = GetComponent<Scoring>();


        paused = false;        

        resolution = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolution.Length; i++)
        {
            string option = resolution[i].width + "x" + resolution[i].height + " (" + resolution[i].refreshRate + "Hz)";
            options.Add(option);

            if (resolution[i].width == Screen.currentResolution.width &&
                resolution[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        if (!inGame)
        {
            _state = GameState.MainMenu;
        }
        else
        {
            _state = GameState.InGame;
        }

        AudioSlider.value = Music.volume;
        SFXSlider.value = buttonEnterSource.volume;
        BrightnessSlider.value = directLight.intensity;        
    }


    void Update()
    {

        switch (_state)
        {
            case GameState.MainMenu:
                inGame = false;
                Main.SetActive(true);
                LevelSelectionPanel.SetActive(false);
                GameSettingsPanel.SetActive(false);
                PausePanel.SetActive(false);
                HUD.SetActive(false);
                scoringRef.countReset();
                if (!Music.isPlaying)
                {
                    Music.Play();
                }

                if (paused)
                {
                    Time.timeScale = 1;
                    paused = false;
                }

                break;
            case GameState.LevelSelection:
                Main.SetActive(false);
                LevelSelectionPanel.SetActive(true);
                GameSettingsPanel.SetActive(false);
                PausePanel.SetActive(false);
                HUD.SetActive(false);

                break;
            case GameState.GameSettings:
                Main.SetActive(false);
                LevelSelectionPanel.SetActive(false);
                GameSettingsPanel.SetActive(true);
                PausePanel.SetActive(false);

                if (inGame)
                {
                    HUD.SetActive(true);
                    btnGameBack.SetActive(true);
                    btnMainBack.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Pause();
                    }

                }
                else
                {
                    HUD.SetActive(false);
                    btnMainBack.SetActive(true);
                    btnGameBack.SetActive(false);
                }

                break;
            case GameState.InGame:
                inGame = true;
                Main.SetActive(false);
                LevelSelectionPanel.SetActive(false);
                PausePanel.SetActive(false);
                GameSettingsPanel.SetActive(false);
                HUD.SetActive(true);
                Music.Stop();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Pause();
                }

                break;
            case GameState.Pause:
                inGame = true;
                Main.SetActive(false);
                LevelSelectionPanel.SetActive(false);
                PausePanel.SetActive(true);
                GameSettingsPanel.SetActive(false);
                HUD.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Pause();                    
                }

                break;
        }
    }

    void OnLevelWasLoaded(int level)
    {
        directionalLightCheck = GameObject.Find("Directional Light");

        if (directionalLightCheck != null)
        {
            Destroy(directionalLightCheck);
        }
    }

    public void Pause()
    {
        if(paused == false)
        {
            _state = GameState.Pause;
            paused = true;
            Time.timeScale = 0;            
        }
        else
        {
            _state = GameState.InGame;
            paused = false;
            Time.timeScale = 1;
        }
    }

    public void startGame()
    {
        _state = GameState.InGame;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);

    }

    public void playSoundEffect()
    {
        buttonEnterSource.PlayOneShot(buttonEnterSound);
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void LoadMain()
    {
        _state = GameState.MainMenu;
    }

    public void LoadPause()
    {
        _state = GameState.Pause;
    }


    public void LoadSettings()
    {
        _state = GameState.GameSettings;
    }

    public void LoadLevelSelect()
    {
        _state = GameState.LevelSelection;
    }

    public void setAudio()
    {
        Music.volume = AudioSlider.value;
    }

    public void setSFX()
    {
        buttonEnterSource.volume = SFXSlider.value;
        buttonEnterSource.PlayOneShot(buttonEnterSound);
    }

    public void setBrightness()
    {
        directLight.intensity = BrightnessSlider.value;
    }

    public void FullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else   
             Application.Quit();
        #endif
    }

}
