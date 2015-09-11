using EasyHook;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows;

namespace GarbageCollector
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public static int ProcessId;
        private static string channelName;
        private static IpcServerChannel server;
        private static InjectInterface Interface;
        private bool _isAttached;

        public bool IsAttached
        {
            get
            {
                return _isAttached;
            }
            set
            {
                _isAttached = value;
                RaisePropertyChanged("IsAttached");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                server = RemoteHooking.IpcCreateServer<InjectInterface>(
                    ref channelName, 
                    WellKnownObjectMode.Singleton);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void OnGarbageCollectClicked(object sender, RoutedEventArgs e)
        {
            Interface.ChangeRunCommands(true);
        }

        private void OnAttachToPid(object sender, RoutedEventArgs e)
        {
            ProcessId = int.Parse(uxPid.Text);
            RemoteHooking.Inject(
                ProcessId, 
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GCHook.dll"), 
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GCHook.dll"), 
                (object)channelName);
            Interface = RemoteHooking.IpcConnectClient<InjectInterface>(channelName);
            IsAttached = true;
        }

        public void RaisePropertyChanged(string property)
        {
            PropertyChangedEventHandler changedEventHandler = PropertyChanged;
            if (changedEventHandler == null)
                return;
            changedEventHandler(this, new PropertyChangedEventArgs(property));
        }
    }
}