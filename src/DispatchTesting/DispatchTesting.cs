using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace DispatchTest
{

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

        private static readonly IList<int>[] _ipolyLists = new IList<int>[34];


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
        public int RuntimeInterfacePolymorphic2()
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
        public int RuntimeInterfacePolymorphic3()
        {
            var returnVal = AddPolymorpicCallSite(1);
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
        public int RuntimeInterfacePolymorphic4()
        {
            var returnVal = AddPolymorpicCallSite(2);
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
        public int RuntimeInterfacePolymorphic5()
        {
            var returnVal = AddPolymorpicCallSite(3);
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
        public int RuntimeInterfacePolymorphic6()
        {
            var returnVal = AddPolymorpicCallSite(4);
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
        public int RuntimeInterfacePolymorphic7()
        {
            var returnVal = AddPolymorpicCallSite(5);
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
        public int RuntimeInterfacePolymorphic8()
        {
            var returnVal = AddPolymorpicCallSite(6);
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
        public int RuntimeInterfacePolymorphic9()
        {
            var returnVal = AddPolymorpicCallSite(7);
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
        public int RuntimeInterfacePolymorphic10()
        {
            var returnVal = AddPolymorpicCallSite(8);
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
        public int RuntimeInterfacePolymorphic11()
        {
            var returnVal = AddPolymorpicCallSite(9);
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
        public int RuntimeInterfacePolymorphic12()
        {
            var returnVal = AddPolymorpicCallSite(10);
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
        public int RuntimeInterfacePolymorphic13()
        {
            var returnVal = AddPolymorpicCallSite(11);
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
        public int RuntimeInterfacePolymorphic14()
        {
            var returnVal = AddPolymorpicCallSite(12);
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
        public int RuntimeInterfacePolymorphic15()
        {
            var returnVal = AddPolymorpicCallSite(13);
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
        public int RuntimeInterfacePolymorphic16()
        {
            var returnVal = AddPolymorpicCallSite(14);
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
        public int RuntimeInterfacePolymorphic17()
        {
            var returnVal = AddPolymorpicCallSite(15);
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
        public int RuntimeInterfacePolymorphic18()
        {
            var returnVal = AddPolymorpicCallSite(16);
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
        public int RuntimeInterfacePolymorphic19()
        {
            var returnVal = AddPolymorpicCallSite(17);
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
        public int RuntimeInterfacePolymorphic20()
        {
            var returnVal = AddPolymorpicCallSite(18);
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
        public int RuntimeInterfacePolymorphic21()
        {
            var returnVal = AddPolymorpicCallSite(19);
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
        public int RuntimeInterfacePolymorphic22()
        {
            var returnVal = AddPolymorpicCallSite(20);
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
        public int RuntimeInterfacePolymorphic23()
        {
            var returnVal = AddPolymorpicCallSite(21);
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
        public int RuntimeInterfacePolymorphic24()
        {
            var returnVal = AddPolymorpicCallSite(22);
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
        public int RuntimeInterfacePolymorphic25()
        {
            var returnVal = AddPolymorpicCallSite(23);
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
        public int RuntimeInterfacePolymorphic26()
        {
            var returnVal = AddPolymorpicCallSite(24);
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
        public int RuntimeInterfacePolymorphic27()
        {
            var returnVal = AddPolymorpicCallSite(25);
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
        public int RuntimeInterfacePolymorphic28()
        {
            var returnVal = AddPolymorpicCallSite(26);
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
        public int RuntimeInterfacePolymorphic29()
        {
            var returnVal = AddPolymorpicCallSite(27);
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
        public int RuntimeInterfacePolymorphic30()
        {
            var returnVal = AddPolymorpicCallSite(28);
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
        public int RuntimeInterfacePolymorphic31()
        {
            var returnVal = AddPolymorpicCallSite(29);
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
        public int RuntimeInterfacePolymorphic32()
        {
            var returnVal = AddPolymorpicCallSite(30);
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
        public int RuntimeInterfacePolymorphic33()
        {
            var returnVal = AddPolymorpicCallSite(31);
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
        public int RuntimeInterfacePolymorphic34()
        {
            var returnVal = AddPolymorpicCallSite(32);
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
        public int RuntimeInterfacePolymorphic35()
        {
            var returnVal = AddPolymorpicCallSite(33);
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
        public int RuntimeInterfacePolymorphic36()
        {
            var returnVal = AddPolymorpicCallSite(34);
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
        private int AddPolymorpicCallSite(int count)
        {
            var returnVal = 0;
            for (var i = 0; i < count; i++)
            {
                returnVal += IListLoop(_ipolyLists[i], 0);
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
        }
    }

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
