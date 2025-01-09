using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Properties")]
    public float defaultMinSpeed = 6f;
    public float defaultMaxSpeed = 100f;

    [HideInInspector]
    public float currentMinSpeed;
    [HideInInspector]
    public float currentMaxSpeed;

    [Header("Bonus/Malus Effects")]
    public float effectDuration = 5f;

    public UnityEvent OnBonusApplied;
    public UnityEvent OnMalusApplied;

    private Coroutine bonusCoroutine;
    private Coroutine malusCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentMinSpeed = defaultMinSpeed;
        currentMaxSpeed = defaultMaxSpeed;
    }

    public void ApplyBonus(float speedMultiplier)
    {
        Debug.Log($"Bonus applied: Speed x{speedMultiplier}");
        if (bonusCoroutine != null)
        {
            StopCoroutine(bonusCoroutine);
        }
        bonusCoroutine = StartCoroutine(ApplyEffect(speedMultiplier, true));
        OnBonusApplied?.Invoke();
    }

    public void ApplyMalus(float speedMultiplier)
    {
        Debug.Log($"Malus applied: Speed /{speedMultiplier}");
        if (malusCoroutine != null)
        {
            StopCoroutine(malusCoroutine);
        }
        malusCoroutine = StartCoroutine(ApplyEffect(speedMultiplier, false));
        OnMalusApplied?.Invoke();
    }

    private IEnumerator ApplyEffect(float speedMultiplier, bool isBonus)
    {
        if (isBonus)
        {
            currentMaxSpeed *= speedMultiplier;
        }
        else
        {
            currentMaxSpeed /= speedMultiplier;
        }

        yield return new WaitForSeconds(effectDuration);

        currentMaxSpeed = defaultMaxSpeed;
    }
}
    