using System.Collections.Generic;
using System.Linq;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class GapFillerTests
{
    private readonly IIntervalProcesser _subject;

    public GapFillerTests()
    {
        _subject = new IntervalProcesser();
    }

    [Test]
    public void TestGapFillingWithGapBetweenTwoIntervals()
    {
        // [0:0:0, 0:4:999]
        // [0:5:0, 1:6:0]
        // [1:6:1, 9:9:999]
        var tickA = new Tick(0, 0, 0);
        var tickB = new Tick(0, 5, 0);
        var tickC = new Tick(1, 6, 1);
        var tickD = new Tick(10, 0, 0);
        var interval1 = new Interval(tickA, tickB);
        var interval2 = new Interval(tickC, tickD);
        var intervals = new List<Interval> { interval1, interval2 };
        var filled = _subject.ProcessIntervals(intervals);
        Assert.AreEqual(3, filled.Count);
        Assert.AreEqual(4999, filled[0].LengthMilisseconds);
        Assert.AreEqual(61000, filled[1].LengthMilisseconds);
        Assert.AreEqual(999 + 53 * 1000 + 8 * 1000 * 60 - 1, filled[2].LengthMilisseconds);
    }

    [Test]
    public void TestGapFillingWithoutGap()
    {
        // 0:0:0
        // 11:11:10
        var tickA = new Tick(0, 0, 0);
        var tickB = new Tick(11, 11, 11);
        var interval = new Interval(tickA, tickB);
        var filled = _subject.ProcessIntervals(new List<Interval> { interval });
        Assert.AreEqual(1, filled.Count);
        var uniqueFrame = filled.First();
        Assert.AreEqual(11 * 60000 + 11 * 1000 + 10, uniqueFrame.LengthMilisseconds);
    }
}