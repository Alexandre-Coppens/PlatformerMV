using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    public GameObject ropePREFAB;

    [Range(0, 5)] public float partRopeLength = 5;
    [Range(1, 30)] public int maxRopeParts = 5;

    [SerializeField] private List<GameObject> ropeParts = new List<GameObject>();
    public GameObject player;

    public void StartGrappling(Vector2 startPosition)
    {
        transform.position = startPosition;
        int ropeNbr = Mathf.Clamp((int)(Vector3.Distance(transform.position, player.transform.position) / partRopeLength), 1, maxRopeParts);
        Vector3 ropeMeasure = Vector3.Normalize(transform.position - player.transform.position);
        
        for(int i = 0; i < ropeNbr; i++)
        {
            GameObject rope = Instantiate(ropePREFAB, transform.position - ropeMeasure * i * partRopeLength, Quaternion.identity, transform);
            if(ropeParts.Count != 0)
            {
                HingeJoint2D ropeHj = rope.GetComponent<HingeJoint2D>();
                ropeHj.connectedBody = ropeParts[i - 1].GetComponent<Rigidbody2D>();
            }
            ropeParts.Add( rope );
        }

        player.GetComponent<DistanceJoint2D>().connectedBody = ropeParts[ropeParts.Count - 1].GetComponent<Rigidbody2D>();
    }

    public void KillGrappling()
    {
        for (int i = 0; i <= ropeParts.Count; i += 0)
        {
            Destroy( ropeParts[ropeParts.Count - 1]);
        }
        ropeParts.Clear();
        player.GetComponent<DistanceJoint2D>().connectedBody = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRopeParts * partRopeLength);
    }
}
