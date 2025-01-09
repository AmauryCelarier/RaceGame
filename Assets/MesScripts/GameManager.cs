using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MyGameNamespace
{
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

        public DifficultySettings currentDifficulty;

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

        public void SetCurrentMinAndMaxSpeed(float minSpeed, float maxSpeed)
        {
            currentMinSpeed = minSpeed;
            currentMaxSpeed = maxSpeed;
        }
        public void SetSpeedRange(float minSpeed, float maxSpeed)
        {
            defaultMinSpeed = minSpeed;
            defaultMaxSpeed = maxSpeed;
            Debug.Log($"Vitesse réglée : min = {minSpeed}, max = {maxSpeed}");
        }
        public void SetTimeToMaxSpeed(float timeFromMinToMax)
        {
            Debug.Log($"Temps pour atteindre la vitesse max : {timeFromMinToMax}");
        }
        public void SetObstacleProbability(float probaAllObst)
        {
            Debug.Log($"Probabilité des obstacles réglée : {probaAllObst}");
        }
        public void SetPatternProbability(int probaPattern0, int probaPattern1, int probaPattern2, int probaPattern3)
        {
            Debug.Log($"Probabilité des patterns réglée : {probaPattern0}, {probaPattern1}, {probaPattern2}, {probaPattern3}");
        }
        public void SetCoeffIncreaseDifficulty(float increasePeriod)
        {
            Debug.Log($"Coefficient d'augmentation de la difficulté réglé : {increasePeriod}");
        }
        public void SetCurrentDifficultySettings(DifficultySettings settings)
        {
            currentDifficulty = settings;
        }
        public DifficultySettings GetCurrentDifficultySettings()
        {
            return currentDifficulty;
        }
    }
}
    