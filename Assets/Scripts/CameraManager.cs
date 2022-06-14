using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraAnimationTransition(string transition)
    {
        //Check for state so not to repeat transition

        //Transisition
        myAnimator.SetTrigger(transition);
    }
}
