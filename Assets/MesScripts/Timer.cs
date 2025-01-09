using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI BestTimeText;

    // Timer variables
    private float timer;
    private int minutes;
    private int seconds;
    private int milliseconds;

    private const int MaxBestTimes = 3; // Top 3 scores
    private float[] bestTimes; 

    private RealistPlayerController playerController; // Référence au script RealistPlayerController

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;

        // Load best times
        bestTimes = new float[MaxBestTimes];
        LoadBestTimes();

        UpdateBestTimeText();

        // Récupérer la référence de RealistPlayerController attaché au même GameObject
        playerController = GetComponent<RealistPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        UpdateTimerText(timerText, timer);
    }

    public void SaveTimer()
    {
        // Check if the current timer qualifies as a top 3 score
        for (int i = 0; i < MaxBestTimes; i++)
        {
            if (timer > bestTimes[i])
            {
                // Shift lower scores down
                for (int j = MaxBestTimes - 1; j > i; j--)
                {
                    bestTimes[j] = bestTimes[j - 1];
                }

                bestTimes[i] = timer; // Insert new score
                break;
            }
        }

        if (bestTimes[0] < timer)
        {
            if (playerController != null)
            {
                playerController.SaveRaceData();
            }
        }

        // Save updated scores
        SaveBestTimes();
        UpdateBestTimeText();
    }

    private void LoadBestTimes()
    {
        for (int i = 0; i < MaxBestTimes; i++)
        {
            bestTimes[i] = PlayerPrefs.GetFloat($"BestTime{i}", 0);
        }
    }

    private void SaveBestTimes()
    {
        for (int i = 0; i < MaxBestTimes; i++)
        {
            PlayerPrefs.SetFloat($"BestTime{i}", bestTimes[i]);
        }
        PlayerPrefs.Save();
    }

    private void UpdateBestTimeText()
    {
        if (BestTimeText != null)
        {
            string bestTimesText = "Best Times:\n";
            for (int i = 0; i < MaxBestTimes; i++)
            {
                bestTimesText += $"{i + 1}. {FormatTime(bestTimes[i])}\n";
            }
            BestTimeText.text = bestTimesText;
        }
    }

    private void UpdateTimerText(TextMeshProUGUI textElement, float time)
    {
        if (textElement != null)
        {
            textElement.text = FormatTime(time);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}