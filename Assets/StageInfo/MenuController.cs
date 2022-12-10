using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public void PlayGame()
    {
        print("started Game");
        SceneManager.LoadScene("Sakuya");
    }

    public void QuitGame()
    {
        print("exited Game");
        Application.Quit();
    }
}
