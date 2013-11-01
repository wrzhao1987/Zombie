using UnityEngine;
using System.Collections;

public class ZombieConeHead : Zombie {

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
				PlayAnimation("LostHeadRun");
				animator.AnimationCompleted = AnimatorDelegate;
			}
			else
			{
				if (life >= 10)
				{
					PlayAnimation("ConeHeadRun");
				}
				else
				{
					PlayAnimation("NormalRun1");
				}
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
					if (life >= 10)
					{
						PlayAnimation("ConeHeadAttack");
					}
					else
					{
						PlayAnimation("NormalAttack");
					}
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
