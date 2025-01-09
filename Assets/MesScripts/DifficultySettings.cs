using UnityEngine;
[CreateAssetMenu(fileName = "DifficultySettings", menuName = "GameSettings/Difficulty")]
public class DifficultySettings : ScriptableObject
{
    public string difficultyName; // Nom de la difficult� (ex. "Easy", "Normal", "Hard")
    public float minSpeed;        // Vitesse minimale
    public float maxSpeed;        // Vitesse maximale
    public float timeFromMinToMax; // Temps pour passer de minSpeed � maxSpeed
    public float increasePeriod; // P�riode d'augmentation de la difficult�
    public float probaAllObst; // Probabilit� d'obstacles
    public int probaPattern0; // Probabilit� du pattern 0
    public int probaPattern1; // Probabilit� du pattern 1
    public int probaPattern2; // Probabilit� du pattern 2
    public int probaPattern3; // Probabilit� du pattern 3

}