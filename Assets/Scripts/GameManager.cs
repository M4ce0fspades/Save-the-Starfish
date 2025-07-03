using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//4 minutes for 50 starfish? maybe do more trial runs first
public class GameManager : MonoBehaviour
{
    [HideInInspector] public int starfishOnBeach=0;
    [HideInInspector] public int starfishInHand=0;
    [HideInInspector] public int starfishInWater=0;
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
    private bool musicHasStarted = false;
    public bool gameIsActive = false;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI winText;
    private bool gameWon = false;
    private bool isPaused = false;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TimerController timerController;
    //[SerializeField] private GameObject failScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseScreen.SetActive(false);
        audioSourceSFX = GetComponent<AudioSource>();
        audioSourceMusic.volume = 0;
        audioSourceSFX.volume = 0;
        mainText.gameObject.SetActive(false);
        winScreen.SetActive(false);
        mainMenu.SetActive(true);
        SetPlayerActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        starfishStats.text = "Starfish out of the water: " + starfishOnBeach +"\nStarfish in inventory: " + starfishInHand +"\nStarfish saved: "+ starfishInWater;
        if (starfishInWater == spawnManager.totalSpawnCount && !gameWon)
        {
            WinGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseControl();
        }
    }
    private void PauseControl()
    {
        if (isPaused)
        {
            timerController.StartTimer();
            timerController.timeLeft--;
            pauseScreen.SetActive(false);
            Time.timeScale = 1.0f;
            gameIsActive = true;
            isPaused = false;
            SetPlayerActive(true);
        }
        else if (!isPaused && gameIsActive)
        {
            timerController.StopTimer();
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            gameIsActive = false;
            isPaused = true;
            SetPlayerActive(false);
        }
    }
    private void SetPlayerActive(bool activate)
    {
        
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
    }
    public void SFXControlVolume()
    {
        volumeSFX = sfxSlider.value;
        audioSourceSFX.volume = volumeSFX;
        audioSourceSFX.PlayOneShot(clipSFX, volumeSFX);
    }
    public void MusicControlVolume()
    {
        volumeMusic = musicSlider.value;
        audioSourceMusic.volume = volumeMusic;
        audioSourceAmbiance.volume = volumeMusic;
        if (!musicHasStarted)
        {
            audioSourceMusic.clip = clipIntroMusic;
            musicHasStarted = true;
            audioSourceMusic.Play();
            audioSourceAmbiance.Play();
        }
    }
    private void WinGame()
    {
        timerController.StopTimer();
        audioSourceMusic.Stop();
        gameWon = true;
        gameIsActive = false;
        SetPlayerActive(false);
        audioSourceSFX.PlayOneShot(winSound, volumeSFX);
        mainText.gameObject.SetActive(false);
        winScreen.SetActive(true);
        winText.text = "Congratulations! You successfully made\na difference for " + starfishInWater.ToString() + " starfish!";
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
