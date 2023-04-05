using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMaterialManager : MonoBehaviour
{
    public Material[] colors;
    public int Colorindexer(Material mat)
    {
        for (int i = 1; i < colors.Length; i++)
        {
            if (mat.GetColor("_OldColor") == colors[i].GetColor("_OldColor"))
                return i;
        }
        return 0;
    }

    public int OppositeColor(int color)
    {
        switch(color)
        {
            case 1:
                return 0;
            case 2:
                return 7;
            case 3:
                return 6;
            case 4:
                return 5;
            case 5:
                return 4;
            case 6:
                return 3;
            case 7:
                return 2;
            case 8:
                return 1;
            case 9:
                return 9;
        }
        return 0;
    }

    public int ColorComparer(int color1, int color2)
    {
        switch (color1)
        {
            case 1:
                return 0;
            case 2:
                if (color2 == 1)
                    return color1;
                if (color2 != 5 && color2 != 6 && color2 != color1)
                    return color1 + color2;
                break;
            case 3:
                if (color2 == 1)
                    return color1;
                if (color2 != 5 && color2 != 7 && color2 != color1)
                    return color1 + color2;
                break;
            case 4:
                if (color2 == 1)
                    return color1;
                if (color2 != 6 && color2 != 7 && color2 != color1)
                    return color1 + color2;
                break;
            case 5:
                if (color2 == 1)
                    return color1;
                if (color2 != 2 && color2 != 3 && color2 != 6 && color2 != 7 && color2 != color1)
                    return color1 + color2;
                break;
            case 6:
                if (color2 == 1)
                    return color1;
                if (color2 != 2 && color2 != 4 && color2 != 5 && color2 != 7 && color2 != color1)
                    return color1 + color2;
                break;
            case 7:
                if (color2 == 1)
                    return color1;
                if (color2 != 3 && color2 != 4 && color2 != 5 && color2 != 6 && color2 != color1)
                    return color1 + color2;
                break;

        }
        return 0;
    }
}
