using EasyHook;
using GarbageCollector;
using System;
using System.Threading;

namespace GCHook
{
  public class InjectionPoint : IEntryPoint
  {
    public static int SleepTime = 5000;
    public InjectInterface Interface = (InjectInterface) null;
    public LocalHook CreateFileHook = (LocalHook) null;

    public InjectionPoint(RemoteHooking.IContext InContext, string InChannelName)
    {
      this.Interface = RemoteHooking.IpcConnectClient<InjectInterface>(InChannelName);
      this.Interface.Ping(RemoteHooking.GetCurrentProcessId());
    }

    public void Run(RemoteHooking.IContext InContext, string InArg1)
    {
      try
      {
        Console.WriteLine("Calling Run method...");
      }
      catch (Exception ex)
      {
        this.Interface.ReportError(RemoteHooking.GetCurrentProcessId(), ex);
        return;
      }
      try
      {
        while (this.Interface.Ping(RemoteHooking.GetCurrentProcessId()))
        {
          if (this.Interface.ShouldRunGarbageCollection())
          {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            this.Interface.ChangeRunCommands(false);
          }
          Thread.Sleep(500);
        }
      }
      catch
      {
      }
    }
  }
}
