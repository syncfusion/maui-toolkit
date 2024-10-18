using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit;

/// <summary>
/// Base class for all unit test cases classes
/// </summary>
public abstract class BaseUnitTest
{
    /// <summary>
    /// Method used to set the private field of an object
    /// </summary>
    /// <param name="obj">The Object</param>
    /// <param name="fieldName">The Field Name</param>
    /// <param name="value">The Value</param>
    /// <exception cref="Exception"></exception>
    protected void SetPrivateField(object obj, string fieldName, object? value)
    {
        var type = obj.GetType();
        FieldInfo? field = null;
        while (type != null)
        {
            field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                break;
            type = type.BaseType; // Move to the base class
        }
            
        if (field == null) throw new Exception($"Field {fieldName} not found");
        field.SetValue(obj, value);
    }

    /// <summary>
    /// Method used to get the private field of an object
    /// </summary>
    /// <typeparam name="T">The Type</typeparam>
    /// <param name="obj">The Object</param>
    /// <param name="fieldName">The Field Name</param>
    /// <returns>The value of the private field or property</returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected object? GetPrivateField<T>(T obj, string fieldName)
    {
        var type = obj?.GetType();
        FieldInfo? field = null;
        while (type != null)
        {
            field = typeof(T).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                break;
            type = type.BaseType; // Move to the base class
        }
        if (field == null)
            throw new InvalidOperationException($"Field '{fieldName}' not found.");
        return field.GetValue(obj);
    }

    /// <summary>
    /// Method used to invoke the private method of an object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="methodName"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected object? InvokePrivateMethod<T>(T obj, string methodName, params object?[] parameters)
    {
        var type = obj?.GetType();
        MethodInfo? method = null;

        // Traverse the class hierarchy to find the method
        while (type != null)
        {
            method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (method != null)
                break; // Found the method, break out of the loop

            type = type.BaseType; // Move to the base class
        }
        if (method == null)
            throw new InvalidOperationException($"Method '{methodName}' not found.");
        return method.Invoke(obj, parameters);
    }

    /// <summary>
    /// Method used to invoke the private method of an object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="methodName"></param>
    /// <param name="parameters"> ref parameter</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected object? InvokeRefPrivateMethod<T>(T obj, string methodName, ref object[] parameters)
    {
        var method = typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (method == null)
            throw new InvalidOperationException($"Method '{methodName}' not found.");

        var result = method.Invoke(obj, parameters);
        return result;
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected object? GetNonPublicProperty<T>(T obj, string propertyName)
    {
        var property = typeof(T).GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (property == null)
            throw new InvalidOperationException($"Property '{propertyName}' not found.");
        return property.GetValue(obj);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    /// <exception cref="Exception"></exception>
    protected void SetNonPublicProperty(object obj, string propertyName, object? value)
    {
        var property = obj.GetType().GetProperty(propertyName, BindingFlags.SetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (property == null)
            throw new Exception($"Property {propertyName} not found in {obj.GetType().Name}");
        property.SetValue(obj, value);
    }

    /// <summary>
    /// Method used to invoke the private static method of an object
    /// </summary>
    /// <typeparam name="T">The generic type parameter</typeparam>
    /// <param name="obj">The instance of the class</param>
    /// <param name="methodName">The private static method name</param>
    /// <param name="parameters">The parameters of the method</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected object InvokeStaticPrivateMethod<T>(T obj, string methodName, params object[] parameters)
    {
        var method = typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
        if (method == null)
            throw new InvalidOperationException($"Method '{methodName}' not found.");

        return method.Invoke(obj, parameters)!;
    }


    /// <summary>
    /// Method used to invoke the private method of a static class
    /// </summary>
    /// <param name="type">The type of the static class</param>
    /// <param name="methodName">The method name in the static class</param>
    /// <param name="parameters">The parameters for the method</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected object InvokeStaticPrivateMethodClass(Type type, string methodName, params object[] parameters)
    {
        var method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);

        if (method == null)
            throw new InvalidOperationException($"Method '{methodName}' not found.");
        return method.Invoke(null, parameters)!;
    }


}
