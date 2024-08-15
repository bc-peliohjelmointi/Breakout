using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreator : MonoBehaviour
{
    // T‰m‰ on se asia mit‰ luodaan kentt‰‰n
    public GameObject blockPrefab;
    public GameObject blockStartPlace;  // Miss‰ on aloituskohta

    // Start is called before the first frame update
    void Start()
    {
		Vector3 blockPlace = blockStartPlace.transform.position;
        float placeX = blockPlace.x;
        float placeY = blockPlace.y;
        for (int rivi = 0; rivi < 3; rivi++)
        {
            for (int i = 0; i < 5; i++)
            {
                Instantiate(blockPrefab, new Vector3(placeX, placeY, 0), Quaternion.identity);

                placeX += 1.5f; 
            }
            placeY -= 1.0f;
            placeX = blockPlace.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
