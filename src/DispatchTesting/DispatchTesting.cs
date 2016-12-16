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
        private const int InnerLoopCount = 256 * 2000;

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
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ArrayLoop(_array);
                returnVal += ListLoop(_list);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ArrayListDirectLoop(_arrayListDirect);
                returnVal += ListListDirectLoop(_listListDirect);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectSealedWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ArrayListDirectSealedLoop(_arrayListDirectSealed);
                returnVal += ListListDirectSealedLoop(_listListDirectSealed);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectVirtualWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ArrayListDirectVirtualLoop(_arrayDerivedList);
                returnVal += ListListDirectVirtualLoop(_listDerivedList);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectVirtualSealedWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ArrayListDirectVirtualSealedLoop(_arrayDerivedListSealed);
                returnVal += ListListDirectVirtualSealedLoop(_listDerivedListSealed);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectBaseWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += DirectBaseWrapperLoop(_listVirtual);
                returnVal += DirectBaseWrapperLoop(_listVirtual);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectBaseNotDerivedWrapper()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += DirectBaseNotDerivedWrapperLoop(_listVirtualNotDerived);
                returnVal += DirectBaseNotDerivedWrapperLoop(_listVirtualNotDerived);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ViaBaseDerivedWrapperMonomorophic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ArrayListViaBaseDerivedWrapperLoop(_baseArrayDerivedList);
                returnVal += ListListViaBaseDerivedWrapperLoop(_baseListDerivedList);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ViaBaseDerivedWrapperPolymorophic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ViaBaseDerivedWrapperLoop(_baseArrayDerivedList);
                returnVal += ViaBaseDerivedWrapperLoop(_baseListDerivedList);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ViaBaseDerivedSealedWrapperMonomorophic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ArrayListViaBaseDerivedSealedWrapperLoop(_baseArrayDerivedListSealed);
                returnVal += ListListViaBaseDerivedSealedWrapperLoop(_baseListDerivedListSealed);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ManualDerivedWrapperPolymorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ManualDerivedWrapperPolymorphicLoop(_baseArrayDerivedList);
                returnVal += ManualDerivedWrapperPolymorphicLoop(_baseListDerivedList);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ManualDerivedSealedWrapperPolymorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ManualDerivedSealedWrapperPolymorphicLoop(_baseArrayDerivedListSealed);
                returnVal += ManualDerivedSealedWrapperPolymorphicLoop(_baseListDerivedListSealed);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int RuntimeInterfaceMonomorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += IListLoopArrayOnly(_iArray);
                returnVal += IListLoopListOnly(_iList);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int RuntimeInterfacePolymorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += IListLoop(_iArray);
                returnVal += IListLoop(_iList);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int DirectInterfaceAsCast()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += ArrayLoop(_iArray as int[]);
                returnVal += ListLoop(_iList as List<int>);
            }
            return returnVal;
        }


        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ManualInterfaceMonomorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += IListArrayLoopAsCast(_iArray);
                returnVal += IListListLoopAsCast(_iList);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int ManualInterfacePolymorphic()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += IListLoopAsCast(_iArray);
                returnVal += IListLoopAsCast(_iList);
            }
            return returnVal;
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public int InterfaceGenericConstraint()
        {
            var returnVal = 0;
            for (var loop = 0; loop < 2000; loop++)
            {
                returnVal += IListGenericConstraintLoop(_array);
                returnVal += IListGenericConstraintLoop(_list);
            }
            return returnVal;
        }

        private static int ListLoop(List<int> list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private static int ArrayLoop(int[] array)
        {
            var returnVal = 0;
            for (var i = 0; i < array.Length; i++)
            {
                returnVal = array[i];
            }
            return returnVal;
        }

        private int ListListDirectLoop(ListListDirect list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ArrayListDirectLoop(ArrayListDirect list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }


        private int ListListDirectSealedLoop(ListListDirectSealed list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ArrayListDirectSealedLoop(ArrayListDirectSealed list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int DirectBaseWrapperLoop(ListVirtual list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int DirectBaseNotDerivedWrapperLoop(ListVirtualNotDerived list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ListListDirectVirtualSealedLoop(ListDerivedListSealed list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ArrayListDirectVirtualSealedLoop(ArrayDerivedListSealed list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ListListDirectVirtualLoop(ListDerivedList list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ArrayListDirectVirtualLoop(ArrayDerivedList list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ArrayListViaBaseDerivedSealedWrapperLoop(ListVirtual list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ListListViaBaseDerivedSealedWrapperLoop(ListVirtual list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ListListViaBaseDerivedWrapperLoop(ListVirtual list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ArrayListViaBaseDerivedWrapperLoop(ListVirtual list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ViaBaseDerivedWrapperLoop(ListVirtual list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private int ManualDerivedWrapperPolymorphicLoop(ListVirtual list)
        {
            var arrayDerivedList = list as ArrayDerivedList;
            if (arrayDerivedList != null)
            {
                return ArrayListDirectVirtualLoop(arrayDerivedList);
            }
            var listDerivedList = list as ListDerivedList;
            if (listDerivedList != null)
            {
                return ListListDirectVirtualLoop(listDerivedList);
            }

            return ViaBaseGeneralDerivedWrapperLoop(list);
        }

        private int ManualDerivedSealedWrapperPolymorphicLoop(ListVirtual list)
        {
            var arrayDerivedListSealed = list as ArrayDerivedListSealed;
            if (arrayDerivedListSealed != null)
            {
                return ArrayListDirectVirtualSealedLoop(arrayDerivedListSealed);
            }
            var listDerivedListSealed = list as ListDerivedListSealed;
            if (listDerivedListSealed != null)
            {
                return ListListDirectVirtualSealedLoop(listDerivedListSealed);
            }

            return ViaBaseGeneralDerivedWrapperLoop(list);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ViaBaseGeneralDerivedWrapperLoop(ListVirtual list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private static int IListLoop(IList<int> list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private static int IListLoopArrayOnly(IList<int> list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private static int IListLoopListOnly(IList<int> list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private static int IListArrayLoopAsCast(IList<int> ilist)
        {
            var array = ilist as int[];
            if (array != null)
            {
                return ArrayLoop(array);
            }
            return IListGeneralLoop(ilist);
        }

        private static int IListListLoopAsCast(IList<int> ilist)
        {
            var list = ilist as List<int>;
            if (list != null)
            {
                return ListLoop(list);
            }
            return IListGeneralLoop(ilist);
        }

        private static int IListGenericConstraintLoop<T>(T list) where T : IList<int>
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }

        private static int IListLoopAsCast(IList<int> ilist)
        {
            var array = ilist as int[];
            if (array != null)
            {
                return ArrayLoop(array);
            }
            var list = ilist as List<int>;
            if (list != null)
            {
                return ListLoop(list);
            }
            return IListGeneralLoop(ilist);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int IListGeneralLoop(IList<int> list)
        {
            var returnVal = 0;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                returnVal = list[i];
            }
            return returnVal;
        }


        [Setup]
        public void Setup()
        {
        }
    }

}
