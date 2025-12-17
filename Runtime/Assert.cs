using System;
using System.Diagnostics;

namespace EyE.Diagnostics
{
    /// <summary>
    /// Provides a set of assertion methods for validating conditions during debugging.
    /// These methods throw FailedAssertionExceptions when conditions are not met, allowing for
    /// easier identification of issues during development. All assertions are 
    /// conditionally compiled to be included only in DEBUG builds.
    /// Note: a thrown exception means that the calling code will stop executing at the assertion point.
    /// The generic methods are provided for convenience only:  The specified type is used for type name display purposes, rather than type safety.
    ///    The exceptions are the ``Assert.Is<T>`` function, which asserts that the object is of the type specified, 
    ///    and the ``Assert.isEqual<T>`` function which requires the compared objects are of the same type.
    /// Most methods have an optional "context" object parameter, which may be included as a parameter.
    ///    The object's ToString() method will be used to generate text for display purposes.
    /// </summary>
    public static class Assert
    {
        //conditional string
        private const string DebugCompilerDefinedConstant = "EYE_DEBUG";

        /// <summary>
        /// Checks to see if DEBUG is defined, which is required for assertions to work.
        /// </summary>
        /// <returns>Returns true when DEBUG is defined, and assertions will work.  False otherwise.</returns>
        public static bool IsActive()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
        /// <summary>
        /// Use by FailedAssertionException constructors to get a string that identifies the context object.
        /// </summary>
        /// <param name="contextObj">the object to identify</param>
        /// <returns>a string that can be displayed in the exception</returns>
        private static string ContextMessage(object contextObj)
        {
            if (contextObj == null) return "";
            return "Context object: " + contextObj.ToString();
        }

        /// <summary>
        /// Exception thrown when an assertion fails.
        /// </summary>
        public class FailedAssertionException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FailedAssertionException"/> class with a specified error message.
            /// </summary>
            /// <param name="message">The error message.</param>
            public FailedAssertionException(string message, object context = null) : base(ContextMessage(context) + message)
            { }
            public FailedAssertionException(string message, System.Type displayType, object context = null)
                : base("<" + displayType.Name + "> " + ContextMessage(context) + message)
            { }
        }




        /// <summary>
        /// Asserts that a condition is true. Throws <see cref="FailedAssertionException"/> if the condition is false.
        /// </summary>
        /// <param name="b">The condition to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">Optional "context" object, will be included in any exceptions thrown, via ToString()</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isTrue(bool b, string message, object context = null)
        {
            if (!b)
                throw new FailedAssertionException(message,context);
        }

        /// <summary>
        /// Asserts that a condition is false. Throws <see cref="FailedAssertionException"/> if the condition is true.
        /// </summary>
        /// <param name="b">The condition to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">Optional "context" object, will be included in any exceptions thrown, via ToString()</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isFalse(bool b, string message, object context = null)
        {
            if (b)
                throw new FailedAssertionException(message, context);
        }

        /// <summary>
        /// Asserts that an object is not null. Throws <see cref="FailedAssertionException"/> if the object is null.
        /// </summary>
        /// <param name="obj">The object to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">Optional "context" object, will be included in any exceptions thrown, via ToString()</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isNotNull(object obj, string message, object context = null)
        {
            if(ReferenceEquals(obj,null) || obj.Equals(null))
                throw new FailedAssertionException(message, context);
    }

        /// <summary>
        /// Asserts that all objects are not null. Throws <see cref="FailedAssertionException"/> if any object is null.
        /// </summary>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="objs">The objects to evaluate.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void areNotNull(string message, params object[] objs)
        {
            areNotNullWithContext(message,null,objs);
        }
        /// <summary>
        /// Asserts that all objects are not null. Throws <see cref="FailedAssertionException"/> if any object is null.
        /// </summary>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">This "context" object, will be included in any exceptions thrown, via ToString()</param>
        /// <param name="objs">The objects to evaluate.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void areNotNullWithContext(string message, object context, params object[] objs)
        {
            foreach (object obj in objs)
                if (ReferenceEquals(obj, null) || obj.Equals(null))
                    throw new FailedAssertionException(message,context);
        }
        /// <summary>
        /// Asserts that two values are equal. Throws <see cref="FailedAssertionException{T}"/> if they are not equal.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare, must support equality comparison. 
        /// If this type does not define equality (e.g., override `Equals` or 
        /// implement `IEquatable<T>`), only reference equality will be checked.</typeparam>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">Optional "context" object, will be included in any exceptions thrown, via ToString()</param>
        /// <exception cref="InvalidOperationException">Thrown if the types of <paramref name="expected"/> and <paramref name="actual"/> cannot be compared for equality.</exception>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isEqual<TValueType>(TValueType expected, TValueType actual, string message, object context = null) where TValueType : IEquatable<TValueType>
        {
            if (expected == null || actual == null)//if one is null
            {
                if (!(expected == null && actual == null)) //if not BOTH null
                    throw new FailedAssertionException(message, typeof(TValueType));
                else //both are null- good enuff
                    return;
            }
            if (!expected.Equals(actual))
                throw new FailedAssertionException(message,context);
        }
        /// <summary>
        /// Checks if the object passed is of the type of the Type parameter.
        /// </summary>
        /// <typeparam name="TypeOfParamObject"></typeparam>
        /// <param name="obj">object to check the type of</param>
        /// <param name="message">type the object should be</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void Is<TypeOfParamObject>(object obj, string message)
        {
            if (!(obj is TypeOfParamObject))
                throw new FailedAssertionException(message);
        }
        /// <summary>
        /// This function is useful when evaluating the exception is expensive, processing wise.  It ensures that the function will NOT be called when ``DEBUG`` is not defined (release build). 
        /// Asserts that the result of a function is true. Throws <see cref="FailedAssertionException"/> if the result is false.
        /// </summary>
        /// <param name="bFunc">The function to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">Optional "context" object, will be included in any exceptions thrown, via ToString()</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void expensiveIsTrue(System.Func<bool> bFunc, string message, object context = null)
        {
            if (bFunc == null)
                throw new ArgumentException("Cannot pass a null function to Assert.expensiveIsTrue");
            if (!bFunc())
                throw new FailedAssertionException(message,context);
        }

        /// <summary>
        /// Asserts that a condition is true. Throws <see cref="FailedAssertionException{T}"/> if the condition is false.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="b">The condition to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">Optional "context" object, will be included in any exceptions thrown, via ToString()</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isTrue<T>(bool b, string message, object context = null)
        {
            if (!b)
                throw new FailedAssertionException(message,typeof(T),context);
        }

        /// <summary>
        /// Asserts that a condition is false. Throws <see cref="FailedAssertionException{T}"/> if the condition is true.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="b">The condition to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">Optional "context" object, will be included in any exceptions thrown, via ToString()</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isFalse<T>(bool b, string message, object context = null)
        {
            if (b)
                throw new FailedAssertionException(message, typeof(T), context);
        }

        /// <summary>
        /// Asserts that an object is not null. Throws <see cref="FailedAssertionException{T}"/> if the object is null.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="obj">The object to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">Optional "context" object, will be included in any exceptions thrown, via ToString()</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isNotNull<T>(object obj, string message, object context = null)
        {
            if (ReferenceEquals(obj, null) || obj.Equals(null))
                throw new FailedAssertionException(message, typeof(T), context);
        }

        /// <summary>
        /// Asserts that all objects are not null. Throws <see cref="FailedAssertionException{T}"/> if any object is null.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="objs">The objects to evaluate.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void areNotNull<T>(string message, params object[] objs)
        {
            areNotNullWithContext<T>(message, null, objs);
        }

        /// <summary>
        /// Asserts that all objects are not null. Throws <see cref="FailedAssertionException{T}"/> if any object is null.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="context">Optional "context" object, will be included in a3ny exceptions thrown, via ToString()</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="objs">The objects to evaluate.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void areNotNullWithContext<T>(string message, object context, params object[] objs)
        {
            foreach (object obj in objs)
                if (ReferenceEquals(obj, null) || obj.Equals(null))
                    throw new FailedAssertionException(message, typeof(T), context);
        }
        /// <summary>
        /// Asserts that the result of a function is true. Throws <see cref="FailedAssertionException{T}"/> if the result is false.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="bFunc">The function to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="context">Optional "context" object, will be included in any exceptions thrown, via ToString()</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void expensiveIsTrue<T>(System.Func<bool> bFunc, string message, object context = null)
        {
            if (!bFunc())
                throw new FailedAssertionException(message, typeof(T), context);
        }



    }
}