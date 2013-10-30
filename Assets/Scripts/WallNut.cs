using UnityEngine;
using System.Collections;

public class WallNut : Plant {
	
	private tk2dSpriteAnimator animator;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		animator = spriteTransform.GetComponent<tk2dSpriteAnimator>();
	}

	// Update is called once per frame
	void Update () {
		
		if (life >= 20)
		{
			animator.Play("WallNutNormal");
		}
		else if (life >= 10)
		{
			animator.Play("WallNutBreak1");
		}
		else
		{
			animator.Play("WallNutBreak2");
		}
	}
}
