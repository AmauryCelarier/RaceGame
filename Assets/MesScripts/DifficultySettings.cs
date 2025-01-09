using UnityEngine;
[CreateAssetMenu(fileName = "DifficultySettings", menuName = "GameSettings/Difficulty")]
public class DifficultySettings : ScriptableObject
{
    public string difficultyName; // Nom de la difficulté (ex. "Easy", "Normal", "Hard")
    public float minSpeed;        // Vitesse minimale
    public float maxSpeed;        // Vitesse maximale
    public float timeFromMinToMax; // Temps pour passer de minSpeed à maxSpeed
    public float increasePeriod; // Période d'augmentation de la difficulté
    public float probaAllObst; // Probabilité d'obstacles
    public int probaPattern0; // Probabilité du pattern 0
    public int probaPattern1; // Probabilité du pattern 1
    public int probaPattern2; // Probabilité du pattern 2
    public int probaPattern3; // Probabilité du pattern 3

}