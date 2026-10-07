using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] GameObject targetPrefab;

    [Header("UI")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text instructionText;
   // [SerializeField] TMP_Text waveClearedText;
    [SerializeField] GameObject fireButton;
    [SerializeField] GameObject restartButton;

    [Header("Target positions (local to base)")]
    [SerializeField] Vector3[] targetOffsets =
    {
        new Vector3(-0.25f, 0.5f, 0f),
        new Vector3( 0.00f, 0.7f, 0.1f),
        new Vector3( 0.25f, 0.5f, 0f),
    };

    Transform baseTransform;
    int score;
    int remaining;

    void Awake() => Instance = this;

    void OnEnable()  => Placement.OnBasePlaced += HandleBasePlaced;
    void OnDisable() => Placement.OnBasePlaced -= HandleBasePlaced;

    void Start()
    {
        fireButton.SetActive(false);
        restartButton.SetActive(false);
        scoreText.text = "Score: 0";
        instructionText.text = "Move your phone to find a surface,\nthen tap to place the arena";
    }

    void HandleBasePlaced(GameObject baseObj)
    {
        baseTransform = baseObj.transform;
        instructionText.gameObject.SetActive(false);
        fireButton.SetActive(true);
        StartWave();
    }

    void StartWave()
    {
        score = 0;
        UpdateScore();
        

        restartButton.SetActive(false);
        fireButton.SetActive(true);

        remaining = targetOffsets.Length;
        foreach (Vector3 offset in targetOffsets)
        {
            GameObject t = Instantiate(targetPrefab, baseTransform);
            t.transform.localPosition = offset;
        }
    }

    public void OnTargetDestroyed(int points)
    {
        score += points;
        remaining--;
        UpdateScore();

        if (remaining <= 0)
        {
           // waveClearedText.gameObject.SetActive(true);
            instructionText.gameObject.SetActive(true);
            instructionText.text = "Wave Cleared";
            fireButton.SetActive(false);
            restartButton.SetActive(true);
        }
    }

    void UpdateScore(){

        scoreText.text = $"Score: {score}";

    } 

    // Hook to Restart button's OnClick
    public void Restart(){

        StartWave();
        
    }
}