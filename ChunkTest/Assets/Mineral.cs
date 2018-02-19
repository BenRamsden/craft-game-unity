using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineral
{
	//
	// Coal
	//
	private static int coalSpawnChance = 45;
	private static int coalSizeChanceLower = 4;
	private static int coalSizeChanceHigher = 14;
	private static int coalSpawnLayer = 28;

	private static int ironSpawnChance = 35;
	private static int ironSizeChanceLower = 2;
	private static int ironSizeChanceHigher = 10;
	private static int ironSpawnLayer = 16;

	public enum Type
	{
		Coal, Iron
	}

	public static bool hasSpawnChance (Type type)
	{
		switch (type)
		{
		case Type.Coal:
			return Random.Range (1, 100) < coalSpawnChance == true;
		case Type.Iron:
			return Random.Range (1, 100) < ironSpawnChance == true;
		default:
			return false;
		}
	}

	public static int getSpawnLayer(Type type)
	{
		switch (type)
		{
		case Type.Coal:
			return coalSpawnLayer;
		case Type.Iron:
			return ironSpawnLayer;
		default:
			return 0;
		}
	}

	public static int getRandomSize(Type type)
	{
		switch (type)
		{
		case Type.Coal:
			return Random.Range (coalSizeChanceLower, coalSizeChanceHigher);
		case Type.Iron:
			return Random.Range (ironSizeChanceLower, ironSizeChanceHigher);
		default:
			return 0;
		}
	}

	public static string getBlock(Type type)
	{
		switch (type)
		{
		case Type.Coal:
			return "CoalBlock";
		case Type.Iron:
			return "IronBlock";
		default:
			return "";
		}
	}
}