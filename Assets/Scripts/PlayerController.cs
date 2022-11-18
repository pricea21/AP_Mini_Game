using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
	public float speed;
	private Rigidbody rig;

	public float jumpForce;
	public float rotateSpeed;
	private float hInput;

	private float startTime;
	private float timeTaken;

	private int collectabledPicked;
	public int maxCollectables = 20;

	public GameObject playButton;
	public TextMeshProUGUI curTimeText;

	private bool isPlaying;

	void Awake()
	{
		rig = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if(!isPlaying)
			return;

		//float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		//rig.velocity = new Vector3(x, rig.velocity.y, z);

		Vector3 dir = (transform.forward * z) * speed;
		dir.y = rig.velocity.y;

		 hInput = Input.GetAxis("Horizontal") * rotateSpeed;

		// set that as our velocity 
		rig.velocity = dir;

		curTimeText.text = (Time.time - startTime).ToString("F2");

		if(Input.GetKeyDown(KeyCode.Space))
			TryJump();
	}

	 void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * hInput;
        Quaternion anglerot = Quaternion.Euler(rotation * Time.fixedDeltaTime);
        rig.MoveRotation(rig.rotation * anglerot);
        //5 
    }

	void TryJump()
	{
		//create a ray facing down
		Ray ray = new Ray(transform.position, Vector3.down);

		//shoot the raycast 
		if(Physics.Raycast(ray, 1.5f))
			rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
	}

	public void Begin()
	{
		startTime = Time.time;
		isPlaying = true;
		playButton.SetActive(false);
	}

	void End()
	{
		timeTaken = Time.time - startTime;
		isPlaying = false;
		Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
		playButton.SetActive(true);
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.CompareTag("Collectable"))
		{
			collectabledPicked++;
			Destroy(other.gameObject);

			if(collectabledPicked == maxCollectables)
				End();
		}
	}
}
