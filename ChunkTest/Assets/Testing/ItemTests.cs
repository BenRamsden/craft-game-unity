using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class ItemTests {
	Block block = null;

	[SetUp]
	public void Setup(){
		if (block != null) {
			block.Delete();
		}
		block = new Block ();
		block.resourceString = "LogBlock";
		block.draw();
	}

	[UnityTest]
	public IEnumerator createBlockTest() {
		yield return null;
		Assert.True(GameObject.Find("LogBlock") != null);
	}

	[UnityTest]
	public IEnumerator createWrongBlockTest() {
		yield return null;
		Assert.True(GameObject.Find("WaterBlock") == null);
	}

	[UnityTest]
	public IEnumerator blockPositionTest() {
		block.setChunkPosition(7,4,3);
		yield return null;
		Assert.True(block.getChunkX() == 7 && block.getChunkZ() == 3);
	}

	[UnityTest]
	public IEnumerator damageBlockTest(){
		yield return null;
		int previousHealth = block.getProperties().blockHealth;
		block.damageBlock(10);
		Assert.AreNotEqual (previousHealth, block.getProperties ().blockHealth);
	}

	[UnityTest]
	public IEnumerator dropBlockTest() {
		block.dropSelf();
		yield return null;
		Assert.True(GameObject.Find("LogBlock").GetComponent<Rigidbody>() != null);
	}

	[UnityTest]
	public IEnumerator destroyBlockTest() {
		block.Delete();
		yield return new WaitForSeconds(2.0f);
		Assert.True(GameObject.Find("LogBlock") == null);
	}
}
