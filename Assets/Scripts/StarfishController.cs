using UnityEngine;

public class StarfishController : MonoBehaviour
{
    [HideInInspector] public bool isThrown = false;
    private SpawnManager spawnManager;
    private GameManager gameManager;
    private AudioSource audioSourceSFX;
    [SerializeField] private GameObject waterParticle;
    [SerializeField] private AudioClip waterAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = GameObject.FindWithTag("Spawn Manager").GetComponent<SpawnManager>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        audioSourceSFX = gameManager.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= .1 && transform.position.z <= 273 && gameObject.activeInHierarchy == true)
        {
            spawnManager.ResetStarfishPos(gameObject);
            Debug.Log("reset starfish spawn");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && isThrown == true)
        {
            Debug.Log("you saved a " + gameObject.name + "! :D");
            gameObject.SetActive(false);
            gameManager.starfishInWater++;
            gameManager.starfishOnBeach--;
            audioSourceSFX.PlayOneShot(waterAudio, gameManager.volumeSFX);
            Instantiate(waterParticle, transform.position, waterParticle.transform.rotation);
        }
    }
}
