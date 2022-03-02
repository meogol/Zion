using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
    public Signal signal { get; set; }
    [Test]
    public void TestPowerSignalWithoutCollision()
    {
        signal = new Signal();
        float distance = 6;
        int collision = 0;
        float radius = 500;
        int received = signal.Power(distance, collision, radius, 1);
        int expect = -14;
        Assert.AreEqual(expect, received);
    }
    [Test]
    public void TestPowerSignalWithOneCollision()
    {
        signal = new Signal();
        float distance = 482.7255f;
        int collision = 1;
        float radius = 500;
        int received = signal.Power(distance, collision, radius, 1);
        int expect = -134;
        Assert.AreEqual(expect, received);
    }
    [Test]
    public void TestPowerSignalWithTwoCollision()
    {
        signal = new Signal();
        float distance = 463.7243f;
        int collision = 2;
        float radius = 500;
        int received = signal.Power(distance, collision, radius, 1);
        int expect = -154;
        Assert.AreEqual(expect, received);
    }
}
