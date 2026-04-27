namespace Scheduler
{
    public int MaxNonOverlappingIntervals(int[][] intervals)
    {
        Array.Sort (intervals, (a, b)) => a[1].CompareTo(b[1])); // Sort intervals by end time

        int count = 0;
        int lastEndTime = int.MinValue;

        foreach (var interval in intervals)
        {
            int start = interval[0];
            int end = interval[1];

            if (start >= lastEnd)
            {
                count++;
                lastEnd = end;
            }
        }

        return count;
    }
}
