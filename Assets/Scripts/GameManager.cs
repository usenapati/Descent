using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GOMenu;
    public PlayerMovementAdvanced pm;
    private float healthplaceholder;
    public void CompleteLevel()
    {
        Debug.Log("reached the end!");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void gameOverMenu()
    {
        if(healthplaceholder == 0)
        {
            GOMenu.SetActive(true);
        }
        
    }
}
