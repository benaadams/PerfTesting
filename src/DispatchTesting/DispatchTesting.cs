using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace DispatchTest
{
    public class ListVirtual
    {
        protected static readonly int[] _array = new int[256];
        public virtual int this[int offset] => _array[offset];
        public virtual int Count => _array.Length;
    }

    public class ListVirtualNotDerived
    {
        protected static readonly int[] _array = new int[256];
        public virtual int this[int offset] => _array[offset];
        public virtual int Count => _array.Length;
    }

    public class ArrayDerivedList : ListVirtual
    {
        public override int this[int offset] => _array[offset];
        public override int Count => _array.Length;
    }

    public class ListDerivedList : ListVirtual
    {
        private static readonly List<int> _list = new List<int>(_array);
        public override int this[int offset] => _list[offset];
        public override int Count => _list.Count;
    }

    public sealed class ArrayDerivedListSealed : ListVirtual
    {
        public override int this[int offset] => _array[offset];
        public override int Count => _array.Length;
    }

    public sealed class ListDerivedListSealed : ListVirtual
    {
        private static readonly List<int> _list = new List<int>(_array);
        public override int this[int offset] => _list[offset];
        public override int Count => _list.Count;
    }

    public class ArrayListDirect
    {
        private static readonly int[] _array = new int[256];
        public int this[int offset] => _array[offset];
        public int Count => _array.Length;
    }

    public class ListListDirect
    {
        private static readonly int[] _array = new int[256];
        private static readonly List<int> _list = new List<int>(_array);
        public int this[int offset] => _list[offset];
        public int Count => _list.Count;
    }

    public sealed class ArrayListDirectSealed
    {
        private static readonly int[] _array = new int[256];
        public int this[int offset] => _array[offset];
        public int Count => _array.Length;
    }

    public sealed class ListListDirectSealed
    {
        private static readonly int[] _array = new int[256];
        private static readonly List<int> _list = new List<int>(_array);
        public int this[int offset] => _list[offset];
        public int Count => _list.Count;
    }

    [Config(typeof(CoreConfig))]
    public class DispatchTesting
    {
        private const int InnerLoopCount = 2000;

        private static readonly int[] _array = new int[256];
        private static readonly List<int> _list = new List<int>(_array);

        private static readonly IList<int> _iArray = _array;
        private static readonly IList<int> _iList = _list;

        private static readonly ListVirtual _listVirtual = new ListVirtual();
        private static readonly ListVirtualNotDerived _listVirtualNotDerived = new ListVirtualNotDerived();
        private static readonly ArrayDerivedList _arrayDerivedList = new ArrayDerivedList();
        private static readonly ListDerivedList _listDerivedList = new ListDerivedList();

        private static readonly ListVirtual _baseArrayDerivedList = _arrayDerivedList;
        private static readonly ListVirtual _baseListDerivedList = _listDerivedList;

        private static readonly ArrayDerivedListSealed _arrayDerivedListSealed = new ArrayDerivedListSealed();
        private static readonly ListDerivedListSealed _listDerivedListSealed = new ListDerivedListSealed();

        private static readonly ListVirtual _baseArrayDerivedListSealed = _arrayDerivedListSealed;
        private static readonly ListVirtual _baseListDerivedListSealed = _listDerivedListSealed;

        private static readonly ArrayListDirect _arrayListDirect = new ArrayListDirect();
        private static readonly ListListDirect _listListDirect = new ListListDirect();

        private static readonly ArrayListDirectSealed _arrayListDirectSealed = new ArrayListDirectSealed();
        private static readonly ListListDirectSealed _listListDirectSealed = new ListListDirectSealed();


        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int Direct()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _array.Length;
                for (var i = 0; i < count; i++)
                    returnVal += ArrayLoop(_array, i);
                count = _list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ListLoop(_list, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
               var  count = _arrayListDirect.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ArrayListDirectLoop(_arrayListDirect, i);
                count = _listListDirect.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ListListDirectLoop(_listListDirect, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectSealedWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _arrayListDirectSealed.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ArrayListDirectSealedLoop(_arrayListDirectSealed, i);
                count = _listListDirectSealed.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ListListDirectSealedLoop(_listListDirectSealed, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectVirtualWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _arrayDerivedList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ArrayListDirectVirtualLoop(_arrayDerivedList, i);
                count = _listDerivedList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ListListDirectVirtualLoop(_listDerivedList, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectVirtualSealedWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _arrayDerivedListSealed.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ArrayListDirectVirtualSealedLoop(_arrayDerivedListSealed, i);
                count = _listDerivedListSealed.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ListListDirectVirtualSealedLoop(_listDerivedListSealed, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectBaseWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _listVirtual.Count;
                for (var i = 0; i < count; i++)
                    returnVal += DirectBaseWrapperLoop(_listVirtual, i);
                count = _listVirtual.Count;
                for (var i = 0; i < count; i++)
                    returnVal += DirectBaseWrapperLoop(_listVirtual, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectBaseNotDerivedWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _listVirtualNotDerived.Count;
                for (var i = 0; i < count; i++)
                    returnVal += DirectBaseNotDerivedWrapperLoop(_listVirtualNotDerived, i);
                count = _listVirtualNotDerived.Count;
                for (var i = 0; i < count; i++)
                    returnVal += DirectBaseNotDerivedWrapperLoop(_listVirtualNotDerived, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ViaBaseDerivedWrapperMonomorophic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _baseArrayDerivedList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ArrayListViaBaseDerivedWrapperLoop(_baseArrayDerivedList, i);
                count = _baseListDerivedList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ListListViaBaseDerivedWrapperLoop(_baseListDerivedList, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ViaBaseDerivedWrapperPolymorophic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _baseArrayDerivedList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ViaBaseDerivedWrapperLoop(_baseArrayDerivedList, i);
                count = _baseListDerivedList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ViaBaseDerivedWrapperLoop(_baseListDerivedList, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ViaBaseDerivedSealedWrapperMonomorophic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _baseArrayDerivedListSealed.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ArrayListViaBaseDerivedSealedWrapperLoop(_baseArrayDerivedListSealed, i);
                count = _baseListDerivedListSealed.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ListListViaBaseDerivedSealedWrapperLoop(_baseListDerivedListSealed, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ManualDerivedWrapperPolymorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _baseArrayDerivedList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ManualDerivedWrapperPolymorphicLoop(_baseArrayDerivedList, i);
                count = _baseListDerivedList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ManualDerivedWrapperPolymorphicLoop(_baseListDerivedList, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ManualDerivedSealedWrapperPolymorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _baseArrayDerivedListSealed.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ManualDerivedSealedWrapperPolymorphicLoop(_baseArrayDerivedListSealed, i);
                count = _baseListDerivedListSealed.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ManualDerivedSealedWrapperPolymorphicLoop(_baseListDerivedListSealed, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int RuntimeInterfaceMonomorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _iArray.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListLoopArrayOnly(_iArray, i);
                count = _iList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListLoopListOnly(_iList, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int RuntimeInterfacePolymorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _iArray.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListLoop(_iArray, i);
                count = _iList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListLoop(_iList, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectInterfaceAsCast()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _iArray.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ArrayLoop(_iArray as int[], i);
                count = _iList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += ListLoop(_iList as List<int>, i);
            }
            return returnVal;
        }


        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ManualInterfaceMonomorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _iArray.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListArrayLoopAsCast(_iArray, i);
                count = _iList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListListLoopAsCast(_iList, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ManualInterfacePolymorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _iArray.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListLoopAsCast(_iArray, i);
                count = _iList.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListLoopAsCast(_iList, i);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int InterfaceGenericConstraint()
        {
            var returnVal = 0;
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                var count = _array.Length;
                for (var i = 0; i < count; i++)
                    returnVal += IListGenericConstraintLoop(_array, i);
                count = _list.Count;
                for (var i = 0; i < count; i++)
                    returnVal += IListGenericConstraintLoop(_list, i);
            }
            return returnVal;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int ListLoop(List<int> list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int ArrayLoop(int[] array, int i)
        {
            return array[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ListListDirectLoop(ListListDirect list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ArrayListDirectLoop(ArrayListDirect list, int i)
        {
            return list[i];
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ListListDirectSealedLoop(ListListDirectSealed list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ArrayListDirectSealedLoop(ArrayListDirectSealed list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int DirectBaseWrapperLoop(ListVirtual list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int DirectBaseNotDerivedWrapperLoop(ListVirtualNotDerived list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ListListDirectVirtualSealedLoop(ListDerivedListSealed list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ArrayListDirectVirtualSealedLoop(ArrayDerivedListSealed list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ListListDirectVirtualLoop(ListDerivedList list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ArrayListDirectVirtualLoop(ArrayDerivedList list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ArrayListViaBaseDerivedSealedWrapperLoop(ListVirtual list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ListListViaBaseDerivedSealedWrapperLoop(ListVirtual list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ListListViaBaseDerivedWrapperLoop(ListVirtual list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ArrayListViaBaseDerivedWrapperLoop(ListVirtual list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ViaBaseDerivedWrapperLoop(ListVirtual list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ManualDerivedWrapperPolymorphicLoop(ListVirtual list, int i)
        {
            var arrayDerivedList = list as ArrayDerivedList;
            if (arrayDerivedList != null)
            {
                return arrayDerivedList[i];
            }
            var listDerivedList = list as ListDerivedList;
            if (listDerivedList != null)
            {
                return listDerivedList[i];
            }

            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ManualDerivedSealedWrapperPolymorphicLoop(ListVirtual list, int i)
        {
            var arrayDerivedListSealed = list as ArrayDerivedListSealed;
            if (arrayDerivedListSealed != null)
            {
                return arrayDerivedListSealed[i];
            }
            var listDerivedListSealed = list as ListDerivedListSealed;
            if (listDerivedListSealed != null)
            {
                return listDerivedListSealed[i];
            }

            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int IListLoop(IList<int> list, int i)
        {
            return list[i];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int IListLoopArrayOnly(IList<int> list, int i)
        {
            return list[i];
        }

        private static int IListLoopListOnly(IList<int> list, int i)
        {
            return list[i];
        }

        private static int IListArrayLoopAsCast(IList<int> ilist, int i)
        {
            var array = ilist as int[];
            if (array != null)
            {
                return array[i];
            }
            return ilist[i];
        }

        private static int IListListLoopAsCast(IList<int> ilist, int i)
        {
            var list = ilist as List<int>;
            if (list != null)
            {
                return list[i];
            }
            return ilist[i];
        }

        private static int IListGenericConstraintLoop<T>(T list, int i) where T : IList<int>
        {
            return list[i];
        }

        private static int IListLoopAsCast(IList<int> ilist, int i)
        {
            var array = ilist as int[];
            if (array != null)
            {
                return array[i];
            }
            var list = ilist as List<int>;
            if (list != null)
            {
                return list[i];
            }
            return ilist[i];
        }

        [Setup]
        public void Setup()
        {
        }
    }

}
