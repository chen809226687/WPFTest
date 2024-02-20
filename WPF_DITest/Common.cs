using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using static WPF_DITest.MainWindow;

namespace WPF_DITest
{

    public class Common
    {

        IUnityContainer _ioc;
        public Common(IUnityContainer ioc) 
        {
            _ioc= ioc;
            
        }

        public void fangfa()
        {

            var carInstance = _ioc.Resolve<Car>("车第一个实例");

            carInstance.Name = "汽车人变身";
        }


    }
}
