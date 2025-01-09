using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyGameNamespace;
public class ChooseDifficulty : MonoBehaviour
{
    public DifficultySettings easySettings;
    public DifficultySettings normalSettings;
    public DifficultySettings hardSettings;
    public DifficultySettings currentSettings;

    public void SetEasy()
    {
        currentSettings = easySettings;
        ApplyDifficultySettings();
        Debug.Log("C'est Easy ! Difficult� : " + currentSettings.difficultyName);
    }

    public void SetNormal()
    {
        currentSettings = normalSettings;
        ApplyDifficultySettings();
        Debug.Log("C'est Normal ! Difficult� : " + currentSettings.difficultyName);
    }

    public void SetHard()
    {
        currentSettings = hardSettings;
        ApplyDifficultySettings();
        Debug.Log("C'est Hard ! Difficult� : " + currentSettings.difficultyName);
    }

    private void ApplyDifficultySettings()
    {
        if (currentSettings == null)
        {
            Debug.LogError("currentSettings est null. V�rifie que SetEasy, SetNormal ou SetHard est appel� avant.");
            return;
        }
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance est null. V�rifie que le GameManager est dans la sc�ne.");
            return;
        }
        GameManager.Instance.SetSpeedRange(currentSettings.minSpeed, currentSettings.maxSpeed);
        GameManager.Instance.SetTimeToMaxSpeed(currentSettings.timeFromMinToMax);
        GameManager.Instance.SetObstacleProbability(currentSettings.probaAllObst);
        GameManager.Instance.SetPatternProbability(currentSettings.probaPattern0, currentSettings.probaPattern1, currentSettings.probaPattern2, currentSettings.probaPattern3);
        GameManager.Instance.SetCoeffIncreaseDifficulty(currentSettings.increasePeriod);
        GameManager.Instance.SetCurrentDifficultySettings(currentSettings);
    }
}