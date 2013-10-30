using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	
	static public Manager Instance;
	public Camera gameCam;
	
	public int sun = 200;
	public float prepareTime = 20.0f;
	public int normalWaveZombieNum = 20;
	public int finalWaveZombieNum = 20;

	private bool planting;
	private bool removing;
	
	private GameStage currentGameStage;
	
	private PlantType plantingType = PlantType.NotDefined;
	private tk2dSprite plantingSprite;
	private Vector2 currentMatrix;
	private bool positionSelected;
	private PlantCard currentPlantCard;
	
	private tk2dTextMesh sunAmount;
	
	public bool Planting
	{
		get{ return planting; }
		set{ planting = value; }
	}
	
	public int Sun
	{
		get { return sun; }
		set { sun = value; }
	}
	
	public PlantType PlantingType
	{
		get { return plantingType; }
		set { plantingType = value; }
	}
	
	public PlantCard CurrentPlantCard
	{
		get { return currentPlantCard; }
		set { currentPlantCard = value; }
	}
	
	void Awake()
	{
		Instance = this;
	}
	// Use this for initialization
	void Start () 
	{
		sunAmount = GameObject.FindGameObjectWithTag("SunAmount").GetComponent<tk2dTextMesh>();
		sunAmount.text = sun.ToString();
		sunAmount.Commit();
		currentGameStage = GameStage.NotStarted;
		MainLoop();
	}
	
	private void MainLoop()
	{
		GameObject sunFlowerCard = GameObject.Find("PlantCardSunFlower");
		sunFlowerCard.GetComponent<PlantCard>().CanBeSelected = true;

		StartCoroutine(SwitchGameStage());
		StartCoroutine(SpawnSun());
	}
	// Update is called once per frame
	void Update () 
	{
		UpdateSunAmount();
		UpdatePlanting();
	}

	private IEnumerator SwitchGameStage()
	{
		float prepareDelta = 0.0f;
		while (true)
		{
			switch (currentGameStage)
			{
			case GameStage.NotStarted:
				currentGameStage = GameStage.Preparing;
				Debug.Log("Enter Prepare Stage");
				break;
			case GameStage.Preparing:
				while (prepareDelta <= prepareTime)
				{
					prepareDelta += Time.deltaTime;
					yield return null;
				}
				currentGameStage = GameStage.NormalWave;
				Debug.Log("Enter Normal Wave Stage");
				break;
			case GameStage.NormalWave:
				SpawnZombie();
				if (normalWaveZombieNum <= 0)
				{
					currentGameStage = GameStage.FinalWave;
					Debug.Log("Enter Final Wave Stage");
				}
				else
				{
					yield return new WaitForSeconds(10.0f);
				}
				break;
			case GameStage.FinalWave:
				SpawnZombie();
				if (finalWaveZombieNum <= 0)
				{
					currentGameStage = GameStage.Ended;
				}
				break;
			}
			yield return null;
		}
	}

	private IEnumerator SpawnSun()
	{
		while (! currentGameStage.Equals(GameStage.Ended))
		{
			float delta = 0.0f;
			float coolDown = Random.Range(8.0f, 11.0f);
			while (delta <= coolDown)
			{
				delta += Time.deltaTime;
				yield return null;
			}
			float genX = Random.Range(AxisManager.XAxis[0], AxisManager.XAxis[AxisManager.XAxis.Count - 1]);
			Instantiate(Resources.Load("Sun", typeof(GameObject)), new Vector3(genX, 32.0f, 1.0f), Quaternion.identity);
			yield return null;
		}
		
	}
	
	private void UpdateSunAmount()
	{
		if (! sunAmount.text.Equals(sun.ToString()))
		{
			sunAmount.text = sun.ToString();
			sunAmount.Commit();
		}
	}
	
	void UpdatePlanting()
	{
		Vector3 mousePos = gameCam.ScreenToWorldPoint(Input.mousePosition);

		if (positionSelected &&  Input.GetMouseButtonDown(0) && planting == true && plantingType != PlantType.NotDefined)
		{
			if (sun >= ManagerDefine.GetSunNeedByPlantType(plantingType))
			{
				string prefabName = "";
				switch (plantingType)
				{
				case PlantType.Peashooter:
					prefabName = "Peashooter";
					break;
				case PlantType.Sunflower:
					prefabName = "SunFlower";
					break;
				case PlantType.SnowPeashooter:
					prefabName = "SnowPeashooter";
					break;
				case PlantType.WallNut:
					prefabName = "WallNut";
					break;
				default:
					prefabName = "SunFlower";
					break;
				}
				Instantiate(Resources.Load(prefabName), new Vector3(AxisManager.GetXAxis((int)currentMatrix.x), 
				                                                    AxisManager.GetYAxis((int)currentMatrix.y), AxisManager.GetZAxis((int)currentMatrix.y)), Quaternion.identity);
				sun -= ManagerDefine.GetSunNeedByPlantType(plantingType);
				
				
				planting = false;
				plantingType = PlantType.NotDefined;
				positionSelected = false;
				CurrentPlantCard.CanBeSelected = false;
				
				Destroy(plantingSprite.gameObject);
			}
		}
		else if (Input.GetMouseButtonDown(1) && planting == true && plantingType != PlantType.NotDefined)
		{
			Destroy(plantingSprite.gameObject);
			planting = false;
			plantingType = PlantType.NotDefined;
			positionSelected = false;
		}
		else 
		{
			if (planting == true && plantingType != PlantType.NotDefined)
			{
				// Moving the plant
				if (plantingSprite != null)
				{
					SetCurrentMatrix(mousePos);
					plantingSprite.transform.position = new Vector3(AxisManager.GetXAxis((int)currentMatrix.x), 
						AxisManager.GetYAxis((int)currentMatrix.y) - 1.4f, 
						AxisManager.GetZAxis((int)currentMatrix.y));
				}
				else 
				{
					plantingSprite = Instantiate(Resources.Load("PlantSprite", typeof(tk2dSprite)), 
														    new Vector3(mousePos.x, mousePos.y, 6.0f), 
															Quaternion.identity) as tk2dSprite;
					plantingSprite.SetSprite(ManagerDefine.GetSpriteNameByPlantType(plantingType));
				}
			}
		}
	}
	
	private void SetCurrentMatrix(Vector3 mousePos)
	{
		Vector2 result = Vector2.zero;
		bool xHit = false;
		bool yHit = false;
		
		for (int i = 0; i < AxisManager.XAxis.Count; i++)
		{
			if(AxisManager.GetXAxis(i) - 1.5f <= mousePos.x && mousePos.x <= AxisManager.GetXAxis(i) + 1.5f)
			{
				xHit = true;
				result.x = i;
				break;
			}
		}
		for (int j = 0; j < AxisManager.YAxis.Count;  j++)
		{
			if(AxisManager.GetYAxis(j) - 2.9f <= mousePos.y && mousePos.y <= AxisManager.GetYAxis(j) + 0.1f)
			{
				yHit = true;
				result.y = j;
				break;
			}
		}
		
		if (xHit && yHit)
		{
			currentMatrix = result;
			positionSelected = true;
		}
	}

	private void SpawnZombie()
	{
		if (currentGameStage.Equals(GameStage.NormalWave))
		{
			int row = Random.Range(0, 5);
			Debug.Log("Spawn Zombie in row" + row);
			float xDelta = Random.Range(0.0f, 3.0f);
			Vector3 genPos = new Vector3(52.0f + xDelta, AxisManager.GetYAxis(row), AxisManager.GetZAxis(row));
			Instantiate(Resources.Load("ZombieNormal"), genPos, Quaternion.identity);
			normalWaveZombieNum--;
		}
		else if (currentGameStage.Equals(GameStage.FinalWave))
		{

		}
	}
}
