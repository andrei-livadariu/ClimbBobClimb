using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	float fall_timer = .65f;
	float implode_timer = 1.65f;
	public int mySkill = 0;
	int numOfSkills = 2;
	
	bool ready = true;
	
	void Start () 
	{
		mySkill = 0;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.transform.tag == "Player" && ready)
		{
			TriggerSomething();
			ready = false;
		}
	}
	
	void TriggerSomething()
	{
		switch(mySkill)
		{
		case 0: 
			StartCoroutine("Fall");
			break;
		case 1:
			StartCoroutine("Implode");
			break;
		}
	}
	
	IEnumerator Fall()
	{
		Quaternion endRotation = transform.rotation;
		while(fall_timer > 0)
		{
			transform.RotateAround(transform.forward, Random.value * 0.03f - 0.015f);
			fall_timer -= Time.deltaTime;
			yield return null;
		}
		transform.rotation = endRotation;
		
		gameObject.rigidbody.constraints = RigidbodyConstraints.None;
		gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
		gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
		gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		gameObject.rigidbody.useGravity = true;
		yield return null;
	}
	
	IEnumerator Implode()
	{
		while(implode_timer > 0)
		{
			transform.localScale = Vector3.Lerp( transform.localScale, transform.localScale * (.35f) , Time.deltaTime);
			implode_timer -= Time.deltaTime;
			yield return null;
		}
		GameObject.Destroy(gameObject);
		yield return null;
	}
}
