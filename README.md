# EyE.Debug.Assert

EyE.Debug.Assert is a C# library providing assertion methods designed for debugging. This utility helps validate conditions and outputs meaningful error messages, simplifying issue identification during development. All assertions are conditionally compiled and are only included in `DEBUG` builds, making them ideal for internal sanity checks.  For this reason, this class should NOT be used to check user data.

## Features

- Provides type - specific assertion failure messages via generic and non-generic assertion methods.
- Custom exceptions (`FailedAssertionException` and `FailedAssertionException<T>`) for detailed error handling.
- Built -in methods for common assertions:
  - `isTrue` and `isFalse` — checks boolean conditions.
 
   - `isNotNull` and `areNotNull` — checks for `null` references.
 
   - `isEqual` — compares expected vs.actual values.
 
   - `expensiveIsTrue` — evaluates conditions based on a function result.

## Installation

 Add this code to your project by copying the `Assert.cs` file from this repository into your project structure.
 
## Usage

 Use the methods provided in `EyE.Debug.Assert` to perform assertions in your code.Example usage:

```csharp
public class AClass
{
    object ref1;
    object ref2;

    public void SomeMethod()
    {
        EyE.Debug.Assert.areNotNull<AClass>("A reference is null", ref1, ref2);
        // Further logic
    }
}
```
### Assertions

The following assertions are available:

- **Boolean Assertions**
  - `isTrue(bool condition, string message, object context = null)`: Verifies that a condition is true.
  - `isFalse(bool condition, string message)`: Verifies that a condition is false.
  
- **Null Checks**
  - `isNotNull(object obj, string message)`: Verifies that an object is not null.
  - `areNotNull(string message, params object[] objs)`: Verifies that none of the specified objects are null.
  
- **Equality Check**
  - `isEqual<TValueType>(TValueType expected, TValueType actual, string message)`: Verifies that two values are equal.
