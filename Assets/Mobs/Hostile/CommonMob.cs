using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class CommonMob : MonoBehaviour, IHighlightable
{   
    private GameObject player;
    private float atk = 1f;
    private float fireRate = 2f;
    private float FireRate { get { return fireRate; } set {fireRate = value;} }
    private PlayerManager playerManager;

    private Vector3 playerPos;
    private Vector3 playerVelo;
    private float interceptTime;
    private float bulletSpeed; 
     
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Object bulletPrefab;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = FindFirstObjectByType<PlayerManager>();
    }

    //FFUCK THIS SHIT 
    //This made me REWRITE THE ENTRE MOVEMENT LOGIC
    //this is reusable for future predictive projectiles tho just tweak it accordingly 

    private Vector3 shootDir()
    {   
        bulletSpeed = 15f; //change this to bulletScript.BulletSpeed later for some reason it returns null lol
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

        Vector3 aimDirection = playerPos + playerVelo * interceptTime;
        return aimDirection.normalized;
    }

    void FixedUpdate()
    {   
        if (InRange)
        {
            LockOnPlayer();
        }
        else
        {
            
        }
    }

    private bool InRange;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {  
            Debug.Log("Player in range");
            InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {  
            Debug.Log("Player out of range");
            InRange = false;
        }
    }

    private void LockOnPlayer()
    {   
        Quaternion lockDirection = transform.rotation = Quaternion.LookRotation(shootDir(), Vector3.up); //target.position - transform.position , Vector3.up); 
        transform.rotation = Quaternion.Slerp(transform.rotation, lockDirection, 0.1f);
  
            fireRate -= Time.deltaTime;
            if(fireRate <= 0)
            {   
                Debug.Log(fireRate);
                 
                Debug.Log(shootDir());
                FireProjectile();
                fireRate = 2;
            }

        Debug.DrawRay(transform.position, shootDir() * 20f, Color.lightGoldenRodYellow, 0.1f);
    }
    
    private void FireProjectile()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bulletInstance.GetComponent<Rigidbody>().linearVelocity = shootDir() * bulletSpeed;
    
    }

    public void Highlight()
    {
        
    }
}
