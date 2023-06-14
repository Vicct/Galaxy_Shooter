using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosion;
    private AudioSource _audiosourceexplosion;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Explosion()
    {
       if (_audiosourceexplosion == null)
        {
            Debug.LogError("The explosion source is null");
        }
        else
        {
            _audiosourceexplosion.clip = _explosion;
        }
    }

}
