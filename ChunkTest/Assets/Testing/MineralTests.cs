using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

class MineralTests
{

    // Cannot unit test for random spawn of coal/iron

    [UnityTest]
    public IEnumerator CoalMineral_SpawnLayer_Expect28()
    {
        yield return null;
        Assert.AreEqual(28, Mineral.getSpawnLayer(Mineral.Type.Coal));
    }

    [UnityTest]
    public IEnumerator CoalMineral_Size_Between4and14()
    {
        yield return null;
        int coalSize = Mineral.getRandomSize(Mineral.Type.Coal);
        Assert.GreaterOrEqual(coalSize, 4);
        Assert.Less(coalSize, 14);
    }

    [UnityTest]
    public IEnumerator CoalMineral_getBlock_Expect_CoalBlock()
    {
        yield return null;
        Assert.AreEqual("CoalBlock", Mineral.getBlock(Mineral.Type.Coal));
    }

    [UnityTest]
    public IEnumerator IronMineral_SpawnLayer_Expect16()
    {
        yield return null;
        Assert.AreEqual(16, Mineral.getSpawnLayer(Mineral.Type.Iron));
    }

    [UnityTest]
    public IEnumerator IronMineral_Size_Between2and10()
    {
        yield return null;
        int ironSize = Mineral.getRandomSize(Mineral.Type.Iron);
        Assert.GreaterOrEqual(ironSize, 2);
        Assert.Less(ironSize, 10);
    }

    [UnityTest]
    public IEnumerator IronMineral_getBlock_Expect_IronBlock()
    {
        yield return null;
        Assert.AreEqual("IronBlock", Mineral.getBlock(Mineral.Type.Iron));
    }
}

