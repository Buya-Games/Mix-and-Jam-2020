using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 timeOfDay;
    float limit = 200;
    void Update()
    {
        timeOfDay.x+=Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(timeOfDay);
        if (timeOfDay.x > limit){
            timeOfDay.x = 0;
        }

    }
}
