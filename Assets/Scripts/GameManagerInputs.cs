using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManagerInputs : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetButtonUp("Leave"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
