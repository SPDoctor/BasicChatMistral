using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Plugins;

public class TimePlugin
{
  [KernelFunction, Description("Gets the current date and time")]
  public static DateTime Time()
  {
    return DateTime.UtcNow;
  }

  [KernelFunction, Description("Gets the current day of the week")]
  public static System.DayOfWeek DayOfWeek()
  {
    return DateTime.UtcNow.DayOfWeek;
  }
}















































//using Microsoft.SemanticKernel;
//using System.ComponentModel;

//namespace Plugins;

//public class TimePlugin
//{
//  [KernelFunction, Description("Get the current date and time")]
//  public static DateTime Time()
//  {
//    return DateTime.UtcNow;
//  }

//  [KernelFunction, Description("get the current day of the week")]
//  public static System.DayOfWeek DayOfWeek()
//  {
//    return DateTime.UtcNow.DayOfWeek;
//  }
//}
