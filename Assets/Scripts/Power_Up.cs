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
    [SerializeField]
    private GameObject _explosionPrefab;
    private Player _player;
 
    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);
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
                    case 3:
                        player.BulletsRefill();
                        break;
                    case 4:
                        player.UnDamage();
                        break;
                }
            }
            Destroy(this.gameObject);
        }

        if(Other.tag == "Laser" && _powerUpID == 5)
        {
            Player _player = GameObject.Find("Player").GetComponent<Player>();
            _player.ShieldisActive();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(Other.gameObject);  
            Transform Bomb_Power_Up = transform;
            Transform Bomb_Effect = Bomb_Power_Up.GetChild(0);
            BlastWave _blastWave = Bomb_Effect.GetComponent<BlastWave>();
            if (_blastWave != null)
            {
                _blastWave.ExplodeBomb();
            }
            Destroy(this.gameObject, 0.9f);
        }
    }
}
