using UnityEngine;

public static class Helper {

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component {
        if (parent != null) {
            Transform t = parent.transform;
            foreach (Transform tr in t) {
                if (tr.tag == tag) {
                    return tr.GetComponent<T>();
                }
            }
        }
        return null;
    }

    public static T FindComponentInChildWithTag<T>(this Transform parent, string tag) where T : Component {
        if (parent != null) {
            foreach (Transform tr in parent) {
                if (tr.tag == tag) {
                    return tr.GetComponent<T>();
                }
            }
        }
        return null;
    }
}

public class TowerSpecs : MonoBehaviour
{
    [Header("Specs")]
    public float range = 15f;
    public float fireRate = 1f;

    [Header("Unity Setup Fields")]
    public GameObject bulletPrefab;

    private Transform firePoint;
    private Transform partToRotate;
    private Transform target;
    private string enemyTag = "Enemy";
    private float fireCountdown = 0f;

    private void Awake() {
        Debug.Log("Awake is called.");
        partToRotate = this.gameObject.FindComponentInChildWithTag<Transform>("PartToRotate");
        firePoint = partToRotate.FindComponentInChildWithTag<Transform>("FirePoint");
    }

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
        Debug.Log("Start is called.");
    }

    private void Update() {
        if (target == null) {
            return;
        }

        AimAtEnemy();
        CheckFireRate();
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance) {
                nearestEnemy = enemy;
                shortestDistance = distanceToEnemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range) {
            target = nearestEnemy.transform;
        } else {
            target = null;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, range);
    }

    void AimAtEnemy() {
        if (partToRotate != null) {
            float damping = 10f;
            Vector3 dir = target.position - this.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * damping).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    void CheckFireRate() {
        if (fireCountdown <= 0f) {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot() {
        if (firePoint != null) {
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null) {
                bullet.Seek(target);
            }
        }
    }
}
