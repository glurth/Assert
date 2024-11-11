using System;
using System.Collections.Generic;
using UnityEngine;

namespace EyE.Debug.Samples
{
    using Debug = UnityEngine.Debug;

    //[System.Serializable]
    public class TestObj
    {
        public string s = "yo";
    }
    [System.Serializable]
    public struct DataS
    {
        public int i;
    }

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
}