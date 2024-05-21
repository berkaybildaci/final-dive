using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoToRules : MonoBehaviour
{
    public void Controls() {
        SceneManager.LoadSceneAsync("Rules");
    }
}
