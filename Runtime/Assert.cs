using System;
using System.Diagnostics;

namespace EyE.Debug
{
    /// <summary>
    /// Provides a set of assertion methods for validating conditions during debugging.
    /// These methods throw FailedAssertionExceptions when conditions are not met, allowing for
    /// easier identification of issues during development. All assertions are 
    /// conditionally compiled to be included only in DEBUG builds.
    /// Using the functions that have generic parameters will include the type name in any failed assertion messages that are generated.
    /// Or, one can use the non-generic versions, if desired.
    /// </summary>
    public static class Assert
    {
        //conditional string
        private const string DebugCompilerDefinedConstant = "DEBUG";

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
            public FailedAssertionException(string message, object context = null) : base(ContextMessage(context) + message) { }
        }

        /// <summary>
        /// Exception thrown when a generic assertion fails.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.</typeparam>
        public class FailedAssertionException<T> : System.Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FailedAssertionException{T}"/> class with a specified error message.
            /// </summary>
            /// <param name="message">The error message.</param>
            public FailedAssertionException(string message, object context = null) :
                base("FailedAssertion[" + typeof(T) + "]: " + ContextMessage(context) + message)
            { }
        }



        /// <summary>
        /// Asserts that a condition is true. Throws <see cref="FailedAssertionException"/> if the condition is false.
        /// </summary>
        /// <param name="b">The condition to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isTrue(bool b, string message, object context = null)
        {
            if (!b)
                throw new FailedAssertionException(message);
        }

        /// <summary>
        /// Asserts that a condition is false. Throws <see cref="FailedAssertionException"/> if the condition is true.
        /// </summary>
        /// <param name="b">The condition to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isFalse(bool b, string message)
        {
            if (b)
                throw new FailedAssertionException(message);
        }

        /// <summary>
        /// Asserts that an object is not null. Throws <see cref="FailedAssertionException"/> if the object is null.
        /// </summary>
        /// <param name="obj">The object to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isNotNull(object obj, string message)
        {
            if (obj == null)
                throw new FailedAssertionException(message);
        }

        /// <summary>
        /// Asserts that all objects are not null. Throws <see cref="FailedAssertionException"/> if any object is null.
        /// </summary>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <param name="objs">The objects to evaluate.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void areNotNull(string message, params object[] objs)
        {
            foreach (object obj in objs)
                if (obj == null)
                    throw new FailedAssertionException(message);
        }
        /// <summary>
        /// Asserts that two values are equal. Throws <see cref="FailedAssertionException{T}"/> if they are not equal.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare, must support equality comparison. </typeparam>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <exception cref="InvalidOperationException">Thrown if the types of <paramref name="expected"/> and <paramref name="actual"/> cannot be compared for equality.</exception>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isEqual<TValueType>(TValueType expected, TValueType actual, string message) where TValueType : IEquatable<TValueType>
        {
            if (!expected.Equals(actual))
                throw new FailedAssertionException(message);
        }
        /// <summary>
        /// Asserts that the result of a function is true. Throws <see cref="FailedAssertionException"/> if the result is false.
        /// </summary>
        /// <param name="bFunc">The function to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void expensiveIsTrue(System.Func<bool> bFunc, string message)
        {
            if (!bFunc())
                throw new FailedAssertionException(message);
        }

        /// <summary>
        /// Asserts that a condition is true. Throws <see cref="FailedAssertionException{T}"/> if the condition is false.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="b">The condition to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isTrue<T>(bool b, string message)
        {
            if (!b)
                throw new FailedAssertionException<T>(message);
        }

        /// <summary>
        /// Asserts that a condition is false. Throws <see cref="FailedAssertionException{T}"/> if the condition is true.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="b">The condition to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isFalse<T>(bool b, string message)
        {
            if (b)
                throw new FailedAssertionException<T>(message);
        }

        /// <summary>
        /// Asserts that an object is not null. Throws <see cref="FailedAssertionException{T}"/> if the object is null.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="obj">The object to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isNotNull<T>(object obj, string message)
        {
            if (obj == null)
                throw new FailedAssertionException<T>(message);
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
            foreach (object obj in objs)
                if (obj == null)
                    throw new FailedAssertionException<T>(message);
        }

        /// <summary>
        /// Asserts that the result of a function is true. Throws <see cref="FailedAssertionException{T}"/> if the result is false.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <param name="bFunc">The function to evaluate.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void expensiveIsTrue<T>(System.Func<bool> bFunc, string message)
        {
            if (!bFunc())
                throw new FailedAssertionException<T>(message);
        }


        /// <summary>
        /// Asserts that two values are equal. Throws <see cref="FailedAssertionException{T}"/> if they are not equal.
        /// </summary>
        /// <typeparam name="T">The type related to the failed assertion.  Usually, this is the class calling the assertion.</typeparam>
        /// <typeparam name="V">The type of the values to compare, must support equality comparison. </typeparam>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception if the assertion fails.</param>
        /// <exception cref="InvalidOperationException">Thrown if the types of <paramref name="expected"/> and <paramref name="actual"/> cannot be compared for equality.</exception>
        [Conditional(DebugCompilerDefinedConstant)]
        public static void isEqual<T, TValueType>(TValueType expected, TValueType actual, string message) where TValueType : IEquatable<TValueType>
        {
            if (!expected.Equals(actual))
                throw new FailedAssertionException<T>(message);
        }
    }
}
public class AClass
{
    object ref1;
    object ref2;

    public void blash()
    {
        EyE.Debug.Assert.areNotNull<AClass>("a ref is null", ref1, ref2);
        // do stuff
    }
}
