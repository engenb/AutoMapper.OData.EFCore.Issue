using System;

namespace ApiModel;

[Flags]
public enum FooType
{
    One =   0b0001,
    Two =   0b0010,
    Three = 0b0100,
    Four =  0b1000
}