using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CommonMob : MonoBehaviour, IHighlightable
{   
    private GameObject player;
    private BulletScript bulletScript;
    private float atk = 1f;
    private float fireRate = 4f;
    private float FireRate { get { return fireRate; } set {fireRate = value;} }
    private PlayerManager playerManager;
    private Vector3 playerPos;
    private Vector3 playerVelo;
    private float interceptTime;
    private float bulletSpeed;  
    private float t1;
    private float t2;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Object bulletPrefab;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = FindFirstObjectByType<PlayerManager>();
    }

    private Vector3 shootDir()
    {   
        bulletSpeed = 10f; //bulletScript.BulletSpeed;
        playerPos = player.transform.position - transform.position;
        playerVelo = player.GetComponent<Rigidbody>().linearVelocity;

        float a = Vector3.Dot(playerVelo, playerVelo) - (bulletSpeed * bulletSpeed);
        float b = 2 * Vector3.Dot(playerVelo, playerPos);
        float c = Vector3.Dot(playerPos, playerPos);

        float sqrtDiscriminant = b * b - 4 * a * c;
        float discriminantSquared = Mathf.Sqrt(sqrtDiscriminant);
        float t1 = (-b - discriminantSquared) / (2 * a);
        float t2 = (-b + discriminantSquared) / (2 * a);

        interceptTime = Mathf.Min(t1, t2);
        if (interceptTime < 0)
        {
            interceptTime = Mathf.Max(t1, t2);
        }

        Debug.Log("t1" + t1);
        Debug.Log("t2" + t2);
        
        Vector3 aimDirection = playerPos + playerVelo * interceptTime;
        return aimDirection.normalized;

    }

    //FFUCK THIS SHIT 
    //This made me REWRITE THE MOVEMENT SYSTEM 
    
    private void rotate()
    {
        
    }
    void FixedUpdate()
    {   
        LockOnPlayer();
    }

    private void LockOnPlayer()
    {   
        transform.rotation = Quaternion.LookRotation(shootDir(), Vector3.up); //target.position - transform.position , Vector3.up); 
        
        if(Physics.Raycast(transform.position, shootDir(), out RaycastHit hitInfo, 20f, playerLayer))
        {   
            fireRate -= Time.deltaTime;
            if(fireRate <= 0 )//&& hitInfo.collider.GetComponent<PlayerManager>() != null)
            {   
                Debug.Log(fireRate);
                Debug.Log(hitInfo.collider.GetComponent<PlayerManager>().CurrentHP); 
                Debug.Log(shootDir());
                FireProjectile();
                fireRate = 4;
            }

        }
        Debug.DrawRay(transform.position, shootDir() * 20f, Color.red, 0.1f);
    }

    //add shoot that instantiates a bullet prefab that aims to hit the player
    //and upon collision do DealDamage()

    private void FireProjectile()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bulletInstance.GetComponent<Rigidbody>().linearVelocity = shootDir() * bulletSpeed;
    
    }

    public void Highlight()
    {
        
    }
}
