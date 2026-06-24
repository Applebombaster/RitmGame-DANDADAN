using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retry : MonoBehaviour
{
    public void ChooseScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }

}
