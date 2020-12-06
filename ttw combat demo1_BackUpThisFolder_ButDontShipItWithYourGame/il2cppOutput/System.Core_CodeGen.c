#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif



#include "codegen/il2cpp-codegen-metadata.h"





IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END




// 0x00000001 System.Exception System.Linq.Error::ArgumentNull(System.String)
extern void Error_ArgumentNull_m0EDA0D46D72CA692518E3E2EB75B48044D8FD41E (void);
// 0x00000002 System.Exception System.Linq.Error::ArgumentOutOfRange(System.String)
extern void Error_ArgumentOutOfRange_m2EFB999454161A6B48F8DAC3753FDC190538F0F2 (void);
// 0x00000003 System.Exception System.Linq.Error::MoreThanOneMatch()
extern void Error_MoreThanOneMatch_m4C4756AF34A76EF12F3B2B6D8C78DE547F0FBCF8 (void);
// 0x00000004 System.Exception System.Linq.Error::NoElements()
extern void Error_NoElements_mB89E91246572F009281D79730950808F17C3F353 (void);
// 0x00000005 System.Exception System.Linq.Error::NotSupported()
extern void Error_NotSupported_m51A0560ABF374B66CF6D1208DAF27C4CBAD9AABA (void);
// 0x00000006 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable::Where(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000007 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::Select(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TResult>)
// 0x00000008 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::Select(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`3<TSource,System.Int32,TResult>)
// 0x00000009 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::SelectIterator(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`3<TSource,System.Int32,TResult>)
// 0x0000000A System.Func`2<TSource,System.Boolean> System.Linq.Enumerable::CombinePredicates(System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,System.Boolean>)
// 0x0000000B System.Func`2<TSource,TResult> System.Linq.Enumerable::CombineSelectors(System.Func`2<TSource,TMiddle>,System.Func`2<TMiddle,TResult>)
// 0x0000000C System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::SelectMany(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Collections.Generic.IEnumerable`1<TResult>>)
// 0x0000000D System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::SelectManyIterator(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Collections.Generic.IEnumerable`1<TResult>>)
// 0x0000000E System.Linq.IOrderedEnumerable`1<TSource> System.Linq.Enumerable::OrderBy(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>)
// 0x0000000F System.Linq.IOrderedEnumerable`1<TSource> System.Linq.Enumerable::ThenBy(System.Linq.IOrderedEnumerable`1<TSource>,System.Func`2<TSource,TKey>)
// 0x00000010 System.Collections.Generic.IEnumerable`1<System.Linq.IGrouping`2<TKey,TSource>> System.Linq.Enumerable::GroupBy(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>)
// 0x00000011 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable::Distinct(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000012 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable::DistinctIterator(System.Collections.Generic.IEnumerable`1<TSource>,System.Collections.Generic.IEqualityComparer`1<TSource>)
// 0x00000013 TSource[] System.Linq.Enumerable::ToArray(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000014 System.Collections.Generic.List`1<TSource> System.Linq.Enumerable::ToList(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000015 TSource System.Linq.Enumerable::First(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000016 TSource System.Linq.Enumerable::FirstOrDefault(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000017 TSource System.Linq.Enumerable::Last(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000018 TSource System.Linq.Enumerable::SingleOrDefault(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000019 TSource System.Linq.Enumerable::ElementAt(System.Collections.Generic.IEnumerable`1<TSource>,System.Int32)
// 0x0000001A System.Boolean System.Linq.Enumerable::Any(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x0000001B System.Boolean System.Linq.Enumerable::Any(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x0000001C System.Int32 System.Linq.Enumerable::Count(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x0000001D System.Int32 System.Linq.Enumerable::Count(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x0000001E System.Boolean System.Linq.Enumerable::Contains(System.Collections.Generic.IEnumerable`1<TSource>,TSource)
// 0x0000001F System.Boolean System.Linq.Enumerable::Contains(System.Collections.Generic.IEnumerable`1<TSource>,TSource,System.Collections.Generic.IEqualityComparer`1<TSource>)
// 0x00000020 System.Void System.Linq.Enumerable_Iterator`1::.ctor()
// 0x00000021 TSource System.Linq.Enumerable_Iterator`1::get_Current()
// 0x00000022 System.Linq.Enumerable_Iterator`1<TSource> System.Linq.Enumerable_Iterator`1::Clone()
// 0x00000023 System.Void System.Linq.Enumerable_Iterator`1::Dispose()
// 0x00000024 System.Collections.Generic.IEnumerator`1<TSource> System.Linq.Enumerable_Iterator`1::GetEnumerator()
// 0x00000025 System.Boolean System.Linq.Enumerable_Iterator`1::MoveNext()
// 0x00000026 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable_Iterator`1::Select(System.Func`2<TSource,TResult>)
// 0x00000027 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable_Iterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x00000028 System.Object System.Linq.Enumerable_Iterator`1::System.Collections.IEnumerator.get_Current()
// 0x00000029 System.Collections.IEnumerator System.Linq.Enumerable_Iterator`1::System.Collections.IEnumerable.GetEnumerator()
// 0x0000002A System.Void System.Linq.Enumerable_Iterator`1::System.Collections.IEnumerator.Reset()
// 0x0000002B System.Void System.Linq.Enumerable_WhereEnumerableIterator`1::.ctor(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x0000002C System.Linq.Enumerable_Iterator`1<TSource> System.Linq.Enumerable_WhereEnumerableIterator`1::Clone()
// 0x0000002D System.Void System.Linq.Enumerable_WhereEnumerableIterator`1::Dispose()
// 0x0000002E System.Boolean System.Linq.Enumerable_WhereEnumerableIterator`1::MoveNext()
// 0x0000002F System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable_WhereEnumerableIterator`1::Select(System.Func`2<TSource,TResult>)
// 0x00000030 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable_WhereEnumerableIterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x00000031 System.Void System.Linq.Enumerable_WhereArrayIterator`1::.ctor(TSource[],System.Func`2<TSource,System.Boolean>)
// 0x00000032 System.Linq.Enumerable_Iterator`1<TSource> System.Linq.Enumerable_WhereArrayIterator`1::Clone()
// 0x00000033 System.Boolean System.Linq.Enumerable_WhereArrayIterator`1::MoveNext()
// 0x00000034 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable_WhereArrayIterator`1::Select(System.Func`2<TSource,TResult>)
// 0x00000035 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable_WhereArrayIterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x00000036 System.Void System.Linq.Enumerable_WhereListIterator`1::.ctor(System.Collections.Generic.List`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000037 System.Linq.Enumerable_Iterator`1<TSource> System.Linq.Enumerable_WhereListIterator`1::Clone()
// 0x00000038 System.Boolean System.Linq.Enumerable_WhereListIterator`1::MoveNext()
// 0x00000039 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable_WhereListIterator`1::Select(System.Func`2<TSource,TResult>)
// 0x0000003A System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable_WhereListIterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x0000003B System.Void System.Linq.Enumerable_WhereSelectEnumerableIterator`2::.ctor(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,TResult>)
// 0x0000003C System.Linq.Enumerable_Iterator`1<TResult> System.Linq.Enumerable_WhereSelectEnumerableIterator`2::Clone()
// 0x0000003D System.Void System.Linq.Enumerable_WhereSelectEnumerableIterator`2::Dispose()
// 0x0000003E System.Boolean System.Linq.Enumerable_WhereSelectEnumerableIterator`2::MoveNext()
// 0x0000003F System.Collections.Generic.IEnumerable`1<TResult2> System.Linq.Enumerable_WhereSelectEnumerableIterator`2::Select(System.Func`2<TResult,TResult2>)
// 0x00000040 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable_WhereSelectEnumerableIterator`2::Where(System.Func`2<TResult,System.Boolean>)
// 0x00000041 System.Void System.Linq.Enumerable_WhereSelectArrayIterator`2::.ctor(TSource[],System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,TResult>)
// 0x00000042 System.Linq.Enumerable_Iterator`1<TResult> System.Linq.Enumerable_WhereSelectArrayIterator`2::Clone()
// 0x00000043 System.Boolean System.Linq.Enumerable_WhereSelectArrayIterator`2::MoveNext()
// 0x00000044 System.Collections.Generic.IEnumerable`1<TResult2> System.Linq.Enumerable_WhereSelectArrayIterator`2::Select(System.Func`2<TResult,TResult2>)
// 0x00000045 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable_WhereSelectArrayIterator`2::Where(System.Func`2<TResult,System.Boolean>)
// 0x00000046 System.Void System.Linq.Enumerable_WhereSelectListIterator`2::.ctor(System.Collections.Generic.List`1<TSource>,System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,TResult>)
// 0x00000047 System.Linq.Enumerable_Iterator`1<TResult> System.Linq.Enumerable_WhereSelectListIterator`2::Clone()
// 0x00000048 System.Boolean System.Linq.Enumerable_WhereSelectListIterator`2::MoveNext()
// 0x00000049 System.Collections.Generic.IEnumerable`1<TResult2> System.Linq.Enumerable_WhereSelectListIterator`2::Select(System.Func`2<TResult,TResult2>)
// 0x0000004A System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable_WhereSelectListIterator`2::Where(System.Func`2<TResult,System.Boolean>)
// 0x0000004B System.Void System.Linq.Enumerable_<SelectIterator>d__5`2::.ctor(System.Int32)
// 0x0000004C System.Void System.Linq.Enumerable_<SelectIterator>d__5`2::System.IDisposable.Dispose()
// 0x0000004D System.Boolean System.Linq.Enumerable_<SelectIterator>d__5`2::MoveNext()
// 0x0000004E System.Void System.Linq.Enumerable_<SelectIterator>d__5`2::<>m__Finally1()
// 0x0000004F TResult System.Linq.Enumerable_<SelectIterator>d__5`2::System.Collections.Generic.IEnumerator<TResult>.get_Current()
// 0x00000050 System.Void System.Linq.Enumerable_<SelectIterator>d__5`2::System.Collections.IEnumerator.Reset()
// 0x00000051 System.Object System.Linq.Enumerable_<SelectIterator>d__5`2::System.Collections.IEnumerator.get_Current()
// 0x00000052 System.Collections.Generic.IEnumerator`1<TResult> System.Linq.Enumerable_<SelectIterator>d__5`2::System.Collections.Generic.IEnumerable<TResult>.GetEnumerator()
// 0x00000053 System.Collections.IEnumerator System.Linq.Enumerable_<SelectIterator>d__5`2::System.Collections.IEnumerable.GetEnumerator()
// 0x00000054 System.Void System.Linq.Enumerable_<>c__DisplayClass6_0`1::.ctor()
// 0x00000055 System.Boolean System.Linq.Enumerable_<>c__DisplayClass6_0`1::<CombinePredicates>b__0(TSource)
// 0x00000056 System.Void System.Linq.Enumerable_<>c__DisplayClass7_0`3::.ctor()
// 0x00000057 TResult System.Linq.Enumerable_<>c__DisplayClass7_0`3::<CombineSelectors>b__0(TSource)
// 0x00000058 System.Void System.Linq.Enumerable_<SelectManyIterator>d__17`2::.ctor(System.Int32)
// 0x00000059 System.Void System.Linq.Enumerable_<SelectManyIterator>d__17`2::System.IDisposable.Dispose()
// 0x0000005A System.Boolean System.Linq.Enumerable_<SelectManyIterator>d__17`2::MoveNext()
// 0x0000005B System.Void System.Linq.Enumerable_<SelectManyIterator>d__17`2::<>m__Finally1()
// 0x0000005C System.Void System.Linq.Enumerable_<SelectManyIterator>d__17`2::<>m__Finally2()
// 0x0000005D TResult System.Linq.Enumerable_<SelectManyIterator>d__17`2::System.Collections.Generic.IEnumerator<TResult>.get_Current()
// 0x0000005E System.Void System.Linq.Enumerable_<SelectManyIterator>d__17`2::System.Collections.IEnumerator.Reset()
// 0x0000005F System.Object System.Linq.Enumerable_<SelectManyIterator>d__17`2::System.Collections.IEnumerator.get_Current()
// 0x00000060 System.Collections.Generic.IEnumerator`1<TResult> System.Linq.Enumerable_<SelectManyIterator>d__17`2::System.Collections.Generic.IEnumerable<TResult>.GetEnumerator()
// 0x00000061 System.Collections.IEnumerator System.Linq.Enumerable_<SelectManyIterator>d__17`2::System.Collections.IEnumerable.GetEnumerator()
// 0x00000062 System.Void System.Linq.Enumerable_<DistinctIterator>d__68`1::.ctor(System.Int32)
// 0x00000063 System.Void System.Linq.Enumerable_<DistinctIterator>d__68`1::System.IDisposable.Dispose()
// 0x00000064 System.Boolean System.Linq.Enumerable_<DistinctIterator>d__68`1::MoveNext()
// 0x00000065 System.Void System.Linq.Enumerable_<DistinctIterator>d__68`1::<>m__Finally1()
// 0x00000066 TSource System.Linq.Enumerable_<DistinctIterator>d__68`1::System.Collections.Generic.IEnumerator<TSource>.get_Current()
// 0x00000067 System.Void System.Linq.Enumerable_<DistinctIterator>d__68`1::System.Collections.IEnumerator.Reset()
// 0x00000068 System.Object System.Linq.Enumerable_<DistinctIterator>d__68`1::System.Collections.IEnumerator.get_Current()
// 0x00000069 System.Collections.Generic.IEnumerator`1<TSource> System.Linq.Enumerable_<DistinctIterator>d__68`1::System.Collections.Generic.IEnumerable<TSource>.GetEnumerator()
// 0x0000006A System.Collections.IEnumerator System.Linq.Enumerable_<DistinctIterator>d__68`1::System.Collections.IEnumerable.GetEnumerator()
// 0x0000006B System.Func`2<TElement,TElement> System.Linq.IdentityFunction`1::get_Instance()
// 0x0000006C System.Void System.Linq.IdentityFunction`1_<>c::.cctor()
// 0x0000006D System.Void System.Linq.IdentityFunction`1_<>c::.ctor()
// 0x0000006E TElement System.Linq.IdentityFunction`1_<>c::<get_Instance>b__1_0(TElement)
// 0x0000006F System.Linq.IOrderedEnumerable`1<TElement> System.Linq.IOrderedEnumerable`1::CreateOrderedEnumerable(System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean)
// 0x00000070 System.Linq.Lookup`2<TKey,TElement> System.Linq.Lookup`2::Create(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>,System.Func`2<TSource,TElement>,System.Collections.Generic.IEqualityComparer`1<TKey>)
// 0x00000071 System.Void System.Linq.Lookup`2::.ctor(System.Collections.Generic.IEqualityComparer`1<TKey>)
// 0x00000072 System.Collections.Generic.IEnumerator`1<System.Linq.IGrouping`2<TKey,TElement>> System.Linq.Lookup`2::GetEnumerator()
// 0x00000073 System.Collections.IEnumerator System.Linq.Lookup`2::System.Collections.IEnumerable.GetEnumerator()
// 0x00000074 System.Int32 System.Linq.Lookup`2::InternalGetHashCode(TKey)
// 0x00000075 System.Linq.Lookup`2_Grouping<TKey,TElement> System.Linq.Lookup`2::GetGrouping(TKey,System.Boolean)
// 0x00000076 System.Void System.Linq.Lookup`2::Resize()
// 0x00000077 System.Void System.Linq.Lookup`2_Grouping::Add(TElement)
// 0x00000078 System.Collections.Generic.IEnumerator`1<TElement> System.Linq.Lookup`2_Grouping::GetEnumerator()
// 0x00000079 System.Collections.IEnumerator System.Linq.Lookup`2_Grouping::System.Collections.IEnumerable.GetEnumerator()
// 0x0000007A System.Int32 System.Linq.Lookup`2_Grouping::System.Collections.Generic.ICollection<TElement>.get_Count()
// 0x0000007B System.Boolean System.Linq.Lookup`2_Grouping::System.Collections.Generic.ICollection<TElement>.get_IsReadOnly()
// 0x0000007C System.Void System.Linq.Lookup`2_Grouping::System.Collections.Generic.ICollection<TElement>.Add(TElement)
// 0x0000007D System.Void System.Linq.Lookup`2_Grouping::System.Collections.Generic.ICollection<TElement>.Clear()
// 0x0000007E System.Boolean System.Linq.Lookup`2_Grouping::System.Collections.Generic.ICollection<TElement>.Contains(TElement)
// 0x0000007F System.Void System.Linq.Lookup`2_Grouping::System.Collections.Generic.ICollection<TElement>.CopyTo(TElement[],System.Int32)
// 0x00000080 System.Boolean System.Linq.Lookup`2_Grouping::System.Collections.Generic.ICollection<TElement>.Remove(TElement)
// 0x00000081 System.Int32 System.Linq.Lookup`2_Grouping::System.Collections.Generic.IList<TElement>.IndexOf(TElement)
// 0x00000082 System.Void System.Linq.Lookup`2_Grouping::System.Collections.Generic.IList<TElement>.Insert(System.Int32,TElement)
// 0x00000083 System.Void System.Linq.Lookup`2_Grouping::System.Collections.Generic.IList<TElement>.RemoveAt(System.Int32)
// 0x00000084 TElement System.Linq.Lookup`2_Grouping::System.Collections.Generic.IList<TElement>.get_Item(System.Int32)
// 0x00000085 System.Void System.Linq.Lookup`2_Grouping::System.Collections.Generic.IList<TElement>.set_Item(System.Int32,TElement)
// 0x00000086 System.Void System.Linq.Lookup`2_Grouping::.ctor()
// 0x00000087 System.Void System.Linq.Lookup`2_Grouping_<GetEnumerator>d__7::.ctor(System.Int32)
// 0x00000088 System.Void System.Linq.Lookup`2_Grouping_<GetEnumerator>d__7::System.IDisposable.Dispose()
// 0x00000089 System.Boolean System.Linq.Lookup`2_Grouping_<GetEnumerator>d__7::MoveNext()
// 0x0000008A TElement System.Linq.Lookup`2_Grouping_<GetEnumerator>d__7::System.Collections.Generic.IEnumerator<TElement>.get_Current()
// 0x0000008B System.Void System.Linq.Lookup`2_Grouping_<GetEnumerator>d__7::System.Collections.IEnumerator.Reset()
// 0x0000008C System.Object System.Linq.Lookup`2_Grouping_<GetEnumerator>d__7::System.Collections.IEnumerator.get_Current()
// 0x0000008D System.Void System.Linq.Lookup`2_<GetEnumerator>d__12::.ctor(System.Int32)
// 0x0000008E System.Void System.Linq.Lookup`2_<GetEnumerator>d__12::System.IDisposable.Dispose()
// 0x0000008F System.Boolean System.Linq.Lookup`2_<GetEnumerator>d__12::MoveNext()
// 0x00000090 System.Linq.IGrouping`2<TKey,TElement> System.Linq.Lookup`2_<GetEnumerator>d__12::System.Collections.Generic.IEnumerator<System.Linq.IGrouping<TKey,TElement>>.get_Current()
// 0x00000091 System.Void System.Linq.Lookup`2_<GetEnumerator>d__12::System.Collections.IEnumerator.Reset()
// 0x00000092 System.Object System.Linq.Lookup`2_<GetEnumerator>d__12::System.Collections.IEnumerator.get_Current()
// 0x00000093 System.Void System.Linq.Set`1::.ctor(System.Collections.Generic.IEqualityComparer`1<TElement>)
// 0x00000094 System.Boolean System.Linq.Set`1::Add(TElement)
// 0x00000095 System.Boolean System.Linq.Set`1::Find(TElement,System.Boolean)
// 0x00000096 System.Void System.Linq.Set`1::Resize()
// 0x00000097 System.Int32 System.Linq.Set`1::InternalGetHashCode(TElement)
// 0x00000098 System.Void System.Linq.GroupedEnumerable`3::.ctor(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>,System.Func`2<TSource,TElement>,System.Collections.Generic.IEqualityComparer`1<TKey>)
// 0x00000099 System.Collections.Generic.IEnumerator`1<System.Linq.IGrouping`2<TKey,TElement>> System.Linq.GroupedEnumerable`3::GetEnumerator()
// 0x0000009A System.Collections.IEnumerator System.Linq.GroupedEnumerable`3::System.Collections.IEnumerable.GetEnumerator()
// 0x0000009B System.Collections.Generic.IEnumerator`1<TElement> System.Linq.OrderedEnumerable`1::GetEnumerator()
// 0x0000009C System.Linq.EnumerableSorter`1<TElement> System.Linq.OrderedEnumerable`1::GetEnumerableSorter(System.Linq.EnumerableSorter`1<TElement>)
// 0x0000009D System.Collections.IEnumerator System.Linq.OrderedEnumerable`1::System.Collections.IEnumerable.GetEnumerator()
// 0x0000009E System.Linq.IOrderedEnumerable`1<TElement> System.Linq.OrderedEnumerable`1::System.Linq.IOrderedEnumerable<TElement>.CreateOrderedEnumerable(System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean)
// 0x0000009F System.Void System.Linq.OrderedEnumerable`1::.ctor()
// 0x000000A0 System.Void System.Linq.OrderedEnumerable`1_<GetEnumerator>d__1::.ctor(System.Int32)
// 0x000000A1 System.Void System.Linq.OrderedEnumerable`1_<GetEnumerator>d__1::System.IDisposable.Dispose()
// 0x000000A2 System.Boolean System.Linq.OrderedEnumerable`1_<GetEnumerator>d__1::MoveNext()
// 0x000000A3 TElement System.Linq.OrderedEnumerable`1_<GetEnumerator>d__1::System.Collections.Generic.IEnumerator<TElement>.get_Current()
// 0x000000A4 System.Void System.Linq.OrderedEnumerable`1_<GetEnumerator>d__1::System.Collections.IEnumerator.Reset()
// 0x000000A5 System.Object System.Linq.OrderedEnumerable`1_<GetEnumerator>d__1::System.Collections.IEnumerator.get_Current()
// 0x000000A6 System.Void System.Linq.OrderedEnumerable`2::.ctor(System.Collections.Generic.IEnumerable`1<TElement>,System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean)
// 0x000000A7 System.Linq.EnumerableSorter`1<TElement> System.Linq.OrderedEnumerable`2::GetEnumerableSorter(System.Linq.EnumerableSorter`1<TElement>)
// 0x000000A8 System.Void System.Linq.EnumerableSorter`1::ComputeKeys(TElement[],System.Int32)
// 0x000000A9 System.Int32 System.Linq.EnumerableSorter`1::CompareKeys(System.Int32,System.Int32)
// 0x000000AA System.Int32[] System.Linq.EnumerableSorter`1::Sort(TElement[],System.Int32)
// 0x000000AB System.Void System.Linq.EnumerableSorter`1::QuickSort(System.Int32[],System.Int32,System.Int32)
// 0x000000AC System.Void System.Linq.EnumerableSorter`1::.ctor()
// 0x000000AD System.Void System.Linq.EnumerableSorter`2::.ctor(System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean,System.Linq.EnumerableSorter`1<TElement>)
// 0x000000AE System.Void System.Linq.EnumerableSorter`2::ComputeKeys(TElement[],System.Int32)
// 0x000000AF System.Int32 System.Linq.EnumerableSorter`2::CompareKeys(System.Int32,System.Int32)
// 0x000000B0 System.Void System.Linq.Buffer`1::.ctor(System.Collections.Generic.IEnumerable`1<TElement>)
// 0x000000B1 TElement[] System.Linq.Buffer`1::ToArray()
// 0x000000B2 System.Void System.Collections.Generic.HashSet`1::.ctor()
// 0x000000B3 System.Void System.Collections.Generic.HashSet`1::.ctor(System.Collections.Generic.IEqualityComparer`1<T>)
// 0x000000B4 System.Void System.Collections.Generic.HashSet`1::.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
// 0x000000B5 System.Void System.Collections.Generic.HashSet`1::System.Collections.Generic.ICollection<T>.Add(T)
// 0x000000B6 System.Void System.Collections.Generic.HashSet`1::Clear()
// 0x000000B7 System.Boolean System.Collections.Generic.HashSet`1::Contains(T)
// 0x000000B8 System.Void System.Collections.Generic.HashSet`1::CopyTo(T[],System.Int32)
// 0x000000B9 System.Boolean System.Collections.Generic.HashSet`1::Remove(T)
// 0x000000BA System.Int32 System.Collections.Generic.HashSet`1::get_Count()
// 0x000000BB System.Boolean System.Collections.Generic.HashSet`1::System.Collections.Generic.ICollection<T>.get_IsReadOnly()
// 0x000000BC System.Collections.Generic.HashSet`1_Enumerator<T> System.Collections.Generic.HashSet`1::GetEnumerator()
// 0x000000BD System.Collections.Generic.IEnumerator`1<T> System.Collections.Generic.HashSet`1::System.Collections.Generic.IEnumerable<T>.GetEnumerator()
// 0x000000BE System.Collections.IEnumerator System.Collections.Generic.HashSet`1::System.Collections.IEnumerable.GetEnumerator()
// 0x000000BF System.Void System.Collections.Generic.HashSet`1::GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
// 0x000000C0 System.Void System.Collections.Generic.HashSet`1::OnDeserialization(System.Object)
// 0x000000C1 System.Boolean System.Collections.Generic.HashSet`1::Add(T)
// 0x000000C2 System.Void System.Collections.Generic.HashSet`1::CopyTo(T[])
// 0x000000C3 System.Void System.Collections.Generic.HashSet`1::CopyTo(T[],System.Int32,System.Int32)
// 0x000000C4 System.Void System.Collections.Generic.HashSet`1::Initialize(System.Int32)
// 0x000000C5 System.Void System.Collections.Generic.HashSet`1::IncreaseCapacity()
// 0x000000C6 System.Void System.Collections.Generic.HashSet`1::SetCapacity(System.Int32)
// 0x000000C7 System.Boolean System.Collections.Generic.HashSet`1::AddIfNotPresent(T)
// 0x000000C8 System.Int32 System.Collections.Generic.HashSet`1::InternalGetHashCode(T)
// 0x000000C9 System.Void System.Collections.Generic.HashSet`1_Enumerator::.ctor(System.Collections.Generic.HashSet`1<T>)
// 0x000000CA System.Void System.Collections.Generic.HashSet`1_Enumerator::Dispose()
// 0x000000CB System.Boolean System.Collections.Generic.HashSet`1_Enumerator::MoveNext()
// 0x000000CC T System.Collections.Generic.HashSet`1_Enumerator::get_Current()
// 0x000000CD System.Object System.Collections.Generic.HashSet`1_Enumerator::System.Collections.IEnumerator.get_Current()
// 0x000000CE System.Void System.Collections.Generic.HashSet`1_Enumerator::System.Collections.IEnumerator.Reset()
// 0x000000CF System.Void System.Collections.Generic.ICollectionDebugView`1::.ctor(System.Collections.Generic.ICollection`1<T>)
// 0x000000D0 T[] System.Collections.Generic.ICollectionDebugView`1::get_Items()
static Il2CppMethodPointer s_methodPointers[208] = 
{
	Error_ArgumentNull_m0EDA0D46D72CA692518E3E2EB75B48044D8FD41E,
	Error_ArgumentOutOfRange_m2EFB999454161A6B48F8DAC3753FDC190538F0F2,
	Error_MoreThanOneMatch_m4C4756AF34A76EF12F3B2B6D8C78DE547F0FBCF8,
	Error_NoElements_mB89E91246572F009281D79730950808F17C3F353,
	Error_NotSupported_m51A0560ABF374B66CF6D1208DAF27C4CBAD9AABA,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
};
static const int32_t s_InvokerIndices[208] = 
{
	0,
	0,
	4,
	4,
	4,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
};
static const Il2CppTokenRangePair s_rgctxIndices[62] = 
{
	{ 0x02000004, { 84, 4 } },
	{ 0x02000005, { 88, 9 } },
	{ 0x02000006, { 99, 7 } },
	{ 0x02000007, { 108, 10 } },
	{ 0x02000008, { 120, 11 } },
	{ 0x02000009, { 134, 9 } },
	{ 0x0200000A, { 146, 12 } },
	{ 0x0200000B, { 161, 9 } },
	{ 0x0200000C, { 170, 1 } },
	{ 0x0200000D, { 171, 2 } },
	{ 0x0200000E, { 173, 12 } },
	{ 0x0200000F, { 185, 11 } },
	{ 0x02000010, { 196, 4 } },
	{ 0x02000011, { 200, 3 } },
	{ 0x02000014, { 203, 17 } },
	{ 0x02000015, { 224, 5 } },
	{ 0x02000016, { 229, 1 } },
	{ 0x02000018, { 230, 8 } },
	{ 0x0200001A, { 238, 4 } },
	{ 0x0200001B, { 242, 3 } },
	{ 0x0200001C, { 247, 5 } },
	{ 0x0200001D, { 252, 7 } },
	{ 0x0200001E, { 259, 3 } },
	{ 0x0200001F, { 262, 7 } },
	{ 0x02000020, { 269, 4 } },
	{ 0x02000021, { 273, 21 } },
	{ 0x02000023, { 294, 2 } },
	{ 0x02000024, { 296, 2 } },
	{ 0x06000006, { 0, 10 } },
	{ 0x06000007, { 10, 10 } },
	{ 0x06000008, { 20, 1 } },
	{ 0x06000009, { 21, 2 } },
	{ 0x0600000A, { 23, 5 } },
	{ 0x0600000B, { 28, 5 } },
	{ 0x0600000C, { 33, 1 } },
	{ 0x0600000D, { 34, 2 } },
	{ 0x0600000E, { 36, 2 } },
	{ 0x0600000F, { 38, 1 } },
	{ 0x06000010, { 39, 4 } },
	{ 0x06000011, { 43, 1 } },
	{ 0x06000012, { 44, 2 } },
	{ 0x06000013, { 46, 3 } },
	{ 0x06000014, { 49, 2 } },
	{ 0x06000015, { 51, 4 } },
	{ 0x06000016, { 55, 3 } },
	{ 0x06000017, { 58, 4 } },
	{ 0x06000018, { 62, 3 } },
	{ 0x06000019, { 65, 3 } },
	{ 0x0600001A, { 68, 1 } },
	{ 0x0600001B, { 69, 3 } },
	{ 0x0600001C, { 72, 2 } },
	{ 0x0600001D, { 74, 3 } },
	{ 0x0600001E, { 77, 2 } },
	{ 0x0600001F, { 79, 5 } },
	{ 0x0600002F, { 97, 2 } },
	{ 0x06000034, { 106, 2 } },
	{ 0x06000039, { 118, 2 } },
	{ 0x0600003F, { 131, 3 } },
	{ 0x06000044, { 143, 3 } },
	{ 0x06000049, { 158, 3 } },
	{ 0x06000070, { 220, 4 } },
	{ 0x0600009E, { 245, 2 } },
};
static const Il2CppRGCTXDefinition s_rgctxValues[298] = 
{
	{ (Il2CppRGCTXDataType)2, 13712 },
	{ (Il2CppRGCTXDataType)3, 15689 },
	{ (Il2CppRGCTXDataType)2, 13713 },
	{ (Il2CppRGCTXDataType)2, 13714 },
	{ (Il2CppRGCTXDataType)3, 15690 },
	{ (Il2CppRGCTXDataType)2, 13715 },
	{ (Il2CppRGCTXDataType)2, 13716 },
	{ (Il2CppRGCTXDataType)3, 15691 },
	{ (Il2CppRGCTXDataType)2, 13717 },
	{ (Il2CppRGCTXDataType)3, 15692 },
	{ (Il2CppRGCTXDataType)2, 13718 },
	{ (Il2CppRGCTXDataType)3, 15693 },
	{ (Il2CppRGCTXDataType)2, 13719 },
	{ (Il2CppRGCTXDataType)2, 13720 },
	{ (Il2CppRGCTXDataType)3, 15694 },
	{ (Il2CppRGCTXDataType)2, 13721 },
	{ (Il2CppRGCTXDataType)2, 13722 },
	{ (Il2CppRGCTXDataType)3, 15695 },
	{ (Il2CppRGCTXDataType)2, 13723 },
	{ (Il2CppRGCTXDataType)3, 15696 },
	{ (Il2CppRGCTXDataType)3, 15697 },
	{ (Il2CppRGCTXDataType)2, 13724 },
	{ (Il2CppRGCTXDataType)3, 15698 },
	{ (Il2CppRGCTXDataType)2, 13725 },
	{ (Il2CppRGCTXDataType)3, 15699 },
	{ (Il2CppRGCTXDataType)3, 15700 },
	{ (Il2CppRGCTXDataType)2, 5654 },
	{ (Il2CppRGCTXDataType)3, 15701 },
	{ (Il2CppRGCTXDataType)2, 13726 },
	{ (Il2CppRGCTXDataType)3, 15702 },
	{ (Il2CppRGCTXDataType)3, 15703 },
	{ (Il2CppRGCTXDataType)2, 5661 },
	{ (Il2CppRGCTXDataType)3, 15704 },
	{ (Il2CppRGCTXDataType)3, 15705 },
	{ (Il2CppRGCTXDataType)2, 13727 },
	{ (Il2CppRGCTXDataType)3, 15706 },
	{ (Il2CppRGCTXDataType)2, 13728 },
	{ (Il2CppRGCTXDataType)3, 15707 },
	{ (Il2CppRGCTXDataType)3, 15708 },
	{ (Il2CppRGCTXDataType)3, 15709 },
	{ (Il2CppRGCTXDataType)2, 13729 },
	{ (Il2CppRGCTXDataType)2, 13730 },
	{ (Il2CppRGCTXDataType)3, 15710 },
	{ (Il2CppRGCTXDataType)3, 15711 },
	{ (Il2CppRGCTXDataType)2, 13731 },
	{ (Il2CppRGCTXDataType)3, 15712 },
	{ (Il2CppRGCTXDataType)2, 13732 },
	{ (Il2CppRGCTXDataType)3, 15713 },
	{ (Il2CppRGCTXDataType)3, 15714 },
	{ (Il2CppRGCTXDataType)2, 5697 },
	{ (Il2CppRGCTXDataType)3, 15715 },
	{ (Il2CppRGCTXDataType)2, 13733 },
	{ (Il2CppRGCTXDataType)2, 13734 },
	{ (Il2CppRGCTXDataType)2, 5698 },
	{ (Il2CppRGCTXDataType)2, 13735 },
	{ (Il2CppRGCTXDataType)2, 5700 },
	{ (Il2CppRGCTXDataType)2, 13736 },
	{ (Il2CppRGCTXDataType)3, 15716 },
	{ (Il2CppRGCTXDataType)2, 13737 },
	{ (Il2CppRGCTXDataType)2, 13738 },
	{ (Il2CppRGCTXDataType)2, 5703 },
	{ (Il2CppRGCTXDataType)2, 13739 },
	{ (Il2CppRGCTXDataType)2, 5705 },
	{ (Il2CppRGCTXDataType)2, 13740 },
	{ (Il2CppRGCTXDataType)3, 15717 },
	{ (Il2CppRGCTXDataType)2, 13741 },
	{ (Il2CppRGCTXDataType)2, 5708 },
	{ (Il2CppRGCTXDataType)2, 13742 },
	{ (Il2CppRGCTXDataType)2, 5710 },
	{ (Il2CppRGCTXDataType)2, 5712 },
	{ (Il2CppRGCTXDataType)2, 13743 },
	{ (Il2CppRGCTXDataType)3, 15718 },
	{ (Il2CppRGCTXDataType)2, 13744 },
	{ (Il2CppRGCTXDataType)2, 5715 },
	{ (Il2CppRGCTXDataType)2, 5717 },
	{ (Il2CppRGCTXDataType)2, 13745 },
	{ (Il2CppRGCTXDataType)3, 15719 },
	{ (Il2CppRGCTXDataType)2, 13746 },
	{ (Il2CppRGCTXDataType)3, 15720 },
	{ (Il2CppRGCTXDataType)3, 15721 },
	{ (Il2CppRGCTXDataType)2, 13747 },
	{ (Il2CppRGCTXDataType)2, 5722 },
	{ (Il2CppRGCTXDataType)2, 13748 },
	{ (Il2CppRGCTXDataType)2, 5724 },
	{ (Il2CppRGCTXDataType)3, 15722 },
	{ (Il2CppRGCTXDataType)3, 15723 },
	{ (Il2CppRGCTXDataType)2, 5727 },
	{ (Il2CppRGCTXDataType)3, 15724 },
	{ (Il2CppRGCTXDataType)3, 15725 },
	{ (Il2CppRGCTXDataType)2, 5739 },
	{ (Il2CppRGCTXDataType)2, 13749 },
	{ (Il2CppRGCTXDataType)3, 15726 },
	{ (Il2CppRGCTXDataType)3, 15727 },
	{ (Il2CppRGCTXDataType)2, 5741 },
	{ (Il2CppRGCTXDataType)2, 13581 },
	{ (Il2CppRGCTXDataType)3, 15728 },
	{ (Il2CppRGCTXDataType)3, 15729 },
	{ (Il2CppRGCTXDataType)2, 13750 },
	{ (Il2CppRGCTXDataType)3, 15730 },
	{ (Il2CppRGCTXDataType)3, 15731 },
	{ (Il2CppRGCTXDataType)2, 5751 },
	{ (Il2CppRGCTXDataType)2, 13751 },
	{ (Il2CppRGCTXDataType)3, 15732 },
	{ (Il2CppRGCTXDataType)3, 15733 },
	{ (Il2CppRGCTXDataType)3, 15143 },
	{ (Il2CppRGCTXDataType)3, 15734 },
	{ (Il2CppRGCTXDataType)2, 13752 },
	{ (Il2CppRGCTXDataType)3, 15735 },
	{ (Il2CppRGCTXDataType)3, 15736 },
	{ (Il2CppRGCTXDataType)2, 5763 },
	{ (Il2CppRGCTXDataType)2, 13753 },
	{ (Il2CppRGCTXDataType)3, 15737 },
	{ (Il2CppRGCTXDataType)3, 15738 },
	{ (Il2CppRGCTXDataType)3, 15739 },
	{ (Il2CppRGCTXDataType)3, 15740 },
	{ (Il2CppRGCTXDataType)3, 15741 },
	{ (Il2CppRGCTXDataType)3, 15149 },
	{ (Il2CppRGCTXDataType)3, 15742 },
	{ (Il2CppRGCTXDataType)2, 13754 },
	{ (Il2CppRGCTXDataType)3, 15743 },
	{ (Il2CppRGCTXDataType)3, 15744 },
	{ (Il2CppRGCTXDataType)2, 5776 },
	{ (Il2CppRGCTXDataType)2, 13755 },
	{ (Il2CppRGCTXDataType)3, 15745 },
	{ (Il2CppRGCTXDataType)3, 15746 },
	{ (Il2CppRGCTXDataType)2, 5778 },
	{ (Il2CppRGCTXDataType)2, 13756 },
	{ (Il2CppRGCTXDataType)3, 15747 },
	{ (Il2CppRGCTXDataType)3, 15748 },
	{ (Il2CppRGCTXDataType)2, 13757 },
	{ (Il2CppRGCTXDataType)3, 15749 },
	{ (Il2CppRGCTXDataType)3, 15750 },
	{ (Il2CppRGCTXDataType)2, 13758 },
	{ (Il2CppRGCTXDataType)3, 15751 },
	{ (Il2CppRGCTXDataType)3, 15752 },
	{ (Il2CppRGCTXDataType)2, 5793 },
	{ (Il2CppRGCTXDataType)2, 13759 },
	{ (Il2CppRGCTXDataType)3, 15753 },
	{ (Il2CppRGCTXDataType)3, 15754 },
	{ (Il2CppRGCTXDataType)3, 15755 },
	{ (Il2CppRGCTXDataType)3, 15160 },
	{ (Il2CppRGCTXDataType)2, 13760 },
	{ (Il2CppRGCTXDataType)3, 15756 },
	{ (Il2CppRGCTXDataType)3, 15757 },
	{ (Il2CppRGCTXDataType)2, 13761 },
	{ (Il2CppRGCTXDataType)3, 15758 },
	{ (Il2CppRGCTXDataType)3, 15759 },
	{ (Il2CppRGCTXDataType)2, 5809 },
	{ (Il2CppRGCTXDataType)2, 13762 },
	{ (Il2CppRGCTXDataType)3, 15760 },
	{ (Il2CppRGCTXDataType)3, 15761 },
	{ (Il2CppRGCTXDataType)3, 15762 },
	{ (Il2CppRGCTXDataType)3, 15763 },
	{ (Il2CppRGCTXDataType)3, 15764 },
	{ (Il2CppRGCTXDataType)3, 15765 },
	{ (Il2CppRGCTXDataType)3, 15166 },
	{ (Il2CppRGCTXDataType)2, 13763 },
	{ (Il2CppRGCTXDataType)3, 15766 },
	{ (Il2CppRGCTXDataType)3, 15767 },
	{ (Il2CppRGCTXDataType)2, 13764 },
	{ (Il2CppRGCTXDataType)3, 15768 },
	{ (Il2CppRGCTXDataType)3, 15769 },
	{ (Il2CppRGCTXDataType)2, 13765 },
	{ (Il2CppRGCTXDataType)2, 13766 },
	{ (Il2CppRGCTXDataType)3, 15770 },
	{ (Il2CppRGCTXDataType)3, 15771 },
	{ (Il2CppRGCTXDataType)2, 5826 },
	{ (Il2CppRGCTXDataType)2, 13767 },
	{ (Il2CppRGCTXDataType)3, 15772 },
	{ (Il2CppRGCTXDataType)3, 15773 },
	{ (Il2CppRGCTXDataType)3, 15774 },
	{ (Il2CppRGCTXDataType)3, 15775 },
	{ (Il2CppRGCTXDataType)3, 15776 },
	{ (Il2CppRGCTXDataType)3, 15777 },
	{ (Il2CppRGCTXDataType)3, 15778 },
	{ (Il2CppRGCTXDataType)2, 13768 },
	{ (Il2CppRGCTXDataType)2, 13769 },
	{ (Il2CppRGCTXDataType)3, 15779 },
	{ (Il2CppRGCTXDataType)2, 5857 },
	{ (Il2CppRGCTXDataType)2, 5851 },
	{ (Il2CppRGCTXDataType)3, 15780 },
	{ (Il2CppRGCTXDataType)2, 5850 },
	{ (Il2CppRGCTXDataType)2, 13770 },
	{ (Il2CppRGCTXDataType)3, 15781 },
	{ (Il2CppRGCTXDataType)3, 15782 },
	{ (Il2CppRGCTXDataType)3, 15783 },
	{ (Il2CppRGCTXDataType)2, 13771 },
	{ (Il2CppRGCTXDataType)3, 15784 },
	{ (Il2CppRGCTXDataType)2, 5873 },
	{ (Il2CppRGCTXDataType)2, 5865 },
	{ (Il2CppRGCTXDataType)3, 15785 },
	{ (Il2CppRGCTXDataType)3, 15786 },
	{ (Il2CppRGCTXDataType)2, 5864 },
	{ (Il2CppRGCTXDataType)2, 13772 },
	{ (Il2CppRGCTXDataType)3, 15787 },
	{ (Il2CppRGCTXDataType)3, 15788 },
	{ (Il2CppRGCTXDataType)2, 13773 },
	{ (Il2CppRGCTXDataType)3, 15789 },
	{ (Il2CppRGCTXDataType)2, 5877 },
	{ (Il2CppRGCTXDataType)3, 15790 },
	{ (Il2CppRGCTXDataType)2, 13774 },
	{ (Il2CppRGCTXDataType)3, 15791 },
	{ (Il2CppRGCTXDataType)2, 13774 },
	{ (Il2CppRGCTXDataType)2, 5905 },
	{ (Il2CppRGCTXDataType)3, 15792 },
	{ (Il2CppRGCTXDataType)3, 15793 },
	{ (Il2CppRGCTXDataType)3, 15794 },
	{ (Il2CppRGCTXDataType)3, 15795 },
	{ (Il2CppRGCTXDataType)2, 13775 },
	{ (Il2CppRGCTXDataType)2, 13776 },
	{ (Il2CppRGCTXDataType)2, 13777 },
	{ (Il2CppRGCTXDataType)3, 15796 },
	{ (Il2CppRGCTXDataType)3, 15797 },
	{ (Il2CppRGCTXDataType)2, 5901 },
	{ (Il2CppRGCTXDataType)2, 5904 },
	{ (Il2CppRGCTXDataType)3, 15798 },
	{ (Il2CppRGCTXDataType)3, 15799 },
	{ (Il2CppRGCTXDataType)2, 5908 },
	{ (Il2CppRGCTXDataType)3, 15800 },
	{ (Il2CppRGCTXDataType)2, 13778 },
	{ (Il2CppRGCTXDataType)2, 5898 },
	{ (Il2CppRGCTXDataType)2, 13779 },
	{ (Il2CppRGCTXDataType)3, 15801 },
	{ (Il2CppRGCTXDataType)3, 15802 },
	{ (Il2CppRGCTXDataType)3, 15803 },
	{ (Il2CppRGCTXDataType)2, 13780 },
	{ (Il2CppRGCTXDataType)3, 15804 },
	{ (Il2CppRGCTXDataType)3, 15805 },
	{ (Il2CppRGCTXDataType)3, 15806 },
	{ (Il2CppRGCTXDataType)2, 5923 },
	{ (Il2CppRGCTXDataType)3, 15807 },
	{ (Il2CppRGCTXDataType)2, 13781 },
	{ (Il2CppRGCTXDataType)2, 13782 },
	{ (Il2CppRGCTXDataType)3, 15808 },
	{ (Il2CppRGCTXDataType)3, 15809 },
	{ (Il2CppRGCTXDataType)2, 5944 },
	{ (Il2CppRGCTXDataType)3, 15810 },
	{ (Il2CppRGCTXDataType)2, 5945 },
	{ (Il2CppRGCTXDataType)3, 15811 },
	{ (Il2CppRGCTXDataType)2, 13783 },
	{ (Il2CppRGCTXDataType)3, 15812 },
	{ (Il2CppRGCTXDataType)3, 15813 },
	{ (Il2CppRGCTXDataType)2, 13784 },
	{ (Il2CppRGCTXDataType)3, 15814 },
	{ (Il2CppRGCTXDataType)3, 15815 },
	{ (Il2CppRGCTXDataType)2, 13785 },
	{ (Il2CppRGCTXDataType)3, 15816 },
	{ (Il2CppRGCTXDataType)2, 13786 },
	{ (Il2CppRGCTXDataType)3, 15817 },
	{ (Il2CppRGCTXDataType)3, 15818 },
	{ (Il2CppRGCTXDataType)3, 15819 },
	{ (Il2CppRGCTXDataType)2, 5980 },
	{ (Il2CppRGCTXDataType)3, 15820 },
	{ (Il2CppRGCTXDataType)2, 5988 },
	{ (Il2CppRGCTXDataType)3, 15821 },
	{ (Il2CppRGCTXDataType)2, 13787 },
	{ (Il2CppRGCTXDataType)2, 13788 },
	{ (Il2CppRGCTXDataType)3, 15822 },
	{ (Il2CppRGCTXDataType)3, 15823 },
	{ (Il2CppRGCTXDataType)3, 15824 },
	{ (Il2CppRGCTXDataType)3, 15825 },
	{ (Il2CppRGCTXDataType)3, 15826 },
	{ (Il2CppRGCTXDataType)3, 15827 },
	{ (Il2CppRGCTXDataType)2, 6004 },
	{ (Il2CppRGCTXDataType)2, 13789 },
	{ (Il2CppRGCTXDataType)3, 15828 },
	{ (Il2CppRGCTXDataType)3, 15829 },
	{ (Il2CppRGCTXDataType)2, 6008 },
	{ (Il2CppRGCTXDataType)3, 15830 },
	{ (Il2CppRGCTXDataType)2, 13790 },
	{ (Il2CppRGCTXDataType)2, 6018 },
	{ (Il2CppRGCTXDataType)2, 6016 },
	{ (Il2CppRGCTXDataType)2, 13791 },
	{ (Il2CppRGCTXDataType)3, 15831 },
	{ (Il2CppRGCTXDataType)2, 13792 },
	{ (Il2CppRGCTXDataType)3, 15832 },
	{ (Il2CppRGCTXDataType)3, 15833 },
	{ (Il2CppRGCTXDataType)3, 15834 },
	{ (Il2CppRGCTXDataType)2, 6022 },
	{ (Il2CppRGCTXDataType)3, 15835 },
	{ (Il2CppRGCTXDataType)3, 15836 },
	{ (Il2CppRGCTXDataType)2, 6025 },
	{ (Il2CppRGCTXDataType)3, 15837 },
	{ (Il2CppRGCTXDataType)1, 13793 },
	{ (Il2CppRGCTXDataType)2, 6024 },
	{ (Il2CppRGCTXDataType)3, 15838 },
	{ (Il2CppRGCTXDataType)1, 6024 },
	{ (Il2CppRGCTXDataType)1, 6022 },
	{ (Il2CppRGCTXDataType)2, 13794 },
	{ (Il2CppRGCTXDataType)2, 6024 },
	{ (Il2CppRGCTXDataType)3, 15839 },
	{ (Il2CppRGCTXDataType)3, 15840 },
	{ (Il2CppRGCTXDataType)3, 15841 },
	{ (Il2CppRGCTXDataType)2, 6023 },
	{ (Il2CppRGCTXDataType)3, 15842 },
	{ (Il2CppRGCTXDataType)2, 6036 },
	{ (Il2CppRGCTXDataType)2, 6047 },
	{ (Il2CppRGCTXDataType)2, 6049 },
};
extern const Il2CppCodeGenModule g_System_CoreCodeGenModule;
const Il2CppCodeGenModule g_System_CoreCodeGenModule = 
{
	"System.Core.dll",
	208,
	s_methodPointers,
	s_InvokerIndices,
	0,
	NULL,
	62,
	s_rgctxIndices,
	298,
	s_rgctxValues,
	NULL,
};
