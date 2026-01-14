using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float bulletSpeed;

    private void Update()
    {
        StartCoroutine(DestroyPrefab());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0f, 0f, 6f * Time.fixedDeltaTime);
    }

    private IEnumerator DestroyPrefab()
    {
        yield return new WaitForSeconds(2.3f);
        Destroy(gameObject);
    }

    private void OllisionEnter(Collision collision)
    {
        // damage the player here 
    }
}
