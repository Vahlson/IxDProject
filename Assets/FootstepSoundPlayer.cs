using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundPlayer : MonoBehaviour
{

    private AudioSource aSource;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                print("Play footstep");
                var index = Random.Range(0, FootstepAudioClips.Length);
                aSource.clip = FootstepAudioClips[index];
                aSource.Play();
                //AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(transform.position), FootstepAudioVolume);
            }
        }
    }
}
