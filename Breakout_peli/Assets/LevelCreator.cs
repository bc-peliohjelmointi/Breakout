using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public GameObject blockPrefab; // Mitä luodaan
    public GameObject blockStartPlace;  // Missä on aloituskohta

    void Start()
    {
        for (int rivi = 0; rivi < 3; rivi++)
        {
			// Hae aloituspaikka toisesta GameObjectista
			Vector3 blockPlace = blockStartPlace.transform.position;
            blockPlace.y -= rivi * 1.0f;
            // Luo yksi rivi
            for (int i = 0; i < 5; i++)
            {
                // Luo kohtaan blockPlace
                Instantiate(blockPrefab, blockPlace, Quaternion.identity);

                blockPlace.x += 1.5f; // Siirrä luomiskohtaa
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
