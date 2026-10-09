using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject targetPrefab;


    [Header("AR")]
    public ARCameraBackground arCameraBackground;
    public ARPlaneManager arPlaneManager;

    public ARSession arSession;

    // =========================
    // MAIN MENU
    // =========================

    [Header("Main Page")]
    public GameObject MainMenuPage;

    public Button StartBtn;
    public Button QuitBtn;


    // =========================
    // PAUSE MENU
    // =========================

    [Header("Pause Page")]
    public GameObject PauseMenuPage;

    public Button ResumeBtn;
    public Button RestartBtn2;
    public Button MainMenuBtn2;

    public Button QuitBtn2;


    // =========================
    // GAMEPLAY
    // =========================

    [Header("Gameplay")]
    public GameObject GameplayPage;



    public GameObject scoreText;
    public GameObject instructionText;

    public GameObject WaveCleared;
    public GameObject ScoreDisplay;

    public GameObject fireBtn;
    public GameObject pauseBtn;

    public GameObject CrossHair;

    public Slider timerSlider;

    public Button RestartBtn;
    public Button MainMenuBtn;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}