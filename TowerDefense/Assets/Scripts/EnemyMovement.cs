using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 20f;

    private Transform targetWavePoint;
    private int wavePointIndex = 0;
    private Quaternion _facing;

    void Start() {
        targetWavePoint = WayPoints.wayPoints[0];
    }

    private void Update() {
        float damping = 8f;
        Vector3 dir = targetWavePoint.position - this.transform.position;
        // Moves enemy to the next point
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Rotate the enemy towards next point
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        // Fix facing direction of enemy assets
        targetRotation *= Quaternion.Euler(0, 90, 0);
        // Slowly rotate enemy
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * damping);

        if (Vector3.Distance(this.transform.position, targetWavePoint.position) <= 0.4f) {
            GetNextWayPoint();
        }
    }

    private void GetNextWayPoint() {

        if (wavePointIndex >= WayPoints.wayPoints.Length - 1) {
            Destroy(gameObject);
            return;
        } 
        wavePointIndex++;
        targetWavePoint = WayPoints.wayPoints[wavePointIndex];
    }
}
