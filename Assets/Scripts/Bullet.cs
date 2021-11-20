using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 velocity { get; set; }

    private void Start()
    {
        GetComponent<AudioSource>().volume *= AppManager.instance.fxVolume;
    }

    void Update()
    {
        // Translates the bullet forward each frame
        //transform.Translate(Vector3.forward * Time.deltaTime * velocity);

        if (velocity == Vector3.zero)
        {
            return;
        } else 
        {
            transform.position += velocity * Time.deltaTime;
        }

        transform.LookAt(transform.position + velocity.normalized);
    }

    void OnCollisionEnter(Collision collision)
    {
        //check if we collided with an alien
        Alien alien = collision.collider.GetComponentInParent<Alien>();
        if (alien != null)
        {
            alien.TakeDamage(50f);
        }


        // Destroys the bullet when it collides with another object
        Destroy(this.gameObject);
    }
}
