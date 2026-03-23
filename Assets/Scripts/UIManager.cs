using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    // Singleton để PlayerController có thể gọi dễ dàng
    public static UIManager instance;

    private VisualElement root;
    private Label scoreLabel;
    private Label highScoreLabel;
    private Button restartButton;

    private const string HS_KEY = "HighScore";

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // Tìm các thành phần theo tên bạn đặt trong UI Builder
        scoreLabel = root.Q<Label>("ScoreLabel");
        highScoreLabel = root.Q<Label>("HighScoreLabel");
        restartButton = root.Q<Button>("RestartButton");

        if (restartButton != null)
        {
            restartButton.clicked += OnRestartClicked;
            restartButton.style.display = DisplayStyle.None; // Ẩn lúc mới chơi
        }

        UpdateHighScoreDisplay();
    }

    public void UpdateScoreUI(int currentScore)
    {
        if (scoreLabel != null) scoreLabel.text = "Score: " + currentScore;
        
        // Kiểm tra và lưu High Score ngay lập tức
        int hs = PlayerPrefs.GetInt(HS_KEY, 0);
        if (currentScore > hs)
        {
            PlayerPrefs.SetInt(HS_KEY, currentScore);
            PlayerPrefs.Save(); // Ép hệ thống ghi xuống ổ cứng
            UpdateHighScoreDisplay();
        }
    }

    public void ShowGameOverUI()
    {
        if (restartButton != null) restartButton.style.display = DisplayStyle.Flex;
    }

    void UpdateHighScoreDisplay()
    {
        int hs = PlayerPrefs.GetInt(HS_KEY, 0);
        if (highScoreLabel != null) highScoreLabel.text = "High Score: " + hs;
    }

    void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    internal void UpdateScoreUI(float score)
    {
        throw new NotImplementedException();
    }
}