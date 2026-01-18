using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BulletScript : MonoBehaviour
{   
    private float bulletSpeed = 30f;
    public float BulletSpeed { get {return bulletSpeed;} }

    private void Start()
    {
        StartCoroutine(DestroyPrefab());
    }

    private void DestroyOnHit()
    {
        Destroy(gameObject);
    }

    private IEnumerator DestroyPrefab()
    {
        yield return new WaitForSeconds(3.3f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {   
        Debug.Log("hit the ground or whatever " + collision.gameObject.name + " is");
        DestroyOnHit();

        if(collision.gameObject.CompareTag("Player"))
        {   
            Debug.Log("hgit player");
            collision.gameObject.GetComponent<PlayerManager>().ApplyDamage(10f);
        }

         

         
        
    }
}
