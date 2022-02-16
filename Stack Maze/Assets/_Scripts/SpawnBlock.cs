using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    public static SpawnBlock Instance;

    public GameObject playerMain;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Check Point"))
        {
            PlayerCtrlTest.Instance.rb.velocity = Vector3.zero;

            foreach(GameObject Child in PlayerCtrlTest.Instance.moveBlockList)
            {
                Destroy(Child.gameObject);
            }

            PlayerCtrlTest.Instance.moveBlockList.RemoveRange(0, PlayerCtrlTest.Instance.moveBlockList.Count);

            playerMain.transform.position = new Vector3(playerMain.transform.position.x, 0.4f, playerMain.transform.position.z);

            Debug.Log("isCheckPoint");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger Move Block"))
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;

            Destroy(PlayerCtrlTest.Instance.moveBlockList[PlayerCtrlTest.Instance.moveBlockList.Count - 1].gameObject);

            PlayerCtrlTest.Instance.moveBlockList.RemoveAt(PlayerCtrlTest.Instance.moveBlockList.Count - 1);

            playerMain.transform.position -= new Vector3(0, 0.15f, 0);
        }
    }

    
}
