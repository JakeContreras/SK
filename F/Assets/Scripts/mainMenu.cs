using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void PlayGame ()
    {
        StartCoroutine(WaitEnter());
    }


    public void QuitGame ()
    {
        StartCoroutine(WaitExit());
    }
    IEnumerator WaitEnter()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
    IEnumerator WaitExit()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Quit!!");
        Application.Quit();
    }
}

