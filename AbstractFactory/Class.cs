using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactory
{

    /// <summary>
    /// 咖啡产品抽象类
    /// </summary>
    public abstract class Coffee
    {
        /// <summary>
        /// 方便演示，只定义两个代表性方法。
        /// </summary>
        public abstract void GetName();

        public void AddSugar()
        {
            Console.WriteLine("加糖");
        }
    }

    /// <summary>
    /// 甜点产品抽象类
    /// </summary>
    public abstract class Dessert
    {
        public abstract void GetName();
    }


    //具体产品类，不同产品继承不同抽象类。
    public class AmericanCoffee : Coffee
    {
        public override void GetName()
        {
            Console.WriteLine("我是一杯美式咖啡。");
        }
    }

    public class LatterCoffe : Coffee
    {
        public override void GetName()
        {
            Console.WriteLine("我是一杯拿铁咖啡。");
        }
    }

    public class MatchaMousse : Dessert
    {
        public override void GetName()
        {
            Console.WriteLine("我是一块抹茶慕斯。");
        }
    }

    public class Tiramisu : Dessert
    {
        public override void GetName()
        {
            Console.WriteLine("我是一块提拉米苏。");
        }
    }


    /// <summary>
    /// 风味工厂
    /// </summary>
    public abstract class RelishFactory
    {
        //生产一杯咖啡
        public abstract Coffee GetCoffee();
        //生产一块甜点
        public abstract Dessert GetDessert();
    }

    /// <summary>
    /// 美式风味工厂
    /// </summary>
    public class AmericanRelishFactory : RelishFactory
    {
        public override Coffee GetCoffee()
        {
            return new AmericanCoffee();
        }

        public override Dessert GetDessert()
        {
            return new MatchaMousse();
        }
    }

    /// <summary>
    /// 意大利风味工厂
    /// </summary>
    public class ItalyRelishFactory : RelishFactory
    {
        public override Coffee GetCoffee()
        {
            return new LatterCoffe();
        }

        public override Dessert GetDessert()
        {
            return new Tiramisu();
        }
    }
}
