using UnityEngine;
using System.Collections;

public class Playorz : MonoBehaviour {
	
	public AudioManager audioManager;
	
	public float movementSpeed = 0f;
	public float jumpSpeed = 0f;
	public float gravity = 0f;
	public float gravity_h = 0f;
	
	public Transform stairway;
	
	Vector3 direction_left = Vector3.zero;
	Vector3 direction_forward = Vector3.zero;
	float VSpeed = 0f;
	public float VSpeedMax = 0f;
	float HSpeed = 0f;
	public float HSpeedMax = 0f;
	
	Vector3 movementVector = Vector3.zero;
	
	CharacterController cc;
	
	float jump_timer = 0f;
	
	public float radialDistance = 26f;
	
	LinkedSpriteManager LSM;
	Sprite S;
	UVAnimation anim_idle; 
	UVAnimation anim_runstart;
	UVAnimation anim_run;
	UVAnimation anim_jump;
	string anim_current;

	bool isJumping = false;
	
	float hitTimerH = 0f;
	float hitTimerV = 0f;
	
	void Start () 
	{
		direction_left = Vector3.left;
		cc = transform.GetComponent<CharacterController>();
		LSM = GameObject.Find("PlayerSpriteManager").GetComponent<LinkedSpriteManager>();
		InitSM();
		
		audioManager = GameObject.Find("AudioManager").transform.GetComponent<AudioManager>();
	}
	
	void InitSM()
	{
		S = LSM.AddSprite(this.gameObject, 5, 5, LSM.PixelCoordToUVCoord(0, 0), LSM.PixelSpaceToUVSpace(512, 512), -transform.forward * .4f, false);
		anim_run = new UVAnimation();
		anim_run.SetAnim(
			new Vector2[]{
				LSM.PixelCoordToUVCoord(3072, 0), 
				LSM.PixelCoordToUVCoord(3072+512, 0), 
				LSM.PixelCoordToUVCoord(0, 512), 
				LSM.PixelCoordToUVCoord(512, 512), 
				LSM.PixelCoordToUVCoord(1024, 512), 
				LSM.PixelCoordToUVCoord(1024+512, 512), 
				LSM.PixelCoordToUVCoord(2048, 512), 
				LSM.PixelCoordToUVCoord(2048+512, 512)
			});
		//anim_run.BuildUVAnim(LSM.PixelCoordToUVCoord(3072, 512), LSM.PixelSpaceToUVSpace(512, 512), 16, 2, 16, 2f);
		anim_run.loopCycles = -1;
		//anim_run.PlayInReverse();
		anim_run.name = "Run";
		S.AddAnimation(anim_run);
		
		anim_idle = new UVAnimation();
		anim_idle.BuildUVAnim(LSM.PixelCoordToUVCoord(0, 512), LSM.PixelCoordToUVCoord(512, 512), 8, 2, 1, 12f);
		anim_idle.loopCycles = -1;
		S.AddAnimation(anim_idle);
		anim_idle.name = "Idle";
		
		anim_runstart = new UVAnimation();
		anim_runstart.BuildUVAnim(LSM.PixelCoordToUVCoord(0, 512), LSM.PixelCoordToUVCoord(512, 512), 8, 2, 1, 12f);
		anim_runstart.loopCycles = -1;
		S.AddAnimation(anim_runstart);
		anim_runstart.name = "RunStart";
		
		anim_current = "Run";
		S.PlayAnim(anim_run);
		LSM.ScheduleBoundsUpdate(0.5f);
	}
	
	void Update () 
	{
		CheckDirection();
		
		movementVector = Vector2.zero;
		
		//checking for jump cap.
		if(cc.isGrounded)
		{
			jump_timer = Time.time + 0.25f;
			isJumping = false;
		}
		
		//gravity
		if(!cc.isGrounded)
		{
			VSpeed -= gravity * Time.deltaTime;
			HSpeed += HSpeed > 0f ? - gravity_h * Time.deltaTime : gravity_h * Time.deltaTime;
		}
		else 
		{
			VSpeed = 0f;
			HSpeed = 0f;
		}
		
		//movement		
		if(Input.GetButton("Left"))
		{
			if(cc.isGrounded || !isJumping) 
				movementVector += direction_left * movementSpeed;
			else 
				HSpeed += movementSpeed * Time.deltaTime * 2f;
		}
		
		if(Input.GetButton("Right"))
		{
			if(cc.isGrounded || !isJumping)
				movementVector -= direction_left * movementSpeed;
			else 
				HSpeed -= movementSpeed * Time.deltaTime * 2f;
		}
		
		if(Input.GetButtonDown("Jump") && jump_timer > Time.time)
		{
			HSpeed = Input.GetButton("Right") ? -movementSpeed : (Input.GetButton("Left") ? movementSpeed : 0f);
			VSpeed = jumpSpeed * (HSpeed == 0f ? 1.25f : .9f);
			jump_timer = 0f;
			audioManager.PlaySound(audioManager.sound_jump, 0.65f);
			isJumping = true;
		}
		
		VSpeed = Mathf.Clamp(VSpeed, -VSpeedMax, VSpeedMax);
		HSpeed = Mathf.Clamp(HSpeed, -HSpeedMax, HSpeedMax);
		
		movementVector.y = VSpeed;
		movementVector += HSpeed * direction_left;
		
		cc.Move(movementVector * Time.deltaTime);
		
		CheckPosition();
		
		//UpdateAnimation();
	}
	
	/*public void UpdateAnimation()
	{
		if(cc.velocity.magnitude < 1f && !isJumping)
		{
			anim_current = "Idle";
			S.PlayAnim("Idle");
		}
		else if(anim_current == "Idle" && cc.velocity.magnitude > 0.1f)
		{
			anim_current = "Run";
			S.PlayAnim("Run");
		}
	}*/
	
	void CheckPosition()
	{
		Vector3 deltaPosition = transform.position - stairway.position;
		deltaPosition.y = 0;
		float angle = Vector3.Angle(stairway.transform.right, deltaPosition) * Mathf.Deg2Rad;
		if( transform.position.z > 0 )
		{
			angle *= -1;
		}
		transform.position = Vector3.Lerp ( transform.position, 
			new Vector3(
				Mathf.Cos(angle) * radialDistance,
				transform.position.y,
				-Mathf.Sin(angle) * radialDistance
			), Time.deltaTime * 2f
		);
	}
	
	void CheckDirection()
	{
		direction_left = Vector3.Project(direction_left, 
			Vector3.Cross(stairway.position - transform.position, Vector3.down)
			).normalized;
		
		direction_forward = stairway.position - transform.position;
		direction_forward.y = 0;
		transform.forward = Vector3.Lerp(transform.forward, direction_forward, Time.deltaTime);
		
	}
	
	public void AdjustRadialDistance(float radialDistance)
	{
		this.radialDistance = radialDistance;
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		if(hit.normal.y < -0.75f && hitTimerV < Time.time) 
		{	
			VSpeed = -.05f;
			hitTimerV = Time.time + .5f;
		}
	}
}
