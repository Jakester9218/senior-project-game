using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene (string sceneToChangeTo)
    {
        SceneManager.LoadScene(sceneToChangeTo);
    }
}
