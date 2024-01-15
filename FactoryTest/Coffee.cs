using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryTest
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

    public class CoffeeFactory
    {
        public CoffeeFactory()
        {
        }

        /// <summary>
        /// 简单工厂中必须要有一个方法来根据指定的逻辑创建实例
        /// </summary>
        /// <param name="fruitType"></param>
        /// <returns></returns>
        public static Coffee OrderCoffe(CoffeeEnum coffeeEnum)
        {
            switch (coffeeEnum)
            {
                case CoffeeEnum.AmericanCoffee:
                    return new AmericanCoffee();
                case CoffeeEnum.LatterCoffe:
                    return new LatterCoffe();
            }
            return null;
        }

        public enum CoffeeEnum
        {
            AmericanCoffee,
            LatterCoffe
        }
    }
}
