using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;
    public float sfxVolume;
    public float musicVolume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
