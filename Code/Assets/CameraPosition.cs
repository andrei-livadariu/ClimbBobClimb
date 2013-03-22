using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {
	public Transform player_body;
	float relativePositionY = -6.5f;
	float relativePositionZ = -12f;
	Vector3 desiredPosition;
	
	void Start () 
	{
		
	}
	
	void Update () 
	{
		desiredPosition = player_body.position + relativePositionZ * player_body.forward + relativePositionY * Vector3.up;
		
		
		transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 3f );
		transform.forward = Vector3.Lerp(transform.forward, player_body.position - transform.position, Time.deltaTime / 4);
	}
}
