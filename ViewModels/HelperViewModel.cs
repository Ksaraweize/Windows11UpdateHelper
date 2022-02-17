using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows11UpdateHelper.Models;

namespace Windows11UpdateHelper.ViewModels
{
    class HelperViewModel : ObservableObject
    {
        private readonly Helper helper = new();
        public int InsiderChannelSelected
        {
            get => helper.InsiderChannelSelected;
            set
            {
                SetProperty(helper.InsiderChannelSelected, value, helper, (obj, val) => obj.InsiderChannelSelected = val);
                ChangeInsiderChannel(value);
            }
        }

        public bool Monitoring
        {
            get => helper.Monitoring;
            set
            {
                SetProperty(helper.Monitoring, value, helper, (obj, val) => obj.Monitoring = val);
                ByPassTpmAndUefi(value);
            }
        }


        public List<string> ChannelNames { get; } = new string[] { "Dev", "Beta", "ReleasePreview" }.ToList();

        public HelperViewModel()
        {
            var branchName = RegistryUtil.GetString(Registry.LocalMachine, @"SOFTWARE\Microsoft\WindowsSelfHost\Applicability", "BranchName");
            InsiderChannelSelected = ChannelNames.ToList().IndexOf(branchName ?? "");
        }

        private void ByPassTpmAndUefi(bool start)
        {
            if (start)
            {
                new Thread(() =>
                {
                    //系统盘
                    var systemDisk = new DirectoryInfo(Environment.SystemDirectory).Root;

                    var monitorDir = new DirectoryInfo($@"{systemDisk.FullName}$WINDOWS.~BT\Sources");

                    while (Monitoring)
                    {
                        if (monitorDir.Exists)
                        {
                            FileInfo f1 = new($@"{monitorDir.FullName}\AppraiserRes.dll");
                            if (f1.Exists)
                            {
                                f1.Delete();
                                Directory.CreateDirectory(f1.FullName);
                            }
                        }
                        Thread.Sleep(1000);
                    }
                }).Start();
            }
        }

        private void ChangeInsiderChannel(int index)
        {
            if (index < 0 || index > ChannelNames.Count - 1) { return; }
            RegistryUtil.SetString(Registry.LocalMachine, @"SOFTWARE\Microsoft\WindowsSelfHost\Applicability", "BranchName", ChannelNames[index]);
            RegistryUtil.SetString(Registry.LocalMachine, @"SOFTWARE\Microsoft\WindowsSelfHost\UI\Selection", "UIBranch", ChannelNames[index]);
        }
    }

}
