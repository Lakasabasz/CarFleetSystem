namespace CarFleetManager.models;

public class Color
{
    public int A { get; set; }
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }

    public static Color Convert(System.Drawing.Color color) => 
        new() { A = color.A, B = color.B, G = color.G, R = color.R };

    public System.Drawing.Color GetColor() => System.Drawing.Color.FromArgb(A, R, G, B);
}