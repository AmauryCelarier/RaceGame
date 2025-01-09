using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

using MyGameNamespace;
public class RealistPlayerController : MonoBehaviour
{
    public Transform positionRoute;
    public string raceDataFileName = "bestRace.json";

    private RaceData currentRaceData;
    private float raceStartTime;

    [Header("AVEC courbe d'animation")]
    public float turnspeed = 20f;
    [Range(3, 10)]
    public float timeFromMinToMax = 5.0f;

    public AnimationCurve accelerationSpeedCURVE;
    public AnimationCurve decelerationSpeedCURVE;
    public AnimationCurve brakingSpeedCURVE;

    [Range(1, 10)] public float RELATION_DECELERATION_SPEED = 1;
    [Range(1, 10)] public float RELATION_BRAKING_SPEED = 1;

    private float accel_x = 0;
    private float brake_x = 0;
    private float decel_x = 0;
    private float decel_val;
    private float brake_val;

    private float speed_y;
    public float speed;
    public float minSpeed = 6f;
    public float maxSpeed = 100f;


    private float horizontalInput;
    private float forwardInput;

    private GameObject player;

    public float increasePeriod = 10;
    private const float COEFF_INCREASE_DIFF = 1.05f;

    public GameObject ghostPrefab;
    private GameObject ghostInstance;

    private void Start()
    {
        DifficultySettings currentSettings = GameManager.Instance.GetCurrentDifficultySettings();
        minSpeed = currentSettings.minSpeed;
        maxSpeed = currentSettings.maxSpeed;
        timeFromMinToMax = currentSettings.timeFromMinToMax;

        player = gameObject;
        speed = minSpeed;
        currentRaceData = new RaceData();
        raceStartTime = Time.time;
        LoadRaceData();
        ReplayGhost();
    }

    void Awake()
    {
        DifficultySettings currentSettings = GameManager.Instance.GetCurrentDifficultySettings();
        InvokeRepeating("UpdateDifficulty", currentSettings.increasePeriod, currentSettings.increasePeriod);
    }

    private void UpdateDifficulty()
    {
        DifficultySettings currentSettings = GameManager.Instance.GetCurrentDifficultySettings();
        Debug.Log("Curr_ProbaAllObst : augmente niveau");
        minSpeed *= COEFF_INCREASE_DIFF;
        GameManager.Instance.SetCurrentMinAndMaxSpeed(minSpeed, maxSpeed);
        if (minSpeed > maxSpeed) minSpeed = maxSpeed; // Limite à la vitesse max
    }

    private void Update()
    {
        // Synchronize speeds with GameManager
        float minSpeed = GameManager.Instance.currentMinSpeed;
        float maxSpeed = GameManager.Instance.currentMaxSpeed;

        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        // DECELERATION
        if (Mathf.Abs(forwardInput) <= 0.01f)
        {
            decel_x += Time.deltaTime;
            if (decel_x > 1) decel_x = 1;

            decel_val = decelerationSpeedCURVE.Evaluate(decel_x) * Time.deltaTime / timeFromMinToMax * RELATION_DECELERATION_SPEED;
            accel_x -= decel_val;
        }
        else
        {
            decel_x -= Time.deltaTime;
            if (decel_x < 0) decel_x = 0;
        }

        // FREINAGE
        if (forwardInput < -0.01f)
        {
            brake_x += Time.deltaTime;
            if (brake_x > 1) brake_x = 1;

            brake_val = brakingSpeedCURVE.Evaluate(brake_x) * Time.deltaTime / timeFromMinToMax * RELATION_BRAKING_SPEED;
            accel_x -= brake_val;
        }
        else
        {
            brake_x -= Time.deltaTime;
            if (brake_x < 0) brake_x = 0;
        }

        // ACCELERATION
        if (forwardInput > 0.01f)
        {
            accel_x += Time.deltaTime / timeFromMinToMax;
        }

        if (accel_x < 0.0f) accel_x = 0.0f;
        if (accel_x > 1.0f) accel_x = 1.0f;

        speed_y = accelerationSpeedCURVE.Evaluate(accel_x);
        speed = minSpeed + (maxSpeed - minSpeed) * speed_y;

        // Move forward and rotate
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.Rotate(Vector3.up, Time.deltaTime * turnspeed * horizontalInput);

        RecordRaceData();
        CheckOutOfBounds();
    }

    private void RecordRaceData()
    {
        float currentTime = Time.time - raceStartTime;

        currentRaceData.positions.Add(transform.position);
        currentRaceData.rotations.Add(transform.rotation);
        currentRaceData.timestamps.Add(currentTime);
    }

    private void CheckOutOfBounds()
    {
        float playerHeight = player.GetComponent<Renderer>().bounds.size.y;
        float heightDifference = positionRoute.position.y - transform.position.y;

        if (heightDifference > playerHeight)
        {
            Debug.Log("ENDscene loaded");
            SceneManager.LoadScene("ENDscene");
        }
    }

    private RaceData LoadRaceData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, raceDataFileName);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<RaceData>(jsonData);
        }
        return null;
    }

    public void SaveRaceData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, raceDataFileName);
        string jsonData = JsonUtility.ToJson(currentRaceData, true);
        File.WriteAllText(filePath, jsonData);
        Debug.Log($"Race data saved to {filePath}");
    }

    private void ReplayGhost()
    {
        RaceData bestRaceData = LoadRaceData();
        if (bestRaceData == null) return;

        StartCoroutine(ReplayGhostCoroutine(bestRaceData));
    }

    private IEnumerator ReplayGhostCoroutine(RaceData raceData)
    {
        ghostInstance = Instantiate(ghostPrefab);
        for (int i = 0; i < raceData.positions.Count; i++)
        {
            ghostInstance.transform.position = raceData.positions[i];
            ghostInstance.transform.rotation = raceData.rotations[i];
            if (i < raceData.timestamps.Count - 1)
            {
                float waitTime = raceData.timestamps[i + 1] - raceData.timestamps[i];
                yield return new WaitForSeconds(waitTime);
            }
        }
        Destroy(ghostInstance);
    }
}
