using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string name;
    public TMP_InputField inputName;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
   
    public string GetName()
    {
        return name;
    }
    public void StartGame()
    {
        inputName = FindObjectOfType<TMP_InputField>();
        name = inputName.text;
        SceneManager.LoadScene(1);
    }
}
