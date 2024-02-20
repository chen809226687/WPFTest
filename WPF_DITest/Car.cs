using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_DITest
{
    public class Car : ICar
    {
        public string Name { get; set; }

        public void aaa()
        {
            Name = "小汽车翻车了";
        }
    }
}
