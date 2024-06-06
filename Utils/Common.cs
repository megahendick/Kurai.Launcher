using static System.Double;

namespace Kurai.Launcher.Utils;

public class Common
{
    public static double ConvertToDouble(string? value, double defaultValue = 0)
    {
        return TryParse(value, out var outVal) ? outVal : defaultValue;
    }
}