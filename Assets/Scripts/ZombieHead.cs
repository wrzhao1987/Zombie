using UnityEngine;
using System.Collections;

public class ZombieHead : MonoBehaviour {
	
	private tk2dSpriteAnimator animator;
	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<tk2dSpriteAnimator>();
		animator.Play("Head");
		animator.AnimationCompleted = AnimatorDelegate;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void AnimatorDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		Destroy(gameObject);
	}
}
