﻿using UnityEngine;
using System.Collections;

public class ZombieNormal : Zombie {

	private string normalRunType;
	// Use this for initialization
	protected override void Start ()
	{
		base.Start();
		normalRunType = "NormalRun" + Random.Range(1, 4);
	}
	// Update is called once per frame
	protected override void Update()
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
				PlayAnimation(normalRunType);
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
}
