using UnityEngine;
using System.Collections;

public class SunFlower : Plant {
	
	public float minSpawnSunTime =  8.0f;
	public float maxSpawnSunTime = 15.0f;
	
	// Use this for initialization
	protected override void Start (){
		base.Start();
		StartCoroutine(SpawnSun());
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	private IEnumerator SpawnSun()
	{
		while (true)
		{
			float delta = 0.0f;
			float coolDown = Random.Range(minSpawnSunTime, maxSpawnSunTime);
			while (delta <= coolDown)
			{
				delta += Time.deltaTime;
				yield return null;
			}
			GameObject sun = Instantiate(Resources.Load("Sun", typeof(GameObject)), new Vector3(myTransform.position.x, myTransform.position.y, 5.0f), Quaternion.identity) as GameObject;
			sun.transform.FindChild("Sprite").GetComponent<Sun>().SpawnByWorld = false;
			yield return null;
		}
	}
}
