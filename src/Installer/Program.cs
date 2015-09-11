using EasyHook;
using System;

namespace Installer
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Console.WriteLine("Installing interfaces...");
      try
      {
        Config.Register("Runs the garbage collector on EasyHook!", "GCHook.dll", "GarbageCollector.exe");
      }
      catch (ApplicationException ex)
      {
        Console.WriteLine("This is an administrative task! Permission denied.");
        Console.ReadLine();
        return;
      }
      Console.WriteLine("Successfully installed.");
      Console.ReadLine();
    }
  }
}
