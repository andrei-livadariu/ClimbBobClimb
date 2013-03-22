using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	public Transform radialSphere;
	public Transform stairway;
	public GameObject colliderPlane;
	public float radialDistance = 22f;
	
	void Start()
	{
		Vector3 v1 = stairway.position;
		v1.y = 0f;
		Vector3 v2 = radialSphere.position;
		v2.y = 0f;
		
		radialDistance = Vector3.Distance(v1, v2);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			other.transform.GetComponent<Playorz>().radialDistance = radialDistance;
			colliderPlane.SetActive(true);
		}
	}
}
