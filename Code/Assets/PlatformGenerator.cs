using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {
	public GameObject platform;
	public GameObject[] checkPoints;
	
	public float platformsPerLayer = 5;
	
	public float changeDirectionChance = 0.1f;
	public float layerAngleVariation = 60f;
	
	public float minimumAngle = 15f;
	public float angleVariation = 9f;
	
	public float minimumHeight = 2f;
	public float heightVariation = 1f;
	
	public Material debugMaterial;
	
	private float radius;
	private float baseRadius;
	
	// Use this for initialization
	void Start () {
		GeneratePlatforms();
	}
	
	void GeneratePlatforms()
	{
		float baseAngle = minimumAngle;
		float basePlatformsPerLayer = platformsPerLayer;
		float baseLayerAngleVariation = layerAngleVariation;
		float angle = 10f * Mathf.Deg2Rad;
		float direction = 1;
		
		CheckPoint checkPointScript = checkPoints[0].GetComponent<CheckPoint>();
		baseRadius = radius = checkPointScript.radialDistance;
		float currentHeight = checkPoints[0].transform.position.y;
		
		
		for(int i = 1; i < checkPoints.Length; ++i)
		{
			checkPointScript = checkPoints[i].GetComponent<CheckPoint>();
			
			while(currentHeight < checkPoints[i].transform.position.y)
			{
				GenerateLayer(currentHeight, angle);
				currentHeight += minimumHeight + heightVariation * Random.value;
				angle += direction * ( layerAngleVariation * Random.value ) * Mathf.Deg2Rad;
				
				if(changeDirectionChance > Random.value)
				{
					direction *= -1;
				}
			}
			
			radius = checkPointScript.radialDistance * 1f; //* .9f;
			minimumAngle = baseAngle * baseRadius / radius;
			platformsPerLayer = basePlatformsPerLayer * ( radius * 0.9f ) / baseRadius;
			layerAngleVariation = baseLayerAngleVariation * radius / baseRadius;
		}
	}
	
	void GenerateLayer(float height, float angle)
	{
		Vector3 currentPosition = new Vector3(
			Mathf.Cos(angle) * radius,
			height,
			Mathf.Sin(angle) * radius
		);
		GameObject newPlatform;
		bool invalid;
		RaycastHit hitInfo;
		int platformLayer = LayerMask.NameToLayer( "Platforms" );
		
		for(int i = 0; i < platformsPerLayer; ++i)
		{
			Debug.DrawRay(currentPosition, Vector3.down * 3f, Color.green, 10000f);
			invalid = Physics.Raycast(new Ray(currentPosition, Vector3.down), out hitInfo, 2f);
						
			if( ! invalid || hitInfo.transform.gameObject.layer != platformLayer ) {
				newPlatform = Instantiate(platform, currentPosition, Quaternion.identity) as GameObject;
				AlignRadial(newPlatform);
			} else {
				Debug.DrawLine(currentPosition, hitInfo.point, Color.red, 10000f);
				angle += minimumAngle / 2;
			}
			
			angle += ( minimumAngle + angleVariation * Random.value ) * Mathf.Deg2Rad;
			currentPosition.x = Mathf.Cos(angle) * radius;
			currentPosition.y = height;
			currentPosition.z = Mathf.Sin(angle) * radius;
		}
	}
	
	void AlignRadial(GameObject go)
	{
		Vector3 deltaPosition = go.transform.position - transform.position;
		deltaPosition.y = 0;
		
		float angle = Vector3.Angle(transform.right, deltaPosition) * Mathf.Deg2Rad;
		if( go.transform.position.z > 0 )
		{
			angle *= -1;
		}
		
		go.transform.position = new Vector3( Mathf.Cos(angle) * radius, go.transform.position.y, -Mathf.Sin(angle) * radius );
		go.transform.forward = deltaPosition;
	}
}
