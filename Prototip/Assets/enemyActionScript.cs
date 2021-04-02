using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyActionScript : MonoBehaviour
{
    healthBarScript playerStats;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<healthBarScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
