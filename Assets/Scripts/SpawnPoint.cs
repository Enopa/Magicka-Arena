using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int enemyCount;

    //this entire script is applied to the spawn points
    //when an enemy enters the spawn point, the counter is incremented
    //when an enemy leaves the spawn point, the count is decreased by one
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            enemyCount++;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            enemyCount--;
        }
    }
}
