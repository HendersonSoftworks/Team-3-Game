using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject TestProjectile;
    public Transform shootPoint;
    public float shootForce = 10f; //

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject newProjectile = Instantiate(TestProjectile, shootPoint.position, shootPoint.rotation);

        // Enable the GameObject so it becomes visible
        newProjectile.SetActive(true);

        // Log message to console for debugging
        Debug.Log("Projectile instantiated and enabled.");

        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(shootPoint.right * shootForce, ForceMode2D.Impulse);
        }
    }
}