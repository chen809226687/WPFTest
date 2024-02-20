using Avalonia.Media.Imaging;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;

namespace AvaloniaApplicationListImage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {

            string imageNGPath = @"D:\YuBenImage\配方3\1\2024-01-26\wallhaven-exrqrr.jpg";
            ImageItem imageItem = new ImageItem();
            imageItem.ImagePath = new Bitmap(imageNGPath);
            Images.Add(imageItem);
            Images.Add(imageItem);
            Images.Add(imageItem);
            Images.Add(imageItem);
            Images.Add(imageItem);

            string imageNGPat = @"D:\YuBenImage\配方3\1\2024-01-26\wallhaven-j3xkxm.jpg";
            ImageItem imageIte = new ImageItem();
            imageIte.ImagePath = new Bitmap(imageNGPat);
            Images.Add(imageIte);
            Images.Add(imageIte);
            Images.Add(imageIte);
            Images.Add(imageIte);
            Images.Add(imageIte);
            Images.Add(imageIte);
            Images.Add(imageIte);
            Images.Add(imageIte);
            Images.Add(imageIte);
        }


        public ObservableCollection<ImageItem> Images { get; } = new ObservableCollection<ImageItem>();

    }

    public class ImageItem
    {
        public Bitmap ImagePath { get; set; }
        // 这里可以添加更多图片相关的属性，如标题、描述等
    }


}
