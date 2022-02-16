using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMove : MonoBehaviour
{
    float speed = 1.5f;
   
    void Start()
    {
        
    }

 
    void Update()
    {
        if(PlayerCtrlTest.Instance.isShooting == true)
        {
            StartCoroutine(MovingBot());
        }
        
    }

    IEnumerator MovingBot()
    {
        yield return new WaitForSeconds(3f);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
