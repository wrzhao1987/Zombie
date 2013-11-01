using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlantType
{
	Sunflower = 1,
	Peashooter,
	SnowPeashooter,
	PotatoMine,
	WallNut,
	NotDefined,
}

public enum ZombieType
{
	NormalZombie = 1,
	ConeHeadZombie,
	BucketHeadZombie,
	FlagZombie,
}

public enum GameStage
{
	NotStarted = 0,
	Preparing,
	NormalWave,
	FinalWave,
	Ended,
}


public class AxisManager
{
	static public List<float> XAxis = new List<float>()
	{
		15.02f, 18.81f, 23.12f, 27.31f, 31.70f, 35.68f, 39.36f, 43.28f, 47.54f,
	};
	
	static public List<float> YAxis = new List<float>()
	{
		10.0f, 15.0f, 20.0f, 25.0f, 30.0f,
	};
	
	static public List<float> ZAxis = new List<float>()
	{
		5.0f, 6.0f, 7.0f, 8.0f, 9.0f,	
	};
	
	static public float GetXAxis(int x)
	{
		float xAxis = 0.0f;
		if (! (x > 9 || x < 0))
		{
			xAxis = XAxis[x];
		}
		return xAxis;
	}
	
	static public float GetYAxis(int y)
	{
		float yAxis = 0.0f;
		if (! (y > 5 || y < 0))
		{
			yAxis = YAxis[y];
		}
		return yAxis;
	}
	
	static public float GetZAxis(int y)
	{
		float zAxis = 0.0f;
		if (! (y > 5 || y < 0))
		{
			zAxis = ZAxis[y];
		}
		return zAxis;
	}
}

public class ManagerDefine
{
	static private Dictionary<PlantType, string> plantSpriteName = new Dictionary<PlantType, string>()
	{
		{PlantType.Peashooter, "Peashooter01"}, {PlantType.PotatoMine, "PotatoMine01"}, 
		{PlantType.SnowPeashooter, "SnowPea01"}, {PlantType.Sunflower, "SunFlower01"},
		{PlantType.WallNut, "WallNut01"},
	};

	static private Dictionary<PlantType, int> plantSunNeed = new Dictionary<PlantType, int>()
	{
		{PlantType.Peashooter, 100}, {PlantType.PotatoMine, 25}, {PlantType.SnowPeashooter, 175}, {PlantType.Sunflower, 50},
		{PlantType.WallNut, 50},
	};
	
	static public string GetSpriteNameByPlantType(PlantType plantType)
	{
		string spriteName = "";
		if (plantSpriteName.ContainsKey(plantType))
		{
			plantSpriteName.TryGetValue(plantType, out spriteName);
		}
		return spriteName;
	}

	static public int GetSunNeedByPlantType(PlantType plantType)
	{
		int sunNeed = 0;
		if (plantSunNeed.ContainsKey(plantType))
		{
			plantSunNeed.TryGetValue(plantType, out sunNeed);
		}
		return sunNeed;
	}
}
