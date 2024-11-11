# EyE.Debug.Assert

EyE.Debug.Assert is a C# library providing assertion methods designed for debugging. This utility helps validate conditions and outputs meaningful error messages, simplifying issue identification during development. All assertions are conditionally compiled and are only included in `DEBUG` builds, making them ideal for internal sanity checks.

## Features

- Provides type - specific assertion failure messages via generic and non-generic assertion methods.
- Custom exceptions (`FailedAssertionException` and `FailedAssertionException<T>`) for detailed error handling.
- Built -in methods for common assertions:
  - `isTrue` and `isFalse` — checks boolean conditions.
 
   - `isNotNull` and `areNotNull` — checks for `null` references.
 
   - `isEqual` — compares expected vs.actual values.
 
   - `expensiveIsTrue` — evaluates conditions based on a function result.

### Dependencies

- .NET Framework

## Installation

Install this package in your Unity project using the Package Manager:

1. Open the Package Manager window (**Packages** > **Manage Packages**).
2. Click on the **+** button in the top left corner and select **Add package from git URL**.
3. Paste the following URL into the address field and click **Install**: https://github.com/glurth/SerializableType.git

 
## Usage

 Use the methods provided in `EyE.Debug.Assert` to perform assertions in your code.  The ``DEBUG`` define *must* be set for the assertions to work.

 The generic methods are provided for convenience only:  The specified type is used for type name display purposes, rather than type safety.  The exception is the ``Assert.Is<T>`` function, which asserts that the object is of the type specified.

 Most methods have an optional "context" object parameter, which may be included as a parameter.  The object's ToString() method will be used to generate text for display purposes.
 

Example usage:

```csharp
    public class AClass : MonoBehaviour
    {
        public float val = 0;
        public GameObject ref1;
        public GameObject ref2;
        public TestObj testObj;
        public DataS testStruct;
        public List<int> intList = new List<int>();

        public void Start()
        {
            Debug.Log("Assertions are active: " + Assert.IsActive());
        }
        public void Update()
        {
            Assert.isTrue<AClass>(val >= 0, "Cannot get Sqrt of negative numbers"); //the typename AClass, with be mentioned in potential output.
            float f = Mathf.Sqrt(val);
            Assert.isFalse(f == 0, "Cannot divide by zero", this); //the ToString for this class will be called, and mentioned in potential output.
            f = 1f / f;

            int valToAdd = 749;//SHOULD not be in list
                               // intList.Add(749);// uh oh!!
            Assert.expensiveIsTrue<AClass>(() => { return !intList.Contains(valToAdd); }
                                               , "Cannot added existing items [" + valToAdd + "] to list", this);//the typename for AClass AND the ToString for this class, with be mentioned in potential output.
                                                                                                                 //intList.Add(valToAdd);

            Assert.isNotNull<TestObj>(testObj,"ref is null "); //the typename AClass, with be mentioned in potential output.
            
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
