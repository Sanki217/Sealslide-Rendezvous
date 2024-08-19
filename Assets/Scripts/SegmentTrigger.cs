using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SegmentTrigger : MonoBehaviour
{

    public GameObject Segment;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SegmentTrigger"))
        {
            Instantiate(Segment, new Vector3(0, 0, 300), Quaternion.identity);
        }
    }
}
