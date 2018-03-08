using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMethods
{
	public static float div = (float) Chunk.CHUNK_SIZE;

	public HelperMethods ()
	{
	}

	public static Vector3 worldPositionToChunkPosition(Vector3 worldPosition) {
		float originX = Mathf.Floor(worldPosition.x / div) * div;

		float originY = Mathf.Floor(worldPosition.y / div) * div;

		float originZ = Mathf.Floor(worldPosition.z / div) * div;

		return new Vector3 (originX, originY, originZ);
	}
		
	public static Vector3 vectorDifference(Vector3 vector1, Vector3 vector2) {
		Vector3 diff = vector1 - vector2;

		diff.x = Mathf.Abs (diff.x);
		diff.y = Mathf.Abs (diff.y);
		diff.z = Mathf.Abs (diff.z);

		return diff;
	}
}

