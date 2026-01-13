using UnityEngine;

public class CommonMob : MonoBehaviour, IHighlightable
{   
    private GameObject player;
    [SerializeField] private LayerMask playerLayer;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {   
        LockOnPlayer();
    }

    private void LockOnPlayer()
    {   
        Transform target = player.transform;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up); 
        
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 2f, playerLayer))
        {
            Debug.Log("shoot");
        }
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red, 0.2f);
    }



    public void Highlight()
    {
        
    }
}
