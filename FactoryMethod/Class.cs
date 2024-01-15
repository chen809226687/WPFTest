using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMethod
{
    /// <summary>
    /// 该类是产品的父类即抽象产品，定义所有子类的共有属性和方法
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

    /// <summary>
    /// 抽象工厂
    /// </summary>
    public abstract class CoffeeFactory
    {
        public abstract Coffee GetCoffee();
    }

    /// <summary>
    /// 美式咖啡工厂
    /// </summary>
    public class AmericanFactory : CoffeeFactory
    {
        public override Coffee GetCoffee()
        {
            return new AmericanCoffee();
        }
    }

    /// <summary>
    /// 拿铁咖啡工厂
    /// </summary>
    public class LatterFactory : CoffeeFactory
    {
        public override Coffee GetCoffee()
        {
            return new LatterCoffe();
        }
    }

}
