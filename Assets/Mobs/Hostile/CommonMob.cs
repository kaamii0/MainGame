using System.Collections;
using UnityEditor;
using UnityEngine;

public class CommonMob : MonoBehaviour, IHighlightable
{   
    private GameObject player;
    [SerializeField] private LayerMask playerLayer;
    private float atk = 15;
    private PlayerManager playerManager;
    
    
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
            if(hitInfo.collider.GetComponent<PlayerManager>() != null)
            {   
                Debug.Log(hitInfo.collider.GetComponent<PlayerManager>().CurrentHP); 
                //StartCoroutine(dealDamage());  
            }

        }
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red, 0.2f);
    }

    /*private IEnumerator dealDamage()
    {   
        yield return new WaitForSeconds(2f);
        playerManager.ApplyDamage(atk);
    }
    */

    public void Highlight()
    {
        
    }
}
