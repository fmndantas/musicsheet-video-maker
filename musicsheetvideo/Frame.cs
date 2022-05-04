namespace musicsheetvideo;

public class Frame : IComparable
{
    public Interval Interval { get; }
    public int PageNumber { get; }

    public Frame(Interval interval, int pageNumber)
    {
        Interval = interval;
        PageNumber = pageNumber;
    }

    public Frame Gap(Frame nextFrame)
    {
        return new Frame(Interval.Gap(nextFrame.Interval), -1);
    }

    public long LengthMilisseconds => Interval.LengthMilisseconds;

    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 0;
        }

        var otherFrame = obj as Frame;
        if (otherFrame == null)
        {
            throw new Exception("Object is not a Frame");
        }

        return Interval.CompareTo(otherFrame.Interval);
    }
}