using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosion;
    private AudioSource _audioSourceExplosion;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Explosion()
    {
       if (_audioSourceExplosion == null)
        {
            Debug.LogError("The explosion source is null");
        }
        else
        {
            _audioSourceExplosion.clip = _explosion;
        }
    }

}
