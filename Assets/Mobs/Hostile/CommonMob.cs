using System.Collections;
using System.Numerics;
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
    private UnityEngine.Vector3 playerPos;
    private UnityEngine.Vector3 playerVelo;
    private float interceptTime;
    private float bulletSpeed; 
    private float innacuracyDegree;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject bulletPrefab;
    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = FindFirstObjectByType<PlayerManager>();
        bulletSpeed = bulletPrefab.GetComponent<BulletScript>().BulletSpeed;
        innacuracyDegree = Random.Range(-3f, 3f);
        
    }

    //FFUCK THIS SHIT 
    //This made me REWRITE THE ENTRE MOVEMENT LOGIC
    //this is reusable for future predictive projectiles tho just tweak it accordingly 
    private UnityEngine.Vector3 shootDir()
    {   
        playerPos = player.transform.position - transform.position;
        playerVelo = player.GetComponent<Rigidbody>().linearVelocity;

        float a = UnityEngine.Vector3.Dot(playerVelo, playerVelo) - (bulletSpeed * bulletSpeed);
        float b = 2 * UnityEngine.Vector3.Dot(playerVelo, playerPos);
        float c = UnityEngine.Vector3.Dot(playerPos, playerPos);

        float sqrtDiscriminant = b * b - 4 * a * c;
        float discriminantSquared = Mathf.Sqrt(sqrtDiscriminant);
        float t1 = (-b - discriminantSquared) / (2 * a);
        float t2 = (-b + discriminantSquared) / (2 * a);

        interceptTime = Mathf.Min(t1, t2);
        if (interceptTime < 0)
        {
            interceptTime = Mathf.Max(t1, t2);
        }

        UnityEngine.Vector3 aimDirection = playerPos + playerVelo * interceptTime;
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
            Debug.Log("player in range");
            InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {  
            Debug.Log("player out of range");
            InRange = false;
        }
    }

    private void LockOnPlayer()
    {   
        UnityEngine.Quaternion lockDirection = transform.rotation = UnityEngine.Quaternion.LookRotation(shootDir(), UnityEngine.Vector3.up); //target.position - transform.position , Vector3.up); 
        transform.rotation = UnityEngine.Quaternion.Slerp(transform.rotation, lockDirection, 0.1f);
  
            fireRate -= Time.deltaTime;
            if(fireRate <= 0)
            {   
                FireProjectile();
                fireRate = 2;
            }

        Debug.DrawRay(transform.position, shootDir() * 20f, Color.lightGoldenRodYellow, 0.1f);
    }

    private void MoveTowardsPlayer()
    {
        //ik i can use player velocity and distance here similar to shootdir 
        //but im lazy and this is good enough for now so i will deal with
        //it tommorow 
    }
    
    private void FireProjectile()
    {   
        Transform shootPoint = transform.GetChild(0).gameObject.transform;
        GameObject bulletInstance = Instantiate(bulletPrefab, shootPoint.position, transform.rotation);
        bulletInstance.GetComponent<Rigidbody>().linearVelocity = shootDir() * bulletSpeed;
    }

    public void Highlight()
    {

    }
}
