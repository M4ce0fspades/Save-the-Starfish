using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;


public class GameManager : MonoBehaviour
{
    [HideInInspector] public int starfishOnBeach=0;
    [HideInInspector] public int starfishInHand=0;
    public int starfishInWater=0;
    [SerializeField] private GameObject mainText;
    [SerializeField] private TextMeshProUGUI starfishStats;
    [HideInInspector] public float volumeSFX;
    [HideInInspector] public float volumeMusic;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private FirstPersonController playerSettings;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    private AudioSource audioSourceSFX;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceAmbiance;
    [SerializeField] private AudioClip clipSFX;
    [SerializeField] private AudioClip clipIntroMusic;
    [SerializeField] private AudioClip clipInGameMusic;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    private bool musicHasStarted = false;
    public bool gameIsActive = false;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI loseText;
    private bool gameWon = false;
    private bool gameLost = false;
    private bool isPaused = false;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TimerController timerController;
    [SerializeField] private GameObject reticle;
    [SerializeField] private GameObject scoresScreen;
    [SerializeField] private TMP_InputField nameInput;
    private bool saveThisSession;
    private string playerName;
    private int sessionNumber = 1;
    private string[] arrayOfNames;
    [SerializeField] private TextMeshProUGUI[] winScoreText = new TextMeshProUGUI[10];
    [SerializeField] private TextMeshProUGUI[] timeScoreText = new TextMeshProUGUI[10];
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created



    private void Awake()
    {
        LoadSession();
    }
    void Start()
    {
        pauseScreen.SetActive(false);
        audioSourceSFX = GetComponent<AudioSource>();
        audioSourceMusic.volume = 0;
        audioSourceSFX.volume = 0;
        scoresScreen.SetActive(false);
        mainText.gameObject.SetActive(false);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        mainMenu.SetActive(true);
        SetPlayerActive(false);
        audioSourceSFX.volume = VolumeManager.Instance.sfxVolume;
        audioSourceMusic.volume = VolumeManager.Instance.musicVolume;
        audioSourceAmbiance.volume = VolumeManager.Instance.musicVolume;
        musicSlider.value = audioSourceMusic.volume;
        sfxSlider.value = audioSourceSFX.volume;
        if (VolumeManager.Instance.musicVolume != 0)
        {
            audioSourceMusic.clip = clipIntroMusic;
            musicHasStarted = true;
            audioSourceMusic.Play();
            audioSourceAmbiance.Play();
            Debug.Log("music played");
        }
        Debug.Log(VolumeManager.Instance.sfxVolume);
        Debug.Log(VolumeManager.Instance.musicVolume);
        Debug.Log(audioSourceMusic.volume);
    }

    // Update is called once per frame
    void Update()
    {
        
        starfishStats.text = "Starfish out of the water: " + starfishOnBeach +"\nStarfish in inventory: " + starfishInHand +"\nStarfish saved: "+ starfishInWater;
        if (starfishInWater == spawnManager.totalSpawnCount && !gameWon && !gameLost)
        {
            WinGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseControl();
        }
        if (timerController.timeLeft <= 0 && !gameLost && !gameWon)
        {
            LoseGame();
        }
    }
    private void PauseControl()
    {
        if (isPaused && !gameIsActive && !gameWon && !gameLost)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1.0f;
            gameIsActive = true;
            isPaused = false;
            SetPlayerActive(true);
        }
        else if (isPaused && gameWon || gameLost)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1.0f;
            isPaused = false;
            SetPlayerActive(false);
        }
        else if (!isPaused && gameIsActive)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            gameIsActive = false;
            isPaused = true;
            SetPlayerActive(false);
        }
    }
    private void SetPlayerActive(bool activate)
    {
            reticle.SetActive(activate);
            playerSettings.cameraCanMove = activate;
            playerSettings.playerCanMove = activate;
            playerSettings.lockCursor = activate;
    }
    public void StartGame()
    {
        gameIsActive = true;
        audioSourceMusic.Stop();
        audioSourceMusic.clip = clipInGameMusic;
        audioSourceMusic.Play();
        spawnManager.SpawnStarfish();
        SetPlayerActive(true);
        mainMenu.SetActive(false);
        mainText.gameObject.SetActive(true);
        timerController.StartTimer();
        if (nameInput.text != "")
        {
            saveThisSession = true;
            playerName = nameInput.text;
        }
        else
        {
            saveThisSession = false;
        }
    }
    public void SFXControlVolume()
    {
        volumeSFX = sfxSlider.value;
        audioSourceSFX.volume = volumeSFX;
        VolumeManager.Instance.sfxVolume = volumeSFX;
        audioSourceSFX.PlayOneShot(clipSFX, volumeSFX);
    }
    public void MusicControlVolume()
    {
        volumeMusic = musicSlider.value;
        audioSourceMusic.volume = volumeMusic;
        audioSourceAmbiance.volume = volumeMusic;
        VolumeManager.Instance.musicVolume = volumeMusic;
        if (!musicHasStarted)
        {
            audioSourceMusic.clip = clipIntroMusic;
            musicHasStarted = true;
            audioSourceMusic.Play();
            audioSourceAmbiance.Play();
        }
    }
    public void ScoresButtonPress()
    {
        mainMenu.SetActive(false);
        scoresScreen.SetActive(true);
    }
    public void BackButtonPress()
    {
        scoresScreen.SetActive(false);
        mainMenu.SetActive(true);    
    }
    private void LoseGame()
    {
        timerController.StopTimer();
        audioSourceMusic.Stop();
        gameLost = true;
        gameIsActive = false;
        SetPlayerActive(false);
        mainText.gameObject.SetActive(false);
        loseScreen.SetActive(true);
        loseText.text = "You ran out of time and " + (50 - starfishInWater).ToString() + " starfish died;\nbetter luck next time.";
        audioSourceSFX.PlayOneShot(loseSound, volumeSFX);
    }
    private void WinGame()
    {
        SaveSession();
        timerController.StopTimer();
        audioSourceMusic.Stop();
        gameWon = true;
        gameIsActive = false;
        SetPlayerActive(false);
        audioSourceSFX.PlayOneShot(winSound, volumeSFX);
        mainText.gameObject.SetActive(false);
        winScreen.SetActive(true);
        winText.text = "Congratulations! You successfully made\na difference for " + starfishInWater.ToString() + " starfish in only\n" + (timerController.timeLimit - timerController.timeLeft).ToString() + " seconds!";
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [System.Serializable]
    class PlayerData
    {
        public string userName;
        public int userScore;
    }
    class GameData
    {
        public int totalSessions;
    }

    public void SaveSession()
    {
        if (saveThisSession)
        {
            GameData mainData = new GameData();
            mainData.totalSessions = sessionNumber;
            string mainJsonData = JsonUtility.ToJson(mainData);
            File.WriteAllText(Application.persistentDataPath + "/mainsaveinfo.json", mainJsonData);




            
            PlayerData data = new PlayerData();
            data.userName = playerName;
            data.userScore = timerController.timeLimit - timerController.timeLeft;
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/savefile" + sessionNumber + ".json", jsonData);




        }
        
    }
    public void LoadSession()
    {
        string mainPath = Application.persistentDataPath + "/mainsaveinfo.json";
        if (File.Exists(mainPath))
        {
            string mainJsonData = File.ReadAllText(mainPath);
            GameData mainData = JsonUtility.FromJson<GameData>(mainJsonData);
            sessionNumber = mainData.totalSessions + 1;
            arrayOfNames = new string[mainData.totalSessions];
            PlayerData[] dataArray = new PlayerData[mainData.totalSessions];
            
            for (int i = 0; i<mainData.totalSessions; i++)
            {
                string path = Application.persistentDataPath + "/savefile" + (i + 1) + ".json";
                string json = File.ReadAllText(path);
                dataArray[i] = JsonUtility.FromJson<PlayerData>(json);
            }
            for (int i = 0; i<dataArray.Length; i++) 
            {
                arrayOfNames[i] = dataArray[i].userName;
                
            }

            Dictionary<string, int> counts = new Dictionary<string, int>();
            foreach (string key in arrayOfNames)
            {
                if (counts.ContainsKey(key))
                {
                    counts[key]++;
                }
                else
                {
                    counts[key] = 1;
                }
            }
            
            List<KeyValuePair<string, int>> sortedWinHighScores = counts.OrderByDescending(pair => pair.Value).ToList();
            foreach(var entry in sortedWinHighScores)
            {
                
            }
            for (int i = 0; i < sortedWinHighScores.Count && i < 10; i++)
            {
                winScoreText[i].text = (i+1) + ". " + sortedWinHighScores[i].Key + ": " + sortedWinHighScores[i].Value;
            }
            PlayerData[] sortedTimeHighScores = dataArray.OrderBy(obj => obj.userScore).ToArray();
            for (int i = 0; i < sortedTimeHighScores.Length && i < 10; i++)
            {
                timeScoreText[i].text = (i + 1) + ". " + sortedTimeHighScores[i].userName + ": " + sortedTimeHighScores[i].userScore + "s";
            }




        }

        
    }

}
