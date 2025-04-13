using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] starfishPrefabs = new GameObject[6];
    private GameManager gameManager;
    public int totalSpawnCount = 50;
    private readonly float yPos = .6f;
    private readonly float rearBound = 248f;
    private readonly float frontBound = 270f;
    private readonly float sideBound = 99f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        
    }

    public void SpawnStarfish()
    {
        for (int i = 0; i < totalSpawnCount; i++)
        {
            int starfishIndex = Random.Range(0, starfishPrefabs.Length);
            GameObject starfish = starfishPrefabs[starfishIndex];
            float xPos = Random.Range(-sideBound, sideBound);
            float zPos = Random.Range(rearBound, frontBound);
            Vector3 spawnPos = new(xPos, yPos, zPos);
            Instantiate(starfish, spawnPos, starfish.transform.rotation);
            gameManager.starfishOnBeach++;
        }
        starfishPrefabs = null;

    }
    public void ResetStarfishPos(GameObject starfish)
    {
        float xPos = Random.Range(-sideBound, sideBound);
        float zPos = Random.Range(rearBound, frontBound);
        starfish.transform.position = new Vector3(xPos, yPos, zPos);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
