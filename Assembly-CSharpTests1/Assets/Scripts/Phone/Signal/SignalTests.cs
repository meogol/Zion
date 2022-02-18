using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StarterAssets;

[TestClass()]
public class SignalTests
{
    public Signal signal { get; set; }

    [TestMethod()]
    public void PowerTest()
    {
        int expected = -90;

        signal = new Signal();
        int n = Convert.ToInt32(signal.Power(100));
        Assert.AreEqual(expected, n);
    }
}