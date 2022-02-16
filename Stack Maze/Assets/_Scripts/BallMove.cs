using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public static BallMove Instance;

    public Rigidbody rb;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    
    void Update()
    {
        Destroy(this.gameObject, 1.5f);
    }

    private void FixedUpdate()
    {
        //transform.Translate(Vector3.forward * 15f * Time.smoothDeltaTime);

        rb.AddForce(Vector3.forward * 15f * Time.smoothDeltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject, 1f);

            Destroy(other.gameObject, 1.5f);

            other.gameObject.GetComponent<Animator>().enabled = false;

            other.gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
