using Avalonia.Media.Imaging;
using Microsoft.VisualBasic;
using ReactiveUI;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SUKIUITest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        List<string> strings;
        public MainWindowViewModel()
        {
            strings = new List<string>();
          
            var imageExtensions = new[] { ".jpg", ".png", ".bmp", ".jpeg", ".gif" }; // 支持的图片格式
            DirectoryInfo directoryInfo = new DirectoryInfo("C:\\Users\\80922\\Pictures\\壁纸");
            FileInfo[] files = directoryInfo.GetFiles().Where(f => imageExtensions.Contains(f.Extension.ToLower())).ToArray();

            foreach (FileInfo file in files)
            {
                MyImage myImage = new MyImage();
                myImage.ImageBitmap = new Bitmap(file.FullName);

                MyImageList.Add(myImage);
            }
        }

        public ObservableCollection<MyImage> _MyImageList = new ObservableCollection<MyImage>();
        public ObservableCollection<MyImage> MyImageList
        {
            get => _MyImageList;
            set => this.RaiseAndSetIfChanged(ref _MyImageList, value);
        }

        public class MyImage
        {
            public Bitmap ImageBitmap { get; set; }
        }

        public void aaa()
        {

        }

    }
}
