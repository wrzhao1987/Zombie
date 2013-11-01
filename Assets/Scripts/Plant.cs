using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour {
	
	public int life = 5;
	
	protected float damageLoop = 1.0f;
	protected float damageDelta = 0.0f;
	protected Transform myTransform;
	protected Transform spriteTransform;

	public int Life
	{
		get {return life;}
		set {life = value;}
	}
	// Use this for initialization
	protected virtual void Start () {
		myTransform = transform;
		spriteTransform = myTransform.FindChild("Sprite");
		StartCoroutine(LiveOrDie());
	}

	protected IEnumerator LiveOrDie()
	{
		while (true)
		{
			if (life <= 0)
			{
				Destroy(gameObject);
			}
			yield return null;
		}
	}
	// Update is called once per frame
	void Update () {

	}
}
