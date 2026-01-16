using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BulletScript : MonoBehaviour
{   
    private float bulletSpeed = 30f;
    public float BulletSpeed { get {return bulletSpeed;} }


    private void Update()
    {
        StartCoroutine(DestroyPrefab());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
       
    }

    private IEnumerator DestroyPrefab()
    {
        yield return new WaitForSeconds(3.3f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
        }
    }
}
