using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] new private Camera camera;
    public List<GameObject> heldStarfish = new List<GameObject>();
    [SerializeField] private float maxDistance;
    private GameManager gameManager;
    private AudioSource audioSourceSFX;
    [SerializeField] private AudioClip collectAudio;
    [SerializeField] private AudioClip throwAudio;
    [SerializeField] private GameObject collectParticle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        audioSourceSFX = gameManager.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameManager.gameIsActive)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = LayerMask.GetMask("Starfish");
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance, mask))
            {
                heldStarfish.Add(hit.transform.gameObject);
                hit.transform.gameObject.SetActive(false);
                gameManager.starfishOnBeach--;
                gameManager.starfishInHand++;
                audioSourceSFX.PlayOneShot(collectAudio, gameManager.volumeSFX);
                Instantiate(collectParticle, hit.transform.position, collectParticle.transform.rotation);
            }
            else
            {

            }

        }
        if (Input.GetKeyDown(KeyCode.Space) && heldStarfish.Count != 0)
        {
            Ray shootRay = camera.ScreenPointToRay(Input.mousePosition);
            GameObject starfishToThrow = heldStarfish[^1];
            starfishToThrow.GetComponent<StarfishController>().isThrown = true;
            Rigidbody starfishRb = starfishToThrow.GetComponent<Rigidbody>();
            heldStarfish.Remove(starfishToThrow);
            starfishToThrow.SetActive(true);
            starfishToThrow.transform.position = camera.transform.position + camera.transform.forward;
            starfishRb.AddForce(shootRay.direction * 20, ForceMode.Impulse);
            gameManager.starfishInHand--;
            gameManager.starfishOnBeach++;
            audioSourceSFX.PlayOneShot(throwAudio, gameManager.volumeSFX);
        }
    }
}
