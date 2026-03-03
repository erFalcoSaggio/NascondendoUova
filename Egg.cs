using System;
using System.Drawing;
using Microsoft.VisualBasic;

namespace Nascondendo_Uova;

public class Egg
{
    public (Color, Color) colors;
    public Egg((Color, Color) color) { colors = color; }
}
