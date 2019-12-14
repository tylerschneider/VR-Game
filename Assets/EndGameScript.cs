using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    private void OnDestroy()
    {
        SceneChanger.Instance.LoadScene(1);
    }
}
