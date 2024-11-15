using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    public GameObject ropePREFAB;
    public GameObject endPosition;

    [Range(1, 30)] public int partRopeLength = 5;
    [Range(1, 30)] public int maxRopeParts = 5;

    [SerializeField] private List<GameObject> ropeParts = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Vector3 ropeMeasure = Vector3.Normalize(transform.position - endPosition.transform.position);
        for(int i = 0; i <= ropeLength; i++)
        {
            GameObject rope = Instantiate(ropePREFAB, transform.position - ropeMeasure * i, Quaternion.identity, transform);
            if(ropeParts.Count != 0)
            {
                HingeJoint2D ropeHj = rope.GetComponent<HingeJoint2D>();
                ropeHj.connectedBody = ropeParts[i - 1].GetComponent<Rigidbody2D>();
            }
            ropeParts.Add( rope );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
