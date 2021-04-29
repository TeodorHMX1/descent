using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompts : MonoBehaviour
{
    public GameObject promptcol1;
    public GameObject promptcol2;
    public GameObject promptcol3;
    public GameObject objtext1;
    public GameObject objtext2;
    public GameObject objtext3;
    ///public AudioClip scribble;
    // Start is called before the first frame update
    void Start()
    {
        objtext1.SetActive(false);
        objtext2.SetActive(false);
        objtext3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider collisionInfo) //Creates popup baised on area entered
    {
        switch (collisionInfo.name)
        {
            case "Objective 1 trigger":
                objtext1.SetActive(true);
                objtext2.SetActive(false);
                objtext3.SetActive(false);
                Destroy(promptcol1);

                break;
            case "Objective 2 trigger":
                objtext1.SetActive(false);
                objtext2.SetActive(true);
                objtext3.SetActive(false);
                Destroy(promptcol2);

                break;
            case "Objective 3 trigger":
                objtext1.SetActive(false);
                objtext2.SetActive(false);
                objtext3.SetActive(true);
                Destroy(promptcol3);

                break;
        }
    }
}
