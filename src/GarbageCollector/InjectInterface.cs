using System;
using System.Windows;

namespace GarbageCollector
{
  public class InjectInterface : MarshalByRefObject
  {
    public static bool RunCommands;

    public bool Ping(int clientPID)
    {
      return MainWindow.ProcessId == clientPID;
    }

    public void ChangeRunCommands(bool shouldRunCommands)
    {
      InjectInterface.RunCommands = shouldRunCommands;
    }

    public bool ShouldRunGarbageCollection()
    {
      return InjectInterface.RunCommands;
    }

    public void ReportError(int inClientPid, Exception e)
    {
      int num = (int) MessageBox.Show(e.ToString(), "A client process (" + (object) inClientPid + ") has reported an error...");
    }
  }
}
