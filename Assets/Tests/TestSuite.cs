using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
    public Signal signal = new Signal();
    private float distance;
    private int collision;
    private int radius = 500;
    private int received;
    private int expect;
    private int countUsers;
    private int[] CountTime = new int[10] {50, 45, 67, 83, 15, 72, 34, 60, 82, 46};
    [Test]
    public void TestPowerSignalWithoutCollision()
    {
        distance = 6;
        collision = 0;
        countUsers = 1;
        signal.ChangeSignal(distance, collision, radius, 1);
        received = signal.power;
        expect = -14;
        Assert.AreEqual(expect, received);
    }
    [Test]
    public void TestPowerSignalWithOneCollision()
    {
        distance = 482.7255f;
        collision = 1;
        countUsers = 1;
        signal.ChangeSignal(distance, collision, radius, countUsers);
        received = signal.power;
        expect = -134;
        Assert.AreEqual(expect, received);
    }
    [Test]
    public void TestPowerSignalWithTwoCollision()
    {
        distance = 463.7243f;
        collision = 2;
        countUsers = 1;
        signal.ChangeSignal(distance, collision, radius, countUsers);
        received = signal.power;
        expect = -154;
        Assert.AreEqual(expect, received);
    }
    [Test]
    public void TestPackLossOneUser()
    {
        countUsers = 1;
        for (int i=0; i<10; i++)
        {
            received = signal.PacketLoss(CountTime[i], countUsers);
        }
        expect = 2;
        Assert.AreEqual(expect, received);
    }
    [Test]
    public void TestPackLossOneHundredUser()
    {
        countUsers = 100;
        for (int i = 0; i < 10; i++)
        {
            received = signal.PacketLoss(CountTime[i], countUsers);
        }
        expect = 41;
        Assert.AreEqual(expect, received);
    }
}
