using UnityEngine;

public class BonusMalus : MonoBehaviour
{
    [Header("Effect Type")]
    public bool isBonus = true;

    [Header("Effect Properties")]
    [Tooltip("Multiplier applied to the player's max speed")]
    public float speedMultiplier = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isBonus)
            {
                GameManager.Instance.ApplyBonus(speedMultiplier);
            }
            else
            {
                GameManager.Instance.ApplyMalus(speedMultiplier);
            }

            // Optionally destroy the object after the effect
            Destroy(gameObject);
        }
    }
}
