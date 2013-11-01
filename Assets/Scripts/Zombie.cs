using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {

	public int life;
	public float moveSpeed;
	protected float currentMoveSpeed;

	protected Transform myTransform;
	protected Transform spriteTransform;
	protected tk2dSprite sprite;
	protected tk2dSpriteAnimator animator;
	protected float animatorFPS;
	protected float currentAnimatorFPS;

	protected Collider plant;

	protected bool slow;
	protected float slowDelta;
	protected float slowDuration = 3.0f;

	protected bool attacking = false;
	protected bool running = true;
	protected bool dying = false;
	protected bool dead = false;
	
	// Use this for initialization
	protected virtual void  Start ()
	{
		myTransform = transform;
		spriteTransform = myTransform.FindChild("Sprite");
		sprite = spriteTransform.GetComponent<tk2dSprite>();
		animator = spriteTransform.GetComponent<tk2dSpriteAnimator>();
		animatorFPS = animator.ClipFps;
		currentMoveSpeed = moveSpeed;

		StartCoroutine(AttackPlant());
		StartCoroutine(SlowDown());
	}

	protected IEnumerator AttackPlant()
	{
		while (true)
		{
			if (plant != null && attacking)
			{
				plant.GetComponent<Plant>().Life--;
				yield return new WaitForSeconds(1.5f);
			}
			yield return null;
		}
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
	protected virtual void  Update ()
	{
	
	}

	protected void AnimatorDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		if (dead)
		{
			Destroy(gameObject);
		}
		else if (dying)
		{
			attacking = false;
			running = false;
			dead = true;
		}
	}

	protected void OnTriggerEnter(Collider other)
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

	protected void PlayAnimation(string animation)
	{
		if (! animator.IsPlaying(animation))
		{
			animator.Play(animation);
		}
	}
}
