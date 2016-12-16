using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace TypeCacheTest
{

    [Config(typeof(CoreConfig))]
    public class TypeCacheTesting
    {
        private const int InnerLoopCount = 2000;

        private static readonly IList<int>[] _ipolyLists = new IList<int>[34];

        [Benchmark(Baseline = true, OperationsPerInvoke = InnerLoopCount)]
        public int InterfaceMonomorphic()
        {
            var list = _ipolyLists[33];
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListLookup(list, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int InterfacePolymorphicX2()
        {
            var returnVal = AddPolymorpicCallSite(1);
            var list = _ipolyLists[33];
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListLookup(list, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int InterfaceMegamorphicX33()
        {
            var returnVal = AddPolymorpicCallSite(32);
            var list = _ipolyLists[33];
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListLookup(list, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int InterfaceUnsafeCastConstTest()
        {
            var returnVal = 0;
            var list = _ipolyLists[33];
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IUnsafeListLookup(list, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int InterfaceUnsafeCastVarTest()
        {
            var returnVal = 0;
            var list = _ipolyLists[33];
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IUnsafeListVariableLookup(list, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectViaCastIndirected()
        {
            var list = (List33)_ipolyLists[33];
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IndirectedListLookup(list, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectViaCastNotInlined()
        {
            var list = (List33)_ipolyLists[33];
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ListLookup(list, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectViaCastInlined()
        {
            var list = (List33)_ipolyLists[33];
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += InlinedListLookup(list, i);
            }
            return returnVal;
        }

        private int AddPolymorpicCallSite(int count)
        {
            var returnVal = 0;
            for (var i = 0; i < count; i++)
            {
                returnVal += IListLookup(_ipolyLists[i], 0);
            }
            return returnVal;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int IListLookup(IList<int> list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int IUnsafeListLookup(IList<int> list, int i)
        {
            if (list.GetType() == typeof(List33))
                return Unsafe.As<List33>(list)[i];
            return IListLookup(list, i);
        }

        private static Type _type;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int IUnsafeListVariableLookup(IList<int> list, int i)
        {
            if (list.GetType() == _type)
                return Unsafe.As<List33>(list)[i];
            return IListLookup(list, i);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int IndirectedListLookup(List33 list, int i)
        {
            return ListLookup(list, i);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int ListLookup(List33 list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlinedListLookup(List33 list, int i)
        {
            return list[i];
        }

        [Setup]
        public void Setup()
        {
            _ipolyLists[0] = new List0();
            _ipolyLists[1] = new List1();
            _ipolyLists[2] = new List2();
            _ipolyLists[3] = new List3();
            _ipolyLists[4] = new List4();
            _ipolyLists[5] = new List5();
            _ipolyLists[6] = new List6();
            _ipolyLists[7] = new List7();
            _ipolyLists[8] = new List8();
            _ipolyLists[9] = new List9();
            _ipolyLists[10] = new List10();
            _ipolyLists[11] = new List11();
            _ipolyLists[12] = new List12();
            _ipolyLists[13] = new List13();
            _ipolyLists[14] = new List14();
            _ipolyLists[15] = new List15();
            _ipolyLists[16] = new List16();
            _ipolyLists[17] = new List17();
            _ipolyLists[18] = new List18();
            _ipolyLists[19] = new List19();
            _ipolyLists[20] = new List20();
            _ipolyLists[21] = new List21();
            _ipolyLists[22] = new List22();
            _ipolyLists[23] = new List23();
            _ipolyLists[24] = new List24();
            _ipolyLists[25] = new List25();
            _ipolyLists[26] = new List26();
            _ipolyLists[27] = new List27();
            _ipolyLists[28] = new List28();
            _ipolyLists[29] = new List29();
            _ipolyLists[30] = new List30();
            _ipolyLists[31] = new List31();
            _ipolyLists[32] = new List32();
            _ipolyLists[33] = new List33();

            _type = typeof(List33);
        }
    }

    public class List0 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List1 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List2 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List3 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List4 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List5 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List6 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List7 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List8 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List9 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List10 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List11 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List12 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List13 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List14 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List15 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List16 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List17 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List18 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List19 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List20 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List21 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List22 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List23 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List24 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List25 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List26 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List27 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List28 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List29 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List30 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List31 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List32 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class List33 : ListBase, IList<int>
    {
        private static readonly int[] _array = new int[256];

        public int Count => _array.Length;
        public int this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }
    public class ListBase : IEnumerable
    {
        public void Add(int item) { throw new NotImplementedException(); }
        public void Clear() { throw new NotImplementedException(); }
        public bool Contains(int item) { throw new NotImplementedException(); }
        public void CopyTo(int[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(int item) { throw new NotImplementedException(); }
        public bool IsReadOnly => false;
        public IEnumerator<int> GetEnumerator() { throw new NotImplementedException(); }
        IEnumerator IEnumerable.GetEnumerator() { throw new NotImplementedException(); }
        public int IndexOf(int item) { throw new NotImplementedException(); }
        public void Insert(int index, int item) { throw new NotImplementedException(); }
        public void RemoveAt(int index) { throw new NotImplementedException(); }
    }
}
