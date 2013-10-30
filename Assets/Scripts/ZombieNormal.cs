using UnityEngine;
using System.Collections;

public class ZombieNormal : MonoBehaviour {
	
	public int life;
	public float moveSpeed = 1.0f;
	private float currentMoveSpeed = 0.0f;

	private Transform myTransform;
	private Transform spriteTransform;
	private tk2dSprite sprite;
	private tk2dSpriteAnimator animator;
	private float animatorFPS;
	private float currentAnimatorFPS;
	
	private Collider plant;
	
	private bool slow;
	private float slowDelta;
	private float slowDuration = 3.0f;
	
	private bool attacking = false;
	private bool running = true;
	private bool dying = false;
	private bool dead = false;

	// Use this for initialization
	void Start ()
	{
		myTransform	= this.transform;
		spriteTransform = myTransform.FindChild("Sprite");
		sprite = spriteTransform.GetComponent<tk2dSprite>();
		animator = spriteTransform.GetComponent<tk2dSpriteAnimator>();
		animatorFPS = animator.ClipFps;
		
		currentMoveSpeed = moveSpeed;
		
		StartCoroutine(SlowDown());
	}
	
	protected IEnumerator SlowDown()
	{
		while (true)
		{
			while (slow && slowDelta <= slowDuration)
			{
				if (! sprite.color.Equals(Color.blue))
				{
					sprite.color = Color.blue;
				}
				currentAnimatorFPS = animatorFPS * 0.5f;
				animator.ClipFps = currentAnimatorFPS;
				currentMoveSpeed = moveSpeed * 0.5f;
				slowDelta += Time.deltaTime;
				yield return null;
			}
			slow = false;
			
			if (! sprite.color.Equals(Color.white))
			{
				sprite.color = Color.white;
			}
			currentMoveSpeed = moveSpeed;
			animator.ClipFps = animatorFPS;
			yield return null;
		}
	}
	// Update is called once per frame
	void Update ()
	{	
		if (dead)
		{
			PlayAnimation("Die");
			animator.AnimationCompleted = AnimatorDelegate;
			return;
		}
		if (running)
		{
			if (dying)
			{
				PlayAnimation("LostHeadRun");
				animator.AnimationCompleted = AnimatorDelegate;
			}
			else
			{
				PlayAnimation("NormalRun1");
			}
			myTransform.Translate(- Vector3.right * Time.deltaTime * currentMoveSpeed);
		}
		else if (attacking)
		{
			if (plant != null)
			{
				if (dying)
				{
					PlayAnimation("LostHeadAttack");
					animator.AnimationCompleted = AnimatorDelegate;
				}
				else
				{
					PlayAnimation("NormalAttack");
				}
			}
			else
			{
				attacking = false;
				running = true;
			}
		}
	}
	
	void PlayAnimation(string animation)
	{
		if (! animator.IsPlaying(animation))
		{
			animator.Play(animation);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Bullet"))
		{
			if (--life <= 0 && dying == false)
			{
				tk2dSprite head = Instantiate(Resources.Load("ZombieHead", typeof(tk2dSprite)), 
				new Vector3(myTransform.position.x + 2.3f, myTransform.position.y - 0.3f, myTransform.position.z), 
					Quaternion.identity) as tk2dSprite;
				if (slow)
				{
					head.color = Color.blue;
				}
				Destroy(rigidbody);
				dying = true;
			}
			return;
		}
		else if (other.CompareTag("SnowBullet"))
		{
			slowDelta = 0.0f;
			slow = true;
			if (--life <= 0 && dying == false)
			{
				tk2dSprite head = Instantiate(Resources.Load("ZombieHead", typeof(tk2dSprite)), 
					new Vector3(myTransform.position.x + 2.3f, myTransform.position.y - 0.3f, myTransform.position.z), 
					Quaternion.identity) as tk2dSprite;
				if (slow)
				{
					head.color = Color.blue;
				}
				Destroy(rigidbody);
				dying = true;
			}
			return;
		}
		else if (other.CompareTag("Plant"))
		{
			attacking = true;
			running = false;
			plant = other;
			return;
		}
	}
	
	void AnimatorDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		if (dead)
		{
			Destroy(this.gameObject);
		}
		else if (dying)
		{
			attacking = false;
			running = false;
			dead = true;
		}
	}
}
