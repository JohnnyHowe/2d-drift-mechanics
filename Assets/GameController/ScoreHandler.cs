using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField]
    private float score;
    public Car car;

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
        return Mathf.Pow(angle * speed * 0.01f, 2);
    }
}
