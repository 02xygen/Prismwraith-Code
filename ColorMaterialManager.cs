using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMaterialManager : MonoBehaviour
{
    public Material[] colors;

    // Index the input color by referencing the colors array
    public int Colorindexer(Material mat)
    {
        for (int i = 1; i < colors.Length; i++)
        {
            if (mat.GetColor("_OldColor") == colors[i].GetColor("_OldColor"))
                return i;
        }
        return 0;
    }

    // Output the color index of the color opposite the inputed color's index
    public int OppositeColor(int color)
    {
        switch(color)
        {
            case 1: // If white
                return 0; // Return illegal

            case 2: // If red
                return 7; // Return green

            case 3: // If blue
                return 6; // Return orange

            case 4: // If yellow
                return 5; // Return violet

            case 5: // If violet
                return 4; // Return yellow

            case 6: // If orange
                return 3; // Return blue

            case 7: // If green
                return 2; // Return red

            case 8: // If illegal
                return 1; // Return white

            case 9: // If brown
                return 9; // Return brown
        }
        return 0; // Return illegal
    }

    // Compare inputed color's indexes to see if they can mix
    public int ColorComparer(int color1, int color2)
    {
        switch (color1)
        {
            case 1: // Held color is White
                return 0; // Output is null, you can't paint anything white

            case 2: // Held color is red
                if (color2 == 1) // Red + white
                    return color1; // Output red
                if (color2 != 5 && color2 != 6 && color2 != color1) // Red cannot mix with violet, orange, or itself
                    return color1 + color2; // output mixed color index
                break;

            case 3: // Held color is blue
                if (color2 == 1) // if canvas is white, paint it blue
                    return color1;
                if (color2 != 5 && color2 != 7 && color2 != color1) // Blue cannot mix with, violet, green, or itself
                    return color1 + color2; // output mixed color index
                break;

            case 4: // Held color is yellow
                if (color2 == 1) // if canvas is white, paint it yellow
                    return color1;
                if (color2 != 6 && color2 != 7 && color2 != color1) // Yellow cannot mix with orange, green, or itself
                    return color1 + color2; // output mixed color index
                break;

            case 5: // Held color is violet
                if (color2 == 1) // if the canvas is white, paint it violet
                    return color1;
                if (color2 != 2 && color2 != 3 && color2 != 6 && color2 != 7 && color2 != color1) // Violet cannot mix with red, blue, green, orange, or itself
                    return color1 + color2; // output mixed color index
                break;

            case 6: // Held color is orange
                if (color2 == 1) // if the canvas is white, paint it orange
                    return color1;
                if (color2 != 2 && color2 != 4 && color2 != 5 && color2 != 7 && color2 != color1) // Orange cannot mix with red, yellow, green, violet, or itself
                    return color1 + color2; // output mixed color index
                break;

            case 7: // Held color is green
                if (color2 == 1) // If the canvas is white, paint it green
                    return color1;
                if (color2 != 3 && color2 != 4 && color2 != 5 && color2 != 6 && color2 != color1) // Green cannot mix with blue, yellow, orange, violet, or itself
                    return color1 + color2; // output mixed color index
                break;

        }
        return 0; // return illegal color index
    }
}
