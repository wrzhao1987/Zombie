using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {
	
	public int sunAmount = 25;
	
	private Transform myTransform;
	private Vector3 destination = Vector3.zero;
	private float accer = 10;
	
	private bool spawnByWorld = true;
	private bool collected = false;
	
	public bool SpawnByWorld
	{
		get {return spawnByWorld; }
		set {spawnByWorld = value; }
	}
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
		if (! spawnByWorld)
		{
			Animation ani = GetComponent<Animation>();
			ani.Play("SunSpawn");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (collected)
		{
			Vector3 velocity = destination - myTransform.position;
			velocity.Normalize();
			myTransform.position = (myTransform.position + velocity * Time.deltaTime * accer);
			accer += 0.5f;
		}
		else
		{
			if (spawnByWorld)
			{
				if (! (myTransform.position.y <= 15.0f))
				{
					myTransform.Translate(- Vector3.up * Time.deltaTime * 3.0f);
				}
			}
		}
	}
	
	void OnMouseDown()
	{
		if (Manager.Instance.Planting)
		{
			return;
		}
		GameObject receiver = GameObject.FindGameObjectWithTag("SunReceiver");
		destination = new Vector3(receiver.transform.position.x, receiver.transform.position.y, 5.0f);
		collected = true;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (! other.gameObject.CompareTag("SunReceiver"))
		{
			return;
		}
		Destroy(myTransform.parent.gameObject);
		Manager.Instance.Sun += sunAmount;
	}
}
