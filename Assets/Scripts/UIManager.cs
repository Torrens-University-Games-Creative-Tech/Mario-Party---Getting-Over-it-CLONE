using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject LosePanel;
    public GameObject WinPanel;

    private static UIManager _instance;

    public static UIManager Instance { get => _instance;}

    // Start is called before the first frame update

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else Destroy(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
