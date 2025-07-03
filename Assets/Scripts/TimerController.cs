using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public int timeLimit = 250;
    private TextMeshProUGUI thisText;
    private bool timerRunning = false;
    public int timeLeft;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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
