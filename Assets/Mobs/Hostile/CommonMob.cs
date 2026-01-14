using System.Collections;
using UnityEditor;
using UnityEngine;

public class CommonMob : MonoBehaviour, IHighlightable
{   
    private GameObject player;
    private float atk = 1f;
    private float fireRate = 3f;
    private float FireRate { get { return fireRate; } set {fireRate = value;} }
    private PlayerManager playerManager;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Object bulletPrefab;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = FindFirstObjectByType<PlayerManager>();
    }
    void Update()
    {   
        LockOnPlayer();
    }

    private void LockOnPlayer()
    {   
        Transform target = player.transform;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up); 
        
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 3f, playerLayer))
        {   
            fireRate -= Time.deltaTime;
            if(fireRate <= 0 && hitInfo.collider.GetComponent<PlayerManager>() != null)
            {   
                Debug.Log(hitInfo.collider.GetComponent<PlayerManager>().CurrentHP); 
                FireProjectile();
                fireRate = 2;
            }

        }
        Debug.DrawRay(transform.position, transform.forward * 3f, Color.red, 0.2f);
    }

    //add shoot that instantiates a bullet prefab that aims to hit the player
    //and upon collision do DealDamage()

    private void FireProjectile()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    public void Highlight()
    {
        
    }
}
