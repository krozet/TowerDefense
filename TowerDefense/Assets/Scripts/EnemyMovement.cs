using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 40f;

    private Transform targetWavePoint;
    private int wavePointIndex = 0;

    void Start() {
        targetWavePoint = WayPoints.wayPoints[0];
    }

    private void Update() {
        Vector3 dir = targetWavePoint.position - this.transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

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
