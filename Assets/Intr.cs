using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Intr : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadSceneAsync("SciFi_Warehouse");
    }
}
