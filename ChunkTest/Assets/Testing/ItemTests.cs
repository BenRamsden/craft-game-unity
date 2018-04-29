using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class ItemTests {
	Block block = null;
    string blockName = "LogBlock";

	[SetUp]
	public void Setup()
    {
		block = new Block ();
		block.resourceString = blockName;
		block.draw();
	}

    [TearDown]
    public void TearDown()
    {
        if (block != null)
        {
            block.Delete();
        }
        block = null;
    }

    [UnityTest]
	public IEnumerator createBlock_ExpectBlockWithCorrectDetails()
    {
		yield return null;
        GameObject testBlock = GameObject.Find(blockName);
        Assert.NotNull(testBlock);
        Assert.AreEqual(blockName, testBlock.name);

    }

	[UnityTest]
	public IEnumerator createWrongBlockTest_ExpectNull()
    {
		yield return null;
		Assert.Null(GameObject.Find("Not_A_Block"));
	}

	[UnityTest]
	public IEnumerator blockChuckPositionSet1_2_3_ExpectBlockChuckX1ChuckZ3()
    {
		block.setChunkPosition(1, 2, 3);
		yield return null;
        Assert.AreEqual(1, block.getChunkX());
        Assert.AreEqual(3, block.getChunkZ());
	}

    [UnityTest]
    public IEnumerator blockWorldPositionSet1_2_3_ExpectBlockWorldPositionProperty1_2_3()
    {
        block.setPosition(1, 2, 3);
        yield return null;
        Vector3 blockPosition = block.getPosition();
        Assert.AreEqual(1.0f, blockPosition.x);
        Assert.AreEqual(2.0f, blockPosition.y);
        Assert.AreEqual(3.0f, blockPosition.z);
    }

    [UnityTest]
    public IEnumerator blockHealth100_Damage50_ExpectBlockHealth50()
    {
        block.damageBlock(50);

        yield return null;
        Assert.AreEqual(50, block.getProperties().blockHealth);
    }

    [UnityTest]
    public IEnumerator blockHealth100_Damage150_ExpectBlockHealthNegative50()
    {
        block.damageBlock(150);

        yield return null;
        Assert.AreEqual(-50, block.getProperties().blockHealth);
    }

    [UnityTest]
	public IEnumerator dropBlock_ExpectBlock()
    {
		block.dropSelf();

		yield return null;
		Assert.NotNull(GameObject.Find(blockName).GetComponent<Rigidbody>());
	}

	[UnityTest]
	public IEnumerator destroyBlock_ExpectNullBlock()
    {
		block.Delete();

		yield return new WaitForSeconds(2.0f);
		Assert.Null(GameObject.Find(blockName));
	}
}
