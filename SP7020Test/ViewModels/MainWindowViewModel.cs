using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Unicode;
using xx.vv.Views.UserMgr;

namespace SP7020Test.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            Users.Add(new UserViewModel()
            {
                Name = "Test",
                Isused = true,
            });
            Users.Add(new UserViewModel()
            {
                Name = "Test2",
                Isused = false,
            });


            var series = new LineSeries<DateTimePoint>
            {
                DataPadding = new LiveChartsCore.Drawing.LvcPoint(20, 2),
                LineSmoothness = 1,
                GeometrySize = 0,
                Fill = null,
                Name = "萨达萨达是·1",
 
                Values = new ObservableCollection<DateTimePoint>
                {
                    new DateTimePoint()
                    {
                        DateTime = DateTime.Now,
                        Value = 1
                    },
                    new DateTimePoint()
                    {
                        DateTime =  DateTime.Now.AddSeconds(1), // 在当前时间上加一秒  
                        Value = 2
                    },
                    new DateTimePoint()
                    {
                        DateTime =  DateTime.Now.AddSeconds(2),
                        Value = 1
                    },
                    new DateTimePoint()
                    {
                        DateTime =  DateTime.Now.AddSeconds(3),
                        Value = 2
                    },
                    new DateTimePoint()
                    {
                        DateTime =  DateTime.Now.AddSeconds(4),
                        Value = 1
                    },

                },
               
          

            };
            var series2 = new LineSeries<DateTimePoint>
            {
                DataPadding = new LiveChartsCore.Drawing.LvcPoint(20, 2),
                LineSmoothness = 1,
                GeometrySize = 0,
                Fill = null,
                Name = Encoding.UTF8.GetString(Encoding.Default.GetBytes("dfgfdf")),

                Values = new ObservableCollection<DateTimePoint>
                {
                    new DateTimePoint()
                    {
                        DateTime = DateTime.Now,
                        Value = 10
                    },
                    new DateTimePoint()
                    {
                        DateTime =  DateTime.Now.AddSeconds(1), // 在当前时间上加一秒  
                        Value = 12
                    },
                    new DateTimePoint()
                    {
                        DateTime =  DateTime.Now.AddSeconds(2),
                        Value = 11
                    },
                    new DateTimePoint()
                    {
                        DateTime =  DateTime.Now.AddSeconds(3),
                        Value = 21
                    },
                    new DateTimePoint()
                    {
                        DateTime =  DateTime.Now.AddSeconds(4),
                        Value = 11
                    },

                },



            };
            Series.Add(series);
            Series.Add(series2);
        }


        public SolidColorPaint LegendTextPaint { get; set; } = new SolidColorPaint(new SKColor(2, 130, 146));

        public SolidColorPaint LedgendBackgroundPaint { get; set; } = new SolidColorPaint(new SKColor(2, 130, 146));
        private ObservableCollection<UserViewModel> _Users = new ObservableCollection<UserViewModel>();
        public ObservableCollection<UserViewModel> Users
        {
            get => _Users;
            set => this.RaiseAndSetIfChanged(ref _Users, value);
        }



        public SolidColorPaint TooltipTextPaint { get; set; } =
             new()
             {
                 Color = new SKColor(2, 130, 146),
                 SKTypeface = SKFontManager.Default.MatchCharacter('汉'),
             };

        private ObservableCollection<ISeries> _Series = new ObservableCollection<ISeries>();
        public ObservableCollection<ISeries> Series
        {
            get => _Series;
            set
            {
                this.RaiseAndSetIfChanged(ref _Series, value);
            }
        }

        private SolidColorPaint _Series1 = new SolidColorPaint(new SKColor(255, 0, 0));
        public SolidColorPaint Series1
        {
            get => _Series1;
            set
            {
                this.RaiseAndSetIfChanged(ref _Series1, value);
            }
        }
    }
}
