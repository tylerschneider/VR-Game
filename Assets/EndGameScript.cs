using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    public void EndGame()
    {

            SceneChanger.Instance.LoadScene(2);

    }
}
