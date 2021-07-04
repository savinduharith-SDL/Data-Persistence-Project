using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    private bool m_Started = false;
    private int m_Points;
    public Text highScoreText;
    
    private bool m_GameOver = false;
    private int highScore = 0;
    private bool isThisAHighScoreInstance;
    private string highScorePerson;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        ScoreText.text = $"Score : 0 " + GameManager.Instance.GetName();
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        GetHighScoreDetails();

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points} " + GameManager.Instance.GetName();
        if(m_Points > highScore)
        {
            HighScoreMachine(m_Points);
        }
    }

    public void GameOver()
    {
        if(isThisAHighScoreInstance)
        {
            SaveHighScoreDetails();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    private void HighScoreMachine(int currentScore)
    {
        isThisAHighScoreInstance = true;
        highScore = currentScore;
        highScorePerson = GameManager.Instance.GetName();
        highScoreText.text = "Best Score : " + currentScore + "Name : " + highScorePerson;
    }
    public void SaveHighScoreDetails()
    {
        HighScoreManager manager = new HighScoreManager();
        manager.currentHighScore = highScore;
        manager.CurrentHighscoreUser = highScorePerson;
        string json = JsonUtility.ToJson(manager);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void GetHighScoreDetails()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreManager manager = JsonUtility.FromJson<HighScoreManager>(json);
            highScore = manager.currentHighScore;
            highScorePerson = manager.CurrentHighscoreUser;
            highScoreText.text = "Best Score : " + highScore +" Name : " + highScorePerson;
        }
    }
}
class HighScoreManager
{
    public int currentHighScore;
    public string CurrentHighscoreUser;
}
