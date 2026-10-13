using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string _loadScene; //シーン名を記述

    public void ChangeScene()
    {
        SceneManager.LoadScene(_loadScene); //記述したシーンに移動

    }
}
