using System;
using System.Diagnostics;

namespace PokeAByte.BizHawk.StpTool.Domain;

public class StpStopwatch : Stopwatch
{
    public TimeSpan StartOffset { get; private set; }

    public void Start(TimeSpan startOffset)
    {
        StartOffset = startOffset;
        base.Start(); 
    }
    
    public new long ElapsedMilliseconds
    {
        get
        {
            return base.ElapsedMilliseconds + (long)StartOffset.TotalMilliseconds;
        }
    }

    public new long ElapsedTicks
    {
        get
        {
            return base.ElapsedTicks + StartOffset.Ticks;
        }
    } 
}