using UnityEngine;
using System.Collections;

public class PlantCard : MonoBehaviour {
	
	public PlantType plantType;
	public float coolDown;
	private Transform myTransform;
	private tk2dSprite image;
	private tk2dClippedSprite mask;
	
	private bool canBeSelected = false;	
	public bool CanBeSelected
	{
		get {return canBeSelected; }
		set { canBeSelected = value; }
	}
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		image = myTransform.FindChild("Image").GetComponent<tk2dSprite>();
		mask = myTransform.FindChild("Mask").GetComponent<tk2dClippedSprite>();

		switch (plantType)
		{
			case PlantType.Sunflower:
				image.SetSprite("SunFlower");
				break;
			case PlantType.Peashooter:
				image.SetSprite("Peashooter");
				break;
			case PlantType.SnowPeashooter:
				image.SetSprite("SnowPea");
				break;
			case PlantType.PotatoMine:
				image.SetSprite("PotatoMine");
				break;
			case PlantType.WallNut:
				image.SetSprite("WallNut");
				break;
			default:
				break;
		}
		if (canBeSelected)
		{
			image.color = Color.white;
			mask.ClipRect = new Rect(0, 1, 1, 1);
		}
		StartCoroutine(UpdateMask());
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	private IEnumerator UpdateMask()
	{
		while (true)
		{
			if (! canBeSelected)
			{
				float delta = 0.0f;
				while (delta <= coolDown)
				{
					delta += Time.deltaTime;
					float ratio = delta / coolDown;
					if (ratio >= 1.0f)
					{
						canBeSelected = true;
						image.color = Color.white;
						mask.ClipRect = new Rect(0, 1, 1, 1);
						yield return null;
					}
					else
					{
						image.color = Color.grey;
						mask.ClipRect = new Rect(0, ratio, 1, 1);
						yield return null;
					}
				}
			}
			yield return null;
		}
	}
	
	void OnMouseDown()
	{
		if (canBeSelected)
		{
			if (Manager.Instance.Planting == false)
			{
				Manager.Instance.PlantingType = plantType;
				Manager.Instance.Planting = true;
				Manager.Instance.CurrentPlantCard = this;
			}
		}
	}
}
