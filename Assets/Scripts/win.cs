using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class win : MonoBehaviour
{
    public UnityEngine.UI.Image victory;

    // Start is called before the first frame update
    void Start()
    {
        victory.CrossFadeAlpha(0, 0, false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player")
        victory.CrossFadeAlpha(1, 3.0f, false);

        //}
    }
}
