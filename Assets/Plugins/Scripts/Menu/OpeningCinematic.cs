using System.Collections;
using Override;
using UnityEngine;

public class OpeningCinematic : MonoBehaviour
{
    public GameObject Player;
    public GameObject CinemaCam;
    public GameObject HUD;
    public GameObject Map;
    public GameObject Pause;

    public GameObject Skipprompt;
    // Start is called before the first frame update

    public AudioClip cinematic1;
    private bool _animationruntime;

    /// <summary>
    ///     <para> Start </para>
    /// </summary>
    private void Start()
    {
        CinemaCam.SetActive(true);
        Player.SetActive(false);
        HUD.SetActive(false);
        Map.SetActive(false);
        Pause.SetActive(false);
        Skipprompt.SetActive(true);
        _animationruntime = true;
        StartCoroutine(EndCut());
        new AudioBuilder()
            .WithClip(cinematic1)
            .WithName("Cinematicaudio")
            .WithVolume(SoundVolume.Normal)
            .Play();
    }

    /// <summary>
    ///     <para> Update </para>
    /// </summary>
    private void Update()
    {
        if (!_animationruntime) return;
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        StopCoroutine(EndCut());
        Player.SetActive(true);
        CinemaCam.SetActive(false);
        HUD.SetActive(true);
        Map.SetActive(true);
        Pause.SetActive(true);
        Skipprompt.SetActive(false);
        _animationruntime = false;
    }

    /// <summary>
    ///     <para> EndCut </para>
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator EndCut()
    {
        yield return new WaitForSeconds(10);
        Player.SetActive(true);
        CinemaCam.SetActive(false);
        HUD.SetActive(true);
        Map.SetActive(true);
        Pause.SetActive(true);
        Skipprompt.SetActive(false);
        _animationruntime = false;
    }
}