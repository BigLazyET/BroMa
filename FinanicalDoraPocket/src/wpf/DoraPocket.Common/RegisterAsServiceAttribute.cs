using System;
using System.Linq;

namespace DoraPocket.Common
{
    /// <summary>
    /// 用于标签需要依赖注入的类
    /// 规定：标签仅针对类；允许针对一个类多次设置标签(取最后一次为准)；针对标签类的继承子类不具有此标签特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class RegisterAsServiceAttribute : Attribute
    {
        public Type[] ServiceTypes { get; }

        public RegisterAsServiceAttribute(Type serviceType)
        {
            Guard.ArgumentNotNull(serviceType, nameof(serviceType));
            ServiceTypes = new Type[] { serviceType };
        }

        public RegisterAsServiceAttribute(Type serviceType, params Type[] serviceTypes)
        {
            Guard.ArgumentNotNull(serviceType, nameof(serviceType));
            if (serviceTypes == null)
            {
                ServiceTypes = new Type[] { serviceType };
            }
            else
            {
                ServiceTypes = serviceTypes.Concat(new Type[] { serviceType }).ToArray();
            }
        }
    }
}
