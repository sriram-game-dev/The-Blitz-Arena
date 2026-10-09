using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    TMP_Text instrucText;

    TMP_Text completedText;
    TMP_Text score_Text;
   private float taskTime = 20f;

    private float remainingTime;
    private bool timerRunning;
    private bool taskCompleted;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Reference();
        AtStart();

        StartGame();
        PauseMenu();
        ResumeBtn();
        RestartMenu();
        RestartMenuOnPause();
        MainMenu();
        QuitGame();
    }
    private void Update()
    {
        if (!timerRunning || taskCompleted)
            return;

        remainingTime -= Time.deltaTime;

        remainingTime = Mathf.Max(remainingTime, 0f);

        ResourceManager.Instance.timerSlider.value = remainingTime;

        if (remainingTime <= 0f)
        {
            TaskFailed(GameManager.Instance.score);
        }
    }

    // =========================================================
    // REFERENCES
    // =========================================================

    void Reference()
    {
        instrucText =
            ResourceManager.Instance.instructionText
            .transform.GetChild(1)
            .GetComponent<TMP_Text>();

        completedText =ResourceManager.Instance.WaveCleared.transform.GetChild(1).GetComponent<TMP_Text>();

        score_Text =
            ResourceManager.Instance.scoreText
            .transform.GetChild(2)
            .GetComponent<TMP_Text>();
    }


    // =========================================================
    // INITIAL STATE
    // =========================================================

    void AtStart()
    {
        Time.timeScale = 1f;

        ResourceManager.Instance.MainMenuPage.SetActive(true);
        ResourceManager.Instance.PauseMenuPage.SetActive(false);
        ResourceManager.Instance.GameplayPage.SetActive(false);

        // AR camera feed OFF
        ResourceManager.Instance.arCameraBackground.enabled = false;

        // Timer OFF at start
        ResourceManager.Instance.timerSlider.gameObject.SetActive(false);
    }


    // =========================================================
    // START GAME
    // =========================================================

    void StartGame()
    {
        ResourceManager.Instance.StartBtn
            .onClick.AddListener(StartGameFunction);
    }

void StartGameFunction()
{
    Time.timeScale = 1f;

    ResourceManager.Instance.MainMenuPage.SetActive(false);
    ResourceManager.Instance.PauseMenuPage.SetActive(false);
    ResourceManager.Instance.GameplayPage.SetActive(true);

    ResourceManager.Instance.arCameraBackground.enabled = true;

    // Reset AR tracking and plane detection
    if (ResourceManager.Instance.arSession != null)
        ResourceManager.Instance.arSession.Reset();

    Placement placement = FindFirstObjectByType<Placement>();

    if (placement != null)
        placement.SetCanPlace(true);

    GamePlay();
}

    // =========================================================
    // GAMEPLAY START
    // =========================================================

    void GamePlay()
    {
        ResourceManager.Instance.scoreText.SetActive(false);
        ResourceManager.Instance.fireBtn.SetActive(false);
        ResourceManager.Instance.WaveCleared.SetActive(false);
        ResourceManager.Instance.CrossHair.SetActive(false);

        ResourceManager.Instance.pauseBtn.SetActive(true);
        ResourceManager.Instance.instructionText.SetActive(true);

        instrucText.text =
            "Move your phone to find a surface,\nthen tap to place the arena";
    }

    public void StartTask()
    {
        remainingTime = taskTime;

        ResourceManager.Instance.timerSlider.minValue = 0f;
        ResourceManager.Instance.timerSlider.maxValue = taskTime;
        ResourceManager.Instance.timerSlider.value = taskTime;

        // Show timer
        ResourceManager.Instance.timerSlider.gameObject.SetActive(true);
        timerRunning = true;
        taskCompleted = false;
    }

  public void CompleteTask()
{
    if (!timerRunning || taskCompleted)
        return;

    taskCompleted = true;
    timerRunning = false;

    ResourceManager.Instance.timerSlider.gameObject.SetActive(false);

    Debug.Log("TASK COMPLETED!");
}

private void TaskFailed(int scoreView)
{
    TMP_Text updatedScore =
            ResourceManager.Instance.ScoreDisplay
            .GetComponent<TMP_Text>();


    timerRunning = false;

    ResourceManager.Instance.timerSlider.value = 0f;
    ResourceManager.Instance.timerSlider.gameObject.SetActive(false);

        ResourceManager.Instance.fireBtn.SetActive(false);
        ResourceManager.Instance.CrossHair.SetActive(false);
        ResourceManager.Instance.pauseBtn.SetActive(false);

        ResourceManager.Instance.WaveCleared.SetActive(true);
        ResourceManager.Instance.ScoreDisplay.SetActive(true);

    Debug.Log("TASK FAILED - TIME EXPIRED!");

        completedText.text = "Wave Failed";
        updatedScore.text = $"Score: {scoreView}";

    Cannon cannon = FindFirstObjectByType<Cannon>();

    if (cannon != null)
        cannon.SetCanFire(false);

    ResourceManager.Instance.fireBtn.SetActive(false);
    ResourceManager.Instance.CrossHair.SetActive(false);
}


    // =========================================================
    // ARENA PLACED
    // =========================================================

 public void GamePlaced()
{
    GameObject instrucImg =
        ResourceManager.Instance.instructionText
        .transform.GetChild(0)
        .gameObject;

    instrucImg.SetActive(false);

    ResourceManager.Instance.scoreText.SetActive(true);

    score_Text.text = "Score: 0";

    ResourceManager.Instance.instructionText.SetActive(true);

    instrucText.text = "Press fire to shoot";

    ResourceManager.Instance.fireBtn.SetActive(true);
    ResourceManager.Instance.CrossHair.SetActive(true);

    // Enable shooting only after arena is placed
    Cannon cannon = FindFirstObjectByType<Cannon>();

    if (cannon != null)
        cannon.SetCanFire(true);
}


    // =========================================================
    // FIRST SHOT
    // =========================================================

    public void FiredOnce(bool isFired)
    {
        if (isFired)
        {
            ResourceManager.Instance.instructionText.SetActive(false);
        }
    }


    // =========================================================
    // SCORE
    // =========================================================

    public void UpdateScore(int score)
    {
        score_Text.text = $"Score: {score}";
    }


    // =========================================================
    // WAVE COMPLETED
    // =========================================================

    public void WaveCompleted(int scoreView)
    {
        TMP_Text updatedScore =
            ResourceManager.Instance.ScoreDisplay
            .GetComponent<TMP_Text>();

        ResourceManager.Instance.fireBtn.SetActive(false);
        ResourceManager.Instance.CrossHair.SetActive(false);
        ResourceManager.Instance.pauseBtn.SetActive(false);

        ResourceManager.Instance.WaveCleared.SetActive(true);
        ResourceManager.Instance.ScoreDisplay.SetActive(true);

        updatedScore.text = $"Score: {scoreView}";

        completedText.text = "Wave Completed";
    }


    // =========================================================
    // PAUSE
    // =========================================================

    void PauseMenu()
    {
        Button pauseButton =
            ResourceManager.Instance.pauseBtn.GetComponent<Button>();

        pauseButton.onClick.AddListener(PauseMenuFunction);
    }

void PauseMenuFunction()
{
    Time.timeScale = 0f;

    // Stop AR plane detection
    if ( ResourceManager.Instance.arPlaneManager != null)
    {
         ResourceManager.Instance.arPlaneManager.enabled = false;
    }

    // Stop placement input
    Placement placement = FindFirstObjectByType<Placement>();

    if (placement != null)
        placement.SetCanPlace(false);

    // Stop shooting
    Cannon cannon = FindFirstObjectByType<Cannon>();

    if (cannon != null)
        cannon.SetCanFire(false);

    ResourceManager.Instance.fireBtn.SetActive(false);
    ResourceManager.Instance.CrossHair.SetActive(false);

    ResourceManager.Instance.MainMenuPage.SetActive(false);
    ResourceManager.Instance.PauseMenuPage.SetActive(true);
    ResourceManager.Instance.GameplayPage.SetActive(false);
}

    // =========================================================
    // RESUME
    // =========================================================

    void ResumeBtn()
    {
        ResourceManager.Instance.ResumeBtn
            .onClick.AddListener(ResumeFunction);
    }

void ResumeFunction()
{
    Time.timeScale = 1f;

    ResourceManager.Instance.MainMenuPage.SetActive(false);
    ResourceManager.Instance.PauseMenuPage.SetActive(false);
    ResourceManager.Instance.GameplayPage.SetActive(true);

    Cannon cannon = FindFirstObjectByType<Cannon>();
    Placement placement = FindFirstObjectByType<Placement>();

    // Keep firing disabled while Resume touch is still active
    if (cannon != null)
        cannon.SetCanFire(false);

    ResourceManager.Instance.fireBtn.SetActive(false);
    ResourceManager.Instance.CrossHair.SetActive(false);

    StartCoroutine(
        EnableCorrectStateAfterResume(placement, cannon)
    );
}

IEnumerator EnableCorrectStateAfterResume(
    Placement placement,
    Cannon cannon)
{
    // Wait until Resume button touch is completely released
    while (Input.touchCount > 0)
    {
        yield return null;
    }

    yield return null;

    if (placement != null && placement.IsPlaced())
    {
        // =================================
        // ARENA ALREADY PLACED
        // =================================

        // Keep plane detection OFF
        if ( ResourceManager.Instance.arPlaneManager != null)
        {
             ResourceManager.Instance.arPlaneManager.enabled = false;
        }

        ResourceManager.Instance.instructionText.SetActive(true);

        instrucText.text = "Press fire to shoot";

        ResourceManager.Instance.fireBtn.SetActive(true);
        ResourceManager.Instance.CrossHair.SetActive(true);

        if (cannon != null)
            cannon.SetCanFire(true);
    }
    else
    {
        // =================================
        // ARENA NOT PLACED YET
        // =================================

        // Resume plane detection
        if (ResourceManager.Instance.arPlaneManager != null)
        {
            ResourceManager.Instance.arPlaneManager.enabled = true;
        }

        ResourceManager.Instance.instructionText.SetActive(true);

        instrucText.text =
            "Move your phone to find a surface,\nthen tap to place the arena";

        ResourceManager.Instance.fireBtn.SetActive(false);
        ResourceManager.Instance.CrossHair.SetActive(false);

        if (cannon != null)
            cannon.SetCanFire(false);

        if (placement != null)
            placement.SetCanPlace(true);
    }
}

    // =========================================================
    // RESTART
    // =========================================================

void RestartMenu()
{
    ResourceManager.Instance.RestartBtn
        .onClick.AddListener(RestartGame);
}

void RestartMenuOnPause()
{
    ResourceManager.Instance.RestartBtn2
        .onClick.AddListener(RestartGame);
}

void RestartGame()
{
    Time.timeScale = 1f;

    ResourceManager.Instance.MainMenuPage.SetActive(false);
    ResourceManager.Instance.PauseMenuPage.SetActive(false);
    ResourceManager.Instance.GameplayPage.SetActive(true);
    ResourceManager.Instance.WaveCleared.SetActive(false);
    ResourceManager.Instance.pauseBtn.SetActive(true);

    GameManager.Instance.Restart();
}
public void ResetToScanningState()
{
    ResourceManager.Instance.MainMenuPage.SetActive(false);
    ResourceManager.Instance.PauseMenuPage.SetActive(false);
    ResourceManager.Instance.GameplayPage.SetActive(true);
    ResourceManager.Instance.WaveCleared.SetActive(false);

    ResourceManager.Instance.scoreText.SetActive(false);
    ResourceManager.Instance.fireBtn.SetActive(false);
    ResourceManager.Instance.CrossHair.SetActive(false);

    ResourceManager.Instance.pauseBtn.SetActive(true);
    ResourceManager.Instance.instructionText.SetActive(true);

    instrucText.text =
        "Move your phone to find a surface,\nthen tap to place the arena";

    Cannon cannon = FindFirstObjectByType<Cannon>();

    if (cannon != null)
        cannon.SetCanFire(false);

    Placement placement = FindFirstObjectByType<Placement>();

    if (placement != null)
        placement.SetCanPlace(true);
}


    // =========================================================
    // MAIN MENU
    // =========================================================

    void MainMenu()
    {
        ResourceManager.Instance.MainMenuBtn
            .onClick.AddListener(MainMenuFunction);
        ResourceManager.Instance.MainMenuBtn2
            .onClick.AddListener(MainMenuFunction);
    }


void MainMenuFunction()
{
    Time.timeScale = 1f;

    // Stop shooting
    Cannon cannon = FindFirstObjectByType<Cannon>();
    if (cannon != null)
        cannon.SetCanFire(false);

    // Reload scene
    SceneManager.LoadScene(
        SceneManager.GetActiveScene().buildIndex
    );
}

    // =========================================================
    // QUIT
    // =========================================================

    void QuitGame()
    {
        ResourceManager.Instance.QuitBtn
            .onClick.AddListener(QuitGameFunction);

        ResourceManager.Instance.QuitBtn2
            .onClick.AddListener(QuitGameFunction);
    }

    void QuitGameFunction()
    {
        Application.Quit();
    }
}