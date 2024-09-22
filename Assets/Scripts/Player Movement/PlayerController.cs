using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Move _move;
    
    public static PlayerController Instance;
    public GameObject FirstPersonCamera;
    public GameObject CutsceneCamera;
    public GameObject Dialogue;
    public GameObject Teleporter;

    private void Awake()
    {
        Instance = this;
    }
    public void activate()
    {
        _move.enabled = false;
    }

    public void deactivate()
    {
        _move.enabled = true;
    }
}
