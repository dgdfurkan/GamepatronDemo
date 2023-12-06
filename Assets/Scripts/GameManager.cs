using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Self Variables

    #region Public Variables

    #region Serializable Variables

    [SerializeField] private Text timeText;
    [SerializeField] private GameObject gameOverUI;
    
    #region Private Variables

    private float _gameTime = 60f;
    private bool _isGameOver = false;
    
    #endregion

    #endregion

    #endregion

    #endregion

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        GameTimer();
    }

    void GameTimer()
    {
        if (!_isGameOver)
        {
            _gameTime -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.Round(_gameTime);

            if (_gameTime <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        _isGameOver = true;
        gameOverUI.SetActive(true);
    }
}
