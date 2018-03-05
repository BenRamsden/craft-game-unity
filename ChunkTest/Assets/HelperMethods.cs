using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMethods
{
	public HelperMethods ()
	{
	}

	public static Vector3 worldPositionToChunkPosition(Vector3 worldPosition) {
		float originX = worldPosition.x;
		originX -= originX % Chunk.CHUNK_SIZE;

		float originY = worldPosition.y;
		originY -= originY % Chunk.CHUNK_SIZE;

		float originZ = worldPosition.z;
		originZ -= originZ % Chunk.CHUNK_SIZE;

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

