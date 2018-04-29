using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

class ContainerTests
{
    GameObject gameObjectWithContainerName;
    Container container = null;
    string containerName = "My Test Container";

    Block block0 = new Block
    {
        resourceString = "Block Type 0"
    };
    Block block1 = new Block
    {
        resourceString = "Block Type 1"
    };
    Block block2 = new Block
    {
        resourceString = "Block Type 2"
    };

    [SetUp]
    public void Setup()
    {
        gameObjectWithContainerName = new GameObject();
        gameObjectWithContainerName.name = containerName;
        gameObjectWithContainerName.AddComponent<UnityEngine.UI.Image>();
    }

    [TearDown]
    public void TearDown()
    {
        container = null;
        gameObjectWithContainerName = null;
    }

    [UnityTest]
    public IEnumerator containerWithSize1AndNoObjects_IsNotFull()
    {
        container = new Container(1, containerName, gameObjectWithContainerName);
        yield return null;
        Assert.IsFalse(container.isFull());
    }

    [UnityTest]
    public IEnumerator containerWithSize1And1Object_IsNotFull()
    {
        container = new Container(1, containerName, gameObjectWithContainerName);
        container.addItem(block0);
        yield return null;
        Assert.IsFalse(container.isFull());
    }

    [UnityTest]
    public IEnumerator containerWithSize1And64Objects_IsFull()
    {
        container = new Container(1, containerName, gameObjectWithContainerName);

        for (int i = 0; i < 64; i++)
        {
            container.addItem(block0);
        }
        yield return null;
        Assert.IsTrue(container.isFull());
    }

    [UnityTest]
    public IEnumerator containerWithSize1And1Object_CanAddOtherItemsOfSameType()
    {
        container = new Container(1, containerName, gameObjectWithContainerName);
        container.addItem(block0);
        yield return null;
        Assert.IsTrue(container.addItem(block0));
    }

    [UnityTest]
    public IEnumerator containerWithSize1And1Object_CannotAdd65ItemsToStack()
    {
        container = new Container(1, containerName, gameObjectWithContainerName);
        yield return null;

        for (int i = 0; i < 64; i++)
        {
            Assert.IsTrue(container.addItem(block0));
        }
        Assert.IsFalse(container.addItem(block0));
    }

    [UnityTest]
    public IEnumerator containerWithSize1And1Object_CannotAddOtherItemsOfDifferentType()
    {
        container = new Container(1, containerName, gameObjectWithContainerName);
        container.addItem(block0);
        yield return null;
        Assert.IsFalse(container.addItem(block1));
    }

    [UnityTest]
    public IEnumerator containerWithItem_GetItem0_ReturnsItem0()
    {
        container = new Container(1, containerName, gameObjectWithContainerName);
        container.addItem(block0);
        yield return null;
        Assert.AreEqual(block0.resourceString, container.getItem(0).resourceString);
    }

    [UnityTest]
    public IEnumerator containerWithNoItems_GetItem0_ReturnsNull()
    {
        container = new Container(1, containerName, gameObjectWithContainerName);
        yield return null;
        Assert.IsNull(container.getItem(0));
    }
}
