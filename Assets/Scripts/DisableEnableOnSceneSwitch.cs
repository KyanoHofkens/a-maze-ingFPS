using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableEnableOnSceneSwitch : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        Debug.Log(_scene.name);
        if(_scene.name == "Leaderboard")
        {
            this.GetComponent<CharacterController>().enabled = false;
            this.GetComponent<FirstPersonController>().enabled = false;
            this.transform.localPosition = Vector3.zero;
        } else if(_scene.name == "MainGameRepeat")
        {
            this.GetComponent<FirstPersonController>().enabled = true;
            this.GetComponent<FirstPersonController>()._canMove = false;
            Debug.Log($"movement disabled for {this.gameObject.layer}");
        }
    }
}
