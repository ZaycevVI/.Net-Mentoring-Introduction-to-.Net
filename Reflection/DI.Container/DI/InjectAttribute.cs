using System;

namespace DI.Container.DI
{
    /// <inheritdoc />
    /// <summary>
    /// Attribute for property injection like in NInject. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    {
    }
}
