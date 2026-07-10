using System;

public class ManaReward
{
    internal ManaReward(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Value = value;
    }

    internal int Value { get; private set; }
}
