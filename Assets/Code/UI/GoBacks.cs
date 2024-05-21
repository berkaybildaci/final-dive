using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoBacks : MonoBehaviour
{
    public void Controls() {
        SceneManager.LoadSceneAsync("Intro");
    }
}
