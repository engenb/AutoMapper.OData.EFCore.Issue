using System;

namespace Domain.Enumerations;

[Flags]
public enum BarType
{
    One =   0b0001,
    Two =   0b0010,
    Three = 0b0100,
    Four =  0b1000
}