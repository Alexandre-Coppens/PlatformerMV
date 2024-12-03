using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    public GameObject ropePREFAB;

    [Range(0, 5)] public float partRopeLength = 5;
    [Range(1, 30)] public int maxRopeParts = 5;

    [SerializeField] private List<GameObject> ropeParts = new List<GameObject>();
    public GameObject player;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

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

        lineRenderer.enabled = true;
        lineRenderer.positionCount = ropeParts.Count + 2;

    }

    private void Update()
    {
        if(ropeParts.Count > 0)
        {
            lineRenderer.SetPosition(0, transform.position);
            int pos = 1;
            foreach(GameObject gO in ropeParts)
            {
                lineRenderer.SetPosition(pos, gO.transform.position);
                pos++;
            }
            lineRenderer.SetPosition(pos, player.transform.position + new Vector3(0,1));
        }
    }

    public void KillGrappling()
    {
        ropeParts.Clear();
        player.GetComponent<DistanceJoint2D>().enabled = false;
        int test = 50;
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
        while ( transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
            if(transform.childCount == 0 || test == 0) { return; }
            test--;
        }
    }

    private void FixedUpdate()
    {
        if (!Physics2D.OverlapCircle(transform.position + new Vector3(0,0.5f), 0.1f))
        {
            KillGrappling();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRopeParts * partRopeLength);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0.5f), 0.1f);

    }
}
