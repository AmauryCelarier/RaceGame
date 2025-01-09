using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WheelRotation : MonoBehaviour
{
    public Rigidbody carRigidbody; // R�f�rence au Rigidbody du v�hicule
    public float wheelRadius = 0.35f; // Rayon de la roue (en m�tres)
    public bool isFrontWheel = true; // Indique si la roue est une roue avant
    public ParticleSystem smokeEffect;
    public RealistPlayerController realistplayerController;

    void Update()
    {
        RotateWheel();
        HandleSmokeEffect();
    }
    void RotateWheel()
    {
        if (carRigidbody == null) return;
        // R�cup�re la vitesse lin�aire depuis le PlayerController ou Rigidbody
        float speed = realistplayerController.speed;
        // Calcule la direction de d�placement (avant ou arri�re)
        float direction = Vector3.Dot(carRigidbody.velocity, carRigidbody.transform.forward) >= 0 ? 1 : -1;
        // Calcule la rotation de la roue en fonction de la vitesse
        float rotationAmount = (direction * speed / (2 * Mathf.PI * wheelRadius)) * 360 * Time.deltaTime;
        transform.Rotate(Vector3.right, rotationAmount, Space.Self);
        // Applique la direction uniquement si c'est une roue avant
        if (isFrontWheel)
        {
            float steeringAngle = Input.GetAxis("Horizontal") * 30; // Ajuster l'angle en degr�s si n�cessaire
            Quaternion targetRotation = Quaternion.Euler(0, steeringAngle, 0);
            transform.localRotation = targetRotation * Quaternion.Euler(transform.localEulerAngles.x, 0, 0);
        }
    }
    void HandleSmokeEffect()
    {
        
        if (realistplayerController.IsBreaking)
        {
            if (!smokeEffect.isPlaying)
            {
                smokeEffect.Play();
            }
        }
    }
}