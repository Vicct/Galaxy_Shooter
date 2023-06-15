using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Up : MonoBehaviour
{
    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    [SerializeField]
    private int _powerUpID;
    [SerializeField]
    private AudioClip _powerUpaudioclip;
   
    // Update is called once per frame
    void Update()
    {
        //Move down at speed of three
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);
        // if out of screen destroy it.
        if(transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_powerUpaudioclip, transform.position);
            Player player = Other.transform.GetComponent<Player>();
            if(player != null)
            {
                switch(_powerUpID)
                {
                    case 0:
                        player.TripleshotisActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldisActive();
                        break;
            
                }
            }
            Destroy(this.gameObject);
        }
    }
}
