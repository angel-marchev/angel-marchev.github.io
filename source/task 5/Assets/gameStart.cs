using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class gameStart : MonoBehaviour
{
	public void gameStartButton (){
		SceneManager.LoadScene("MainScene");
	}

    public void tutorialButton()
    {
        SceneManager.LoadScene("tutorial");
    }
}
