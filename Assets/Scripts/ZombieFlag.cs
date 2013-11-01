using UnityEngine;
using System.Collections;

public class ZombieFlag : Zombie {
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
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
				PlayAnimation("FlagLostHeadRun");
				animator.AnimationCompleted = AnimatorDelegate;
			}
			else
			{
				PlayAnimation("FlagRun");
			}
			myTransform.Translate(- Vector3.right * Time.deltaTime * currentMoveSpeed);
		}
		else if (attacking)
		{
			if (plant != null)
			{
				if (dying)
				{
					PlayAnimation("FlagLostHeadAttack");
					animator.AnimationCompleted = AnimatorDelegate;
				}
				else
				{
					PlayAnimation("FlagAttack");
				}
			}
			else
			{
				attacking = false;
				running = true;
			}
		}
	}
}
