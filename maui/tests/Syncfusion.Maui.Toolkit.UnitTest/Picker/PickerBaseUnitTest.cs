using System.Globalization;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class PickerBaseUnitTest : IDisposable
    {
        CultureInfo _defaultCulture;
        CultureInfo _defaultUICulture;
        private readonly IDispatcherProvider _originalProvider;

        public PickerBaseUnitTest()
        {
            _defaultCulture = Thread.CurrentThread.CurrentCulture;
            _defaultUICulture = Thread.CurrentThread.CurrentUICulture;
            _originalProvider = DispatcherProvider.Current;
            var mockDispatcher = new MockDispatcher();
            var mockDispatcherProvider = new MockDispatcherProvider(mockDispatcher);
            DispatcherProvider.SetCurrent(mockDispatcherProvider);
        }

        public void Dispose()
        {
            DispatcherProvider.SetCurrent(_originalProvider);
        }

        bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Thread.CurrentThread.CurrentCulture = _defaultCulture;
                Thread.CurrentThread.CurrentUICulture = _defaultUICulture;
            }

            _disposed = true;
        }

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
                {
                    break;
                }

                type = type.BaseType; // Move to the base class
            }

            if (field == null)
            {
                throw new Exception($"Field {fieldName} not found");
            }

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
                {
                    break;
                }

                type = type.BaseType; // Move to the base class
            }

            if (field == null)
            {
                throw new InvalidOperationException($"Field '{fieldName}' not found.");
            }

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
                {
                    break; // Found the method, break out of the loop
                }

                type = type.BaseType; // Move to the base class
            }

            if (method == null)
            {
                throw new InvalidOperationException($"Method '{methodName}' not found.");
            }

            return method.Invoke(obj, parameters);
        }
    }

    public class MockDispatcherProvider : IDispatcherProvider
    {
        private readonly IDispatcher _mockDispatcher;

        public MockDispatcherProvider(IDispatcher mockDispatcher)
        {
            _mockDispatcher = mockDispatcher;
        }

        public IDispatcher GetForCurrentThread()
        {
            return _mockDispatcher;
        }
    }

    public class MockDispatcher : IDispatcher
    {
        public bool IsDispatchRequired => false;

        public bool Dispatch(Action action)
        {
            action();
            return true;
        }

        public bool DispatchDelayed(TimeSpan delay, Action action)
        {
            action();
            return true;
        }

        public IDispatcherTimer CreateTimer()
        {
            return new MockDispatcherTimer();
        }
    }

    public class MockDispatcherTimer : IDispatcherTimer
    {
        public TimeSpan Interval { get; set; }
        public bool IsRepeating { get; set; }
        public bool IsRunning { get; private set; }

        public event EventHandler? Tick;

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void SimulateTick()
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }
    }
}
