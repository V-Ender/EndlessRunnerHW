using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentGenerator : MonoBehaviour
{
    public GameObject[] segment;
    public GameObject bufferSegment;

    [SerializeField] int zPos = 0;
    [SerializeField] int zPosInterval = 30;
    [SerializeField] bool creatingSegment = false;
    [SerializeField] int segmentNum;
    [SerializeField] int totalSegments = 4;
    [SerializeField] int bufferSegmentZ = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (creatingSegment == false) 
        {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        }
    }

    IEnumerator SegmentGen() 
    {
        Instantiate(bufferSegment, new Vector3(0, 0, zPos + bufferSegmentZ / 2f), Quaternion.identity);
        zPos += bufferSegmentZ;
        yield return null;

        segmentNum = Random.Range(0, totalSegments);
        Instantiate(segment[segmentNum], new Vector3(0, 0, zPos + zPosInterval / 2f), Quaternion.identity);
        zPos += zPosInterval;
        yield return new WaitForSeconds(1);
        creatingSegment = false;
    }
}
