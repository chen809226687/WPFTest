using Avalonia.Controls;
using Avalonia.Interactivity;
using ListTest.Views;
using OpenCvSharp;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reactive;
using System.Text.RegularExpressions;

namespace ListTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        readonly string _Url = "rtsp://admin:fb123456@192.168.1.64/h264/ch1/main/av_stream";

     
        public MainWindowViewModel()
        {

        }

    }

}

