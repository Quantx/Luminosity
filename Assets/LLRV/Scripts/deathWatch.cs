using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;

public class deathWatch : MonoBehaviour
{
    public GameObject ejectSeat;
    public GameObject bigText;
    public GameObject reasonText;
    public GameObject blackBack;
    public bool useSteamVR = true;

    private bool dead = false;
    private bool victory = false;
    private float counter = 5f;

    private string[] levels = { "Level 1", "Level 2", "Level 3" };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rtbx = blackBack.GetComponent<RectTransform>();
        RectTransform bx = GetComponent<RectTransform>();

        rtbx.sizeDelta = bx.sizeDelta;

        if (dead)
        {
            counter = Mathf.Max(counter - Time.deltaTime, 0f);

            if (counter == 0)
            {
                string cur = SceneManager.GetActiveScene().name;

                if (ejectSeat.transform.parent == null || cur == "Tutorial" || cur == "Test")
                {
                    loadLevel(cur);
                }
                else
                {
                    loadLevel("Level 1");
                }
            }
        }
        else if (victory)
        {
            counter = Mathf.Max(counter - Time.deltaTime, 0f);

            if (counter == 0)
            {
                string cur = SceneManager.GetActiveScene().name;

                int i;

                for ( i = 0; i < levels.Length; i++ )
                {
                    if (cur == levels[i])
                    {
                        i++;
                        break;
                    }
                }

                if (i == levels.Length) i = 0;

                loadLevel(levels[i]);
                
            }
        }
    }

    private void loadLevel(string levelName)
    {
        if ( useSteamVR )
        {
            SteamVR_LoadLevel.Begin(levelName);
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }


    public void killPlayer(string reason) { killPlayer(reason, false); }

    public void killPlayer(string reason, bool indoorEject)
    {
        if (victory || dead) return;

        reasonText.GetComponent<Text>().text = reason;

        Text bt = bigText.GetComponent<Text>();

        if (ejectSeat.transform.parent == null && !indoorEject)
        {
            bt.color = Color.yellow;
            bt.text = "Ejected";
        }
        else
        {
            bt.color = Color.red;
            bt.text = "You Died";
        }
        
        GetComponent<Canvas>().enabled = true;

        dead = true;
    }

    public void victoryPlayer(string reason)
    {
        if (victory || dead) return;

        reasonText.GetComponent<Text>().text = reason;

        Text bt = bigText.GetComponent<Text>();
        bt.color = Color.green;
        bt.text = "You Win";

        GetComponent<Canvas>().enabled = true;

        victory = true;
    }

    public bool isDead()
    {
        return dead;
    }

    public bool isVictory()
    {
        return victory;
    }
}