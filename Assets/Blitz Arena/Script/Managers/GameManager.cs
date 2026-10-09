using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [Header("Target positions (local to base)")]

    [SerializeField] Vector3[] targetOffsets =
    {
        new Vector3(-0.25f, 0.5f, 0f),
        new Vector3( 0.00f, 0.7f, 0.1f),
        new Vector3( 0.25f, 0.5f, 0f),
    };


    private Transform baseTransform;

    public int score;
    private int remaining;

    private List<GameObject> spawnedTargets =
        new List<GameObject>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    private void OnEnable()
    {
        Placement.OnBasePlaced += HandleBasePlaced;
    }


    private void OnDisable()
    {
        Placement.OnBasePlaced -= HandleBasePlaced;
    }


    // =========================================================
    // BASE PLACED
    // =========================================================

    void HandleBasePlaced(GameObject baseObj)
    {
        baseTransform = baseObj.transform;

        UiManager.Instance.GamePlaced();

        StartWave();

        // Enable shooting
        Cannon cannon = FindFirstObjectByType<Cannon>();

        if (cannon != null)
            cannon.SetCanFire(true);
    }


    // =========================================================
    // START WAVE
    // =========================================================

void StartWave()
{
    if (baseTransform == null)
    {
        Debug.LogWarning("Base has not been placed.");
        return;
    }

    ClearTargets();

    score = 0;
    remaining = targetOffsets.Length;

    UiManager.Instance.UpdateScore(score);

    foreach (Vector3 offset in targetOffsets)
    {
        GameObject target =
            Instantiate(
                ResourceManager.Instance.targetPrefab,
                baseTransform
            );

        target.transform.localPosition = offset;

        spawnedTargets.Add(target);
    }

    // Start the task timer
    UiManager.Instance.StartTask();
}


    // =========================================================
    // CLEAR TARGETS
    // =========================================================

    void ClearTargets()
    {
        foreach (GameObject target in spawnedTargets)
        {
            if (target != null)
                Destroy(target);
        }

        spawnedTargets.Clear();
    }


    // =========================================================
    // TARGET DESTROYED
    // =========================================================

    public void OnTargetDestroyed(int points)
    {
        score += points;

        remaining--;

        UiManager.Instance.UpdateScore(score);


        if (remaining <= 0)
        {
    
            UiManager.Instance.CompleteTask();
            UiManager.Instance.WaveCompleted(score);

            Cannon cannon =
                FindFirstObjectByType<Cannon>();

            if (cannon != null)
                cannon.SetCanFire(false);
        }
    }


    // =========================================================
    // RESTART
    // =========================================================

public void Restart()
{
    Time.timeScale = 1f;

    Placement placement = FindFirstObjectByType<Placement>();

    if (placement != null && placement.IsPlaced())
    {
        // Arena already exists
        StartWave();

        UiManager.Instance.GamePlaced();

        Cannon cannon = FindFirstObjectByType<Cannon>();

        if (cannon != null)
            cannon.SetCanFire(true);
    }
    else
    {
        // Still scanning / arena not placed
        StartWave();

        UiManager.Instance.ResetToScanningState();
    }
}
}