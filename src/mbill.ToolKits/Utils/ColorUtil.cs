namespace mbill.ToolKits.Utils;

public static class ColorUtil
{
    public static string GetRandomColor()
    {
        Random randomNum_1 = new Random(Guid.NewGuid().GetHashCode());
        System.Threading.Thread.Sleep(randomNum_1.Next(1));
        int int_Red = randomNum_1.Next(255);

        Random randomNum_2 = new Random((int)DateTime.Now.Ticks);
        int int_Green = randomNum_2.Next(255);

        Random randomNum_3 = new Random(Guid.NewGuid().GetHashCode());

        int int_Blue = randomNum_3.Next(255);
        int_Blue = (int_Red + int_Green > 380) ? int_Red + int_Green - 380 : int_Blue;
        int_Blue = (int_Blue > 255) ? 255 : int_Blue;


        return GetDarkerColor(int_Red, int_Green, int_Blue);
    }

    //获取加深颜色
    private static string GetDarkerColor(int r, int g, int b)
    {
        const int max = 255;
        int increase = new Random(Guid.NewGuid().GetHashCode()).Next(30, 255); //还可以根据需要调整此处的值

        int rc = Math.Abs(Math.Min(r - increase, max));
        int gc = Math.Abs(Math.Min(g - increase, max));
        int bc = Math.Abs(Math.Min(b - increase, max));

        return rc.ToString("X2") + gc.ToString("X2") + bc.ToString("X2"); ;
    }
}

