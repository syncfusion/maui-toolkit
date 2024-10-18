using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal class FastReflection
    {
        Func<object, object>? _getMethod;

        public FastReflection()
        {
        }

        internal bool SetPropertyName(string name, object obj)
        {
            var propertyInfo = ChartDataUtils.GetPropertyInfo(obj, name);

            IPropertyAccessor? xPropertyAccessor = null;
            if (propertyInfo != null)
            {
                xPropertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
            }

            if (xPropertyAccessor != null)
            {
                _getMethod = xPropertyAccessor.GetMethod;
                return true;
            }

            return false;
        }

        internal object? GetValue(object item)
        {
            return _getMethod != null ? _getMethod(item) : null;
        }

        internal bool IsArray(object item)
        {
            var obj = GetValue(item);
            return obj != null && obj.GetType().IsArray;
        }
    }

    /// <summary>
    /// Contains members to hold PropertyInfo.
    /// </summary>
    internal static class FastReflectionCaches
    {
        static FastReflectionCaches()
        {
            MethodInvokerCache = new MethodInvokerCache();
            PropertyAccessorCache = new PropertyAccessorCache();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IFastReflectionCache<MethodInfo, IMethodInvoker> MethodInvokerCache { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static IFastReflectionCache<PropertyInfo, IPropertyAccessor> PropertyAccessorCache { get; set; }
    }

    internal class MethodInvokerCache : FastReflectionCache<MethodInfo, IMethodInvoker>
    {
        protected override IMethodInvoker Create(MethodInfo key)
        {
            return FastReflectionFactories.MethodInvokerFactory.Create(key);
        }
    }

    internal static class FastReflectionFactories
    {
        static FastReflectionFactories()
        {
            MethodInvokerFactory = new MethodInvokerFactory();
        }

        public static IFastReflectionFactory<MethodInfo, IMethodInvoker> MethodInvokerFactory { get; set; }
    }

    internal class MethodInvokerFactory : IFastReflectionFactory<MethodInfo, IMethodInvoker>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public IMethodInvoker Create(MethodInfo key)
        {
            return new MethodInvoker(key);
        }

        IMethodInvoker IFastReflectionFactory<MethodInfo, IMethodInvoker>.Create(MethodInfo key)
        {
            return this.Create(key);
        }
    }

    internal interface IFastReflectionFactory<TKey, TValue>
    {
        TValue Create(TKey key);
    }

    internal interface IFastReflectionCache<TKey, TValue>
    {
        TValue Get(TKey key);
    }

    internal interface IPropertyAccessor
    {
        Func<object, object>? GetMethod
        {
            get;
        }

        object GetValue(object instance);

        void SetValue(object instance, object value);
    }

    internal class PropertyAccessorCache : FastReflectionCache<PropertyInfo, IPropertyAccessor>
    {
        protected override IPropertyAccessor Create(PropertyInfo key)
        {
            return new PropertyAccessor(key);
        }
    }

    internal class PropertyAccessor : IPropertyAccessor
    {
        public Func<object, object>? GetMethod
        {
            get
            {
                return getter;
            }
        }

        Func<object, object>? getter;
        MethodInvoker? setMethodInvoker;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
            this.InitializeGet(propertyInfo);
            this.InitializeSet(propertyInfo);
        }

        void InitializeGet(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead || propertyInfo.DeclaringType == null)
            {
                return;
            }

            // Target: (object)(((TInstance)instance).Property)

            // preparing parameter, object type
            var instance = Expression.Parameter(typeof(object), "instance");

            var methodInfo = propertyInfo.GetGetMethod();

            // non-instance for static method, or ((TInstance)instance)
            var instanceCast = methodInfo != null && methodInfo.IsStatic ? null :
                Expression.Convert(instance, propertyInfo.DeclaringType);

            // ((TInstance)instance).Property
            var propertyAccess = Expression.Property(instanceCast, propertyInfo);

            // (object)(((TInstance)instance).Property)
            var castPropertyValue = Expression.Convert(propertyAccess, typeof(object));

            // Lambda expression
            var lambda = Expression.Lambda<Func<object, object>>(castPropertyValue, instance);

            this.getter = lambda.Compile();
        }

        void InitializeSet(PropertyInfo propertyInfo)
        {
            var methodInfo = propertyInfo.GetSetMethod();

            if (!propertyInfo.CanWrite || methodInfo == null)
            {
                return;
            }


            this.setMethodInvoker = new MethodInvoker(methodInfo); // .GetSetMethod(true));
        }

        public object GetValue(object o)
        {
            if (this.getter == null)
            {
                throw new NotSupportedException("Get method is not defined for this property.");
            }

            return this.getter(o);
        }

        public void SetValue(object o, object value)
        {
            if (this.setMethodInvoker == null)
            {
                throw new NotSupportedException("Set method is not defined for this property.");
            }

            this.setMethodInvoker.Invoke(o, new object[] { value });
        }

        #region IPropertyAccessor Members

        object IPropertyAccessor.GetValue(object instance)
        {
            return this.GetValue(instance);
        }

        void IPropertyAccessor.SetValue(object instance, object value)
        {
            this.SetValue(instance, value);
        }

        #endregion
    }

    internal interface IMethodInvoker
    {
        object Invoke(object instance, params object[] parameters);
    }

    internal class MethodInvoker : IMethodInvoker
    {
        Func<object, object[], object> invoker;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public MethodInfo MethodInfo { get; private set; }

        public MethodInvoker(MethodInfo methodInfo)
        {
            this.MethodInfo = methodInfo;
            this.invoker = CreateInvokeDelegate(methodInfo);
        }

        public object Invoke(object instance, params object[] parameters)
        {
            return this.invoker(instance, parameters);
        }

        static Func<object, object[], object> CreateInvokeDelegate(MethodInfo methodInfo)
        {
            // Target: ((TInstance)instance).Method((T0)parameters[0], (T1)parameters[1], ...)
            // parameters to execute
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            // build parameter list
            var parameterExpressions = new List<Expression>();
            var paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                // (Ti)parameters[i]
                BinaryExpression valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(
                    valueObj, paramInfos[i].ParameterType);

                parameterExpressions.Add(valueCast);
            }

            // non-instance for static method, or ((TInstance)instance)
            var declaringType = methodInfo.DeclaringType;

            var instanceCast = methodInfo.IsStatic ? null : declaringType != null ?
                Expression.Convert(instanceParameter, declaringType) : null;

            // static invoke or ((TInstance)instance).Method
            var methodCall = Expression.Call(instanceCast, methodInfo, parameterExpressions);

            // ((TInstance)instance).Method((T0)parameters[0], (T1)parameters[1], ...)
            if (methodCall.Type == typeof(void))
            {
                var lambda = Expression.Lambda<Action<object, object[]>>(
                        methodCall, instanceParameter, parametersParameter);

                Action<object, object[]> execute = lambda.Compile();
                return (instance, parameters) =>
                {
                    execute(instance, parameters);
#pragma warning disable CS8603 // Possible null reference return.
                    return null;
#pragma warning restore CS8603 // Possible null reference return.
                };
            }
            else
            {
                var castMethodCall = Expression.Convert(methodCall, typeof(object));
                var lambda = Expression.Lambda<Func<object, object[], object>>(
                    castMethodCall, instanceParameter, parametersParameter);

                return lambda.Compile();
            }
        }

        #region IMethodInvoker Members

        object IMethodInvoker.Invoke(object instance, params object[] parameters)
        {
            return this.Invoke(instance, parameters);
        }

        #endregion
    }

    internal abstract class FastReflectionCache<TKey, TValue> : IFastReflectionCache<TKey, TValue> where TKey : class
    {
        Dictionary<TKey, TValue> cache = new Dictionary<TKey, TValue>();

        public TValue Get(TKey key)
        {
            TValue? value = default(TValue);
            if (this.cache.TryGetValue(key, out value))
            {
                return value;
            }

            lock (key)
            {
                if (!this.cache.TryGetValue(key, out value))
                {
                    value = this.Create(key);
                    this.cache[key] = value;
                }
            }

            return value;
        }

        protected abstract TValue Create(TKey key);
    }
}
