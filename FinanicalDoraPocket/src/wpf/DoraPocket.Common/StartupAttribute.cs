using System;

namespace DoraPocket.Common
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class StartupAttribute : Attribute
    {
        public Type Startup { get; }

        public StartupAttribute(Type startup)
        {
            Startup = Guard.ArgumentNotNull(startup, nameof(startup));
            if (!typeof(IStartup).IsAssignableFrom(startup))    // 如果标签startup类不是继承自IStartup = 如果startup不能分配给IStartup类型的变量
            {
                throw new ArgumentException($"Specified startup type '{startup.FullName}' is not derived from IStartup.", nameof(startup));
            }
        }
    }
}
