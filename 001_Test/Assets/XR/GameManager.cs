using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int _scoreCounter;

    [SerializeField] private Text _scoreText;

    public void AddScore()
    {
        _scoreCounter++;

        _scoreText.text = _scoreCounter.ToString();

        if(_scoreCounter > 9)
        {
            SceneManager.LoadScene(0);
        }

    }
}
 