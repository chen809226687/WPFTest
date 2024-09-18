using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfg_Test
{
    public class Model
    {

        public string Name { get; set; }

        public string PassWord { get; set; }

        public object value { get; set; }
        // 将 object 改为 string 并进行适当的转换  
        public string Value
        {
            get => value?.ToString(); // 转为字符串输出  
            set => this.value = value; // 如果你需要设置它  
        }

    }
}
