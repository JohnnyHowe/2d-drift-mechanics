using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private float score;
    public Car car;
    [SerializeField]
    private float scale = 1;
    [SerializeField]
    private float angleWeight = 1;
    [SerializeField]
    private float speedWeight = 1;

    public int GetScore() {
        return (int) score;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        score += GetInstantScore(car.GetDriftAngle(), car.GetDriftSpeed()) * Time.deltaTime;
    }

    float GetInstantScore(float angle, float speed) {
        return (angle * angleWeight + speed * speedWeight) * scale;
    }
}
