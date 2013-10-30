using UnityEngine;
using System.Collections;

public class SnowPeashooter : Plant {
	
	public float bulletCoolDown = 1.5f;
	private bool existsZombie = false;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		StartCoroutine(ExistsZombie());
		StartCoroutine(ShootBullet());
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	private IEnumerator ExistsZombie()
	{
		while (true)
		{
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(myTransform.position, Vector3.right, out hit))
			{
				if (hit.transform.CompareTag("Zombie"))
				{
					existsZombie = true;
				}
				else
				{
					existsZombie = false;
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
	private IEnumerator ShootBullet()
	{
		while (true)
		{
			if (existsZombie)
			{
				yield return new WaitForSeconds(bulletCoolDown);
				Instantiate(Resources.Load("SnowBullet"), myTransform.position, Quaternion.identity);
			}
			yield return null;
		}
	}
}
