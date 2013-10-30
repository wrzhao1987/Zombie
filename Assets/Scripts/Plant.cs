using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour {
	
	public int life = 5;
	
	protected float damageLoop = 1.0f;
	protected float damageDelta = 0.0f;
	protected Transform myTransform;
	protected Transform spriteTransform;
	
	// Use this for initialization
	protected virtual void Start () {
		myTransform = transform;
		spriteTransform = myTransform.FindChild("Sprite");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected void OnTriggerStay(Collider other)
	{
		if (other.tag.CompareTo("Zombie") == 0)
		{
			damageDelta += Time.deltaTime;
			if (damageDelta >= damageLoop)
			{
				damageDelta = 0;
				life -= 1;
				if (life <= 0)
				{
					Destroy(gameObject);
				}
			}
		}
	}
}
