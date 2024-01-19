using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToTheMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void EnterLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}
