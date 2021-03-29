using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Override;

public class OpeningCinematic : MonoBehaviour
{
    public GameObject Player;
    public GameObject CinemaCam;
    public GameObject HUD;
    public GameObject Map;
    public GameObject Pause;
    public GameObject Skipprompt;
    private bool Animationruntime;
    // Start is called before the first frame update

    public AudioClip cinematic1;
    void Start()
    {
        CinemaCam.SetActive(true);
        Player.SetActive(false);
        HUD.SetActive(false);
        Map.SetActive(false);
        Pause.SetActive(false);
        Skipprompt.SetActive(true);
        Animationruntime = true;
        StartCoroutine(EndCut());
        new AudioBuilder()
                        .WithClip(cinematic1)
                        .WithName("Cinematicaudio")
                        .WithVolume(SoundVolume.Normal)
                        .Play();

    }
    IEnumerator EndCut()
    {
        yield return new WaitForSeconds(10);
        Player.SetActive(true);
        CinemaCam.SetActive(false);
        HUD.SetActive(true);
        Map.SetActive(true);
        Pause.SetActive(true);
        Skipprompt.SetActive(false);
        Animationruntime = false;
    }

    private void Update()
    {
        if (Animationruntime == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopCoroutine(EndCut());
                Player.SetActive(true);
                CinemaCam.SetActive(false);
                HUD.SetActive(true);
                Map.SetActive(true);
                Pause.SetActive(true);
                Skipprompt.SetActive(false);
                Animationruntime = false;
            }
        }
    }

 

    
}
