using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public int framerate;
    // Start is called before the first frame update

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        framerate = Mathf.RoundToInt(1 / Time.deltaTime);
    }
}
