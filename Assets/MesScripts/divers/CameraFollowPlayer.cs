using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    [Tooltip("temps de latence : décalage de suivi du joueur ")]
    public float smoothTime ;

    private Vector3 offsetThirdPerson; // Décalage pour la vue 3ème personne
    private Vector3 offsetFirstPerson; // Décalage pour la vue 1ère personne
    private Vector3 currentOffset;     // Décalage actuel (interpolation entre les deux)  
    private Vector3 velocity = Vector3.zero;

    [Tooltip("Vitesse de transition entre les deux vues")]
    public float transitionSpeed = 2.0f;
    private bool isFirstPerson = false;
    void Start()
    {
        //offset= new Vector3(0,5,-7);          // décalalge calculé au lancement de la scène
        offsetThirdPerson = new Vector3(0, 5, -7);  // Exemple pour une vue 3ème personne
        offsetFirstPerson = new Vector3(0, 2, 1);   // Exemple pour une vue 1ère personne (à ajuster)
        currentOffset = offsetThirdPerson;
    }

    void Update()    {
        // sans latence
        //transform.position=player.transform.position + offset ; 
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            StopAllCoroutines();
            StartCoroutine(SmoothTransition());
        }
        // avec latence    c'est la question K du TP1 
        Vector3 targetPosition = player.transform.TransformPoint(currentOffset);
        if (isFirstPerson)
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.0f);
        else
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

    }
    private IEnumerator SmoothTransition()
    {
        Vector3 startOffset = currentOffset;
        Vector3 endOffset = isFirstPerson ? offsetFirstPerson : offsetThirdPerson;
        float elapsedTime = 0;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;
            currentOffset = Vector3.Lerp(startOffset, endOffset, elapsedTime);
            yield return null;
        }
        currentOffset = endOffset; // Assure la fin exacte de la transition
    }
}
