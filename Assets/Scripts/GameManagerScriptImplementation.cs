﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScriptImplementation : GameManagerScript
{
    bool playerOneTurn = true;
    PlayerScript player0, player1;
    bool calculateVerocity = false;

    bool gameEnd = false;

    GameObject camera1;
    GameObject camera2;
    // Use this for initialization
    [SerializeField]
    GameObject flat_ground, restart;

	void Start () {
		if (Application.loadedLevelName != "Menu" && Application.loadedLevelName != "Guide") {
			player0 = GameObject.Find ("Player0").GetComponent<PlayerScript> ();
			player1 = GameObject.Find ("Player1").GetComponent<PlayerScript> ();

            camera1 = GameObject.Find("MainCamera");
            camera2 = GameObject.Find("MainCamera2");

            if (!flat_ground.activeSelf)
			{
				CreateGroundBoards();
			}
		}
    }

    void CreateGroundBoards()
    {
        for(int x=0; x<4; x++)
        {
            for(int y=0; y<3; y++)
            {
                GameObject obj = (GameObject)Instantiate(Resources.Load("Board"), GameObject.Find("Grounds").transform);
                obj.transform.position = new Vector3(x * 5, 0, y * 5);
                obj.GetComponent<BoardScript>().delay = x * 10 + y * 40;
            }
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (camera1.GetComponent<Camera>().enabled)
            {
                camera1.GetComponent<Camera>().enabled = false;
                camera2.GetComponent<Camera>().enabled = true;
            }
            else
            {
                camera2.GetComponent<Camera>().enabled = false;
                camera1.GetComponent<Camera>().enabled = true;
            }
        }

        if (calculateVerocity)
        {
            if(DummyVerocityZero())
            {
                playerOneTurn = !playerOneTurn;
                SetTurn();
                calculateVerocity = false;
            }
        }

        MoveGround();

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Main");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Main2");
        }

		if (Input.GetKeyDown (KeyCode.G) && Application.loadedLevelName == "Menu") {
			SceneManager.LoadScene ("Guide");
		}

		if (Input.GetKeyDown (KeyCode.R) && Application.loadedLevelName == "Guide"){
			SceneManager.LoadScene ("Menu");
		}
	}

    bool movedown = true;
    void MoveGround()
    {
        if (flat_ground.activeSelf)
        {
            if (movedown)
            {
                GameObject.Find("Ground").transform.position = GameObject.Find("Ground").transform.position - new Vector3(0, 1.0f / 120.0f, 0);
                if (GameObject.Find("Ground").transform.position.y < -1.0f)
                {
                    movedown = false;
                }
            }
            else
            {
                GameObject.Find("Ground").transform.position = GameObject.Find("Ground").transform.position + new Vector3(0, 1.0f / 120.0f, 0);
                if (GameObject.Find("Ground").transform.position.y > 1.0f)
                {
                    movedown = true;
                }
            }
        }
    }

    void SetTurn()
    {
        if (playerOneTurn)
        {
            player0.SetPlayerActivate(true);
            player1.SetPlayerActivate(false);
        }
        else
        {
            player0.SetPlayerActivate(false);
            player1.SetPlayerActivate(true);
        }

    }

    bool DummyVerocityZero()
    {
        return player0.IsRigidbodyVelocityZero() && player1.IsRigidbodyVelocityZero();
    }

    public override void PlayerShoot()
    {
        calculateVerocity = true;
    }

    public override void GameEnd(int playerNum)
    {
		if (!gameEnd) {
			gameEnd = true;

			if (playerNum == 0) {
				//Debug.Log("2PWon");
				GameObject.Find ("Text").GetComponent<UnityEngine.UI.Text> ().text = "2PWon";
			} else {
				//Debug.Log("1PWon");
				GameObject.Find ("Text").GetComponent<UnityEngine.UI.Text> ().text = "1PWon";
			}

			restart.SetActive (true);
            
		} else if (Input.GetKeyDown (KeyCode.R))
			SceneManager.LoadScene ("Menu");
//            if(Application.loadedLevelName == "Main")
//                SceneManager.LoadScene("Main");
//            else
//                SceneManager.LoadScene("Main2");
    }
}
