using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    int lives = 3;


    [SerializeField]
    GameObject projectileVisualisationPrefab;


    void Start()
    {
        GameplayManager.actorSpawned?.Invoke(this);
        Activate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        Rotate(Random.Range(0, 360));
        Shoot();
        StartCoroutine(Routine());
    }

    void Rotate(float angle)
    {
        transform.Rotate(0, angle, 0);
    }

    void Shoot()
    {
        RaycastHit hitInfo;

        Projectile projectile = Instantiate(projectileVisualisationPrefab).GetComponent<Projectile>();
        if (Physics.Raycast(transform.position + Vector3.up * .25f, transform.forward, out hitInfo))
        {
            projectile.SetLine(transform.position + Vector3.up * .5f, hitInfo.point);

            Actor hitActor = hitInfo.collider.GetComponent<Actor>();
            if (hitActor)
                hitActor.Damage();

        }
        else
        {
            projectile.SetLine(transform.position + Vector3.up * .5f, transform.position + Vector3.up * .5f + 20 * transform.forward);
        }
    }

    void Death()
    {
        Destroy(gameObject);
        GameplayManager.actorDestroyed?.Invoke(this);
    }

    public void Damage()
    {
        if (!isActiveAndEnabled) return;
        lives--;
        if (lives > 1)
        {
            gameObject.SetActive(false);
            GameplayManager.Instance.RespawnMe(this);
        }
        else
        {
            Death();
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

}
