using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MoveBlock")
        {
            other.gameObject.tag = "normal";

            PlayerCtrlTest.Instance.PickMoveBlock(other.gameObject);

            other.gameObject.AddComponent<Rigidbody>();

            other.gameObject.GetComponent<Rigidbody>().useGravity = false;

            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            other.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            other.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;

            other.gameObject.AddComponent<Stack>();

            Destroy(this);
        }
    }

    
}
