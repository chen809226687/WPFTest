using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_DITest
{
    public class Service
    {
        ICar _car;
        public Service(ICar car)
        {
            _car = car;

        }

        public void fangfafafa()
        {
            _car.Name = "汽车人集合！";
        }
        


    }
}
