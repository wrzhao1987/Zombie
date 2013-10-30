using UnityEngine;
using System.Collections;

public class PeashooterBullet : MonoBehaviour {
	
	public float speed = 15.0f;
	private Transform myTransform;
	private Transform spriteTransform;
	private tk2dSpriteAnimator animator;

	private bool moving = true;
	// Use this for initialization
	void Start () {
		myTransform	= this.transform;
		spriteTransform = myTransform.FindChild("Sprite");
		animator = spriteTransform.GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
		{
			myTransform.Translate(Vector3.right * Time.deltaTime * speed);
		}
		if(myTransform.position.x >= 51.0f)
		{
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (! other.CompareTag("Zombie"))
		{
			return;
		}
		moving = false;
		animator.Play("BulletHit");
		animator.AnimationCompleted = AnimationDelegate;
	}
	
	void AnimationDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		Destroy(gameObject);
	}
}
