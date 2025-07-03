using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField] private int timeLimit = 300;
    private TextMeshProUGUI thisText;
    private bool timerRunning = false;
    public int timeLeft = 999;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisText = gameObject.GetComponent<TextMeshProUGUI>();
        timeLeft = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        thisText.text = "Time Left: " + timeLeft + "s";
    }
    public void StartTimer()
    {
        if (!timerRunning)
        {
            InvokeRepeating("TickTimer",1,1);
            timerRunning = true;
        }
    }
    public void StopTimer()
    {
        if (timerRunning)
        {
            CancelInvoke("TickTimer");
            timerRunning = false;
        }
    }
    private void TickTimer()
    {
        timeLeft--;
    }
}
