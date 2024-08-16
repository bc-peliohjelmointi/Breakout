using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
	// M‰‰rittele itse delegate ja siihen sopiva tapahtuma
	public delegate void BlockIsDeadDelegate();
    public event BlockIsDeadDelegate BlockDestroyEvent;

	public void OnDestroy()
	{
		// Ilmoita LevelCreatorille ett‰ kuolit :(
		// L‰het‰ tapahtuma
		BlockDestroyEvent();
	}
}
