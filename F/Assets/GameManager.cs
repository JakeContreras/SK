using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 3;
    public float m_StartDelay = 3f;            
    public float m_EndDelay = 3f;   
    public CameraControl m_CameraControl;
    public Text m_MessageText;
    public GameObject AI_prefab;
    public GameObject Player1_prefab;
    [HideInInspector] public GameObject Player1;
    [HideInInspector] public GameObject Player2;
    public GameObject Player2_prefab;
    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;         
    private WaitForSeconds m_EndWait;
    private string roundwinner;
    private string gamewinner;
    public int AInumber = 50;
    GameObject[] AI = new GameObject[50];
    void Start()
    {
        m_StartWait = new WaitForSeconds (m_StartDelay);
        m_EndWait = new WaitForSeconds (m_EndDelay);

        SpawnCharacters();

        StartCoroutine (GameLoop ());
    }

    private IEnumerator GameLoop ()
    {
        yield return StartCoroutine (RoundStarting ());

        yield return StartCoroutine (RoundPlaying());

        yield return StartCoroutine (RoundEnding());

        if (gamewinner != null)
        {
            SceneManager.LoadScene (0);
        }
        else
        {
            StartCoroutine (GameLoop ());
        }
    }

    private IEnumerator RoundStarting ()
    {
        m_CameraControl.SetStartPositionAndSize();
        Reset();
        m_RoundNumber+=1;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        yield return m_StartWait;
    }

    private IEnumerator RoundPlaying ()
    {
        m_MessageText.text = string.Empty;

        while (!OnePlayerLeft())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding ()
    {
        roundwinner = null;

        GetRoundWinner ();
        m_CameraControl.Move();
        m_CameraControl.Zoom();

        GetGameWinner ();

        string message = EndMessage ();
        m_MessageText.text = message;

        yield return m_EndWait;
    }

    private bool OnePlayerLeft()
    {

        int numPlayersLeft = 2;

        if (Player1.GetComponent<PlayerMovement>().isdead == true)
        {
            numPlayersLeft--;
        }
        if (Player2.GetComponent<PlayerMovement>().isdead == true)
        {
            numPlayersLeft--;
        }

        return numPlayersLeft <= 1;
    }

    private void GetRoundWinner()
    {
        if(Player1.GetComponent<PlayerMovement>().isdead == true && Player2.GetComponent<PlayerMovement>().isdead == false)
        {
            Player2.GetComponent<PlayerMovement>().wins+=1;
            roundwinner = "Player 2";
            m_CameraControl.m_Targets = Player2.transform;
            m_CameraControl.size = 4.5f;
        }
        else if(Player1.GetComponent<PlayerMovement>().isdead == false && Player2.GetComponent<PlayerMovement>().isdead == true)
        {
            Player1.GetComponent<PlayerMovement>().wins+=1;
            roundwinner = "Player 1";
            m_CameraControl.m_Targets = Player1.transform;
            m_CameraControl.size = 4.5f;
        }
        else
        {
            roundwinner = null;
            m_CameraControl.m_Targets = Player1.transform;
            m_CameraControl.size = 4.5f;
        }
    }

    private void GetGameWinner()
    {
        if (Player1.GetComponent<PlayerMovement>().wins == m_NumRoundsToWin)
        {
            gamewinner = "Player 1";
        }
        else if (Player2.GetComponent<PlayerMovement>().wins == m_NumRoundsToWin)
        {
            gamewinner = "Player 2";
        }
        else
        {
            gamewinner = null;
        }
    }

    private string EndMessage()
    {
        string message = "DRAW!";

        if (roundwinner != null)
            message = roundwinner + " WINS THE ROUND!";
        message += "\n\n\n";

        message += "Player 1: " + Player1.GetComponent<PlayerMovement>().wins + " WINS\nPlayer 2: " + Player2.GetComponent<PlayerMovement>().wins + " WINS";

        if (gamewinner != null)
            message = gamewinner + " WINS THE GAME!";

        return message;
    }

    void SpawnCharacters()
    {
        Player1 = GameObject.Instantiate(Player1_prefab);
        Player2 = GameObject.Instantiate(Player2_prefab);
        for (int i = 0; i <AInumber; i++)
        {
            AI[i] = GameObject.Instantiate (AI_prefab) as GameObject;  
            AI[i].transform.position = new Vector3(Random.Range(-12f,12f), 0.04f, Random.Range(-7.5f,7.5f));
        }
    }

    void Reset()
    {
        Player1.GetComponent<PlayerMovement>().Restart();
        Player2.GetComponent<PlayerMovement>().Restart();
        for (int i = 0; i <AInumber; i++)
        {
            AI[i].GetComponent<AIMovement2>().Restart();
        }
    }
}
