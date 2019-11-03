using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandlerStart : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("Map");
    }
}
