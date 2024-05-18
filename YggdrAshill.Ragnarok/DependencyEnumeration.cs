using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IDependencyEnumeration"/>.
    /// </summary>
    public sealed class DependencyEnumeration : IDependencyEnumeration
    {
        /// <summary>
        /// Singleton of <see cref="DependencyEnumeration"/>.
        /// </summary>
        public static DependencyEnumeration Instance { get; } = new();

        private static BindingFlags Binding => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private DependencyEnumeration()
        {
        }

        /// <inheritdoc/>
        public IEnumerable<ConstructorInfo> GetConstructorList(Type type)
        {
            return type.GetConstructors(Binding).OrderByDescending(info => info.GetParameters().Length);
        }

        /// <inheritdoc/>
        public IEnumerable<FieldInfo> GetFieldList(Type type)
        {
            return type.GetFields(Binding).Where(info => !info.IsLiteral &&!info.IsInitOnly && !info.IsDefined(typeof(CompilerGeneratedAttribute), false));
        }

        /// <inheritdoc/>
        public IEnumerable<PropertyInfo> GetPropertyList(Type type)
        {
            return type.GetProperties(Binding).Where(info => info.SetMethod != null && info.GetIndexParameters().Length == 0);
        }

        /// <inheritdoc/>
        public IEnumerable<MethodInfo> GetMethodList(Type type)
        {
            return type.GetMethods(Binding).OrderByDescending(info => info.GetParameters().Length);
        }
    }
}
