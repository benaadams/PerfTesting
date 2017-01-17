using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AsciiEncoding
{
    public class ThrowHelper
    {
        private static ArgumentNullException GetArgumentNullException(ExceptionArgument argument, ExceptionResource resource)
        {
            throw new ArgumentNullException(GetArgumentName(argument), GetResourceString(resource));
        }

        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
        {
            return new ArgumentOutOfRangeException(GetArgumentName(argument), GetResourceString(resource));
        }

        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
        {
            throw GetArgumentOutOfRangeException(argument, resource);
        }

        internal static void ThrowArgumentNullException(ExceptionArgument argument, ExceptionResource resource)
        {
            throw GetArgumentNullException(argument, resource);
        }

        // This function will convert an ExceptionArgument enum value to the argument name string.
        private static string GetArgumentName(ExceptionArgument argument)
        {
            // This is indirected through a second NoInlining function it has a special meaning
            // in System.Private.CoreLib of indicatating it takes a StackMark which cause 
            // the caller to also be not inlined; so we can't mark it directly.
            // So is the effect of marking this function as non-inlining in a regular situation.
            return GetArgumentNameInner(argument);
        }

        // This function will convert an ExceptionArgument enum value to the argument name string.
        // Second function in chain so as to not propergate the non-inlining to outside caller
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetArgumentNameInner(ExceptionArgument argument)
        {
            Debug.Assert(Enum.IsDefined(typeof(ExceptionArgument), argument),
                "The enum value is not defined, please check the ExceptionArgument Enum.");

            return argument.ToString();
        }

        // This function will convert an ExceptionResource enum value to the resource string.
        private static string GetResourceString(ExceptionResource resource)
        {
            // This is indirected through a second NoInlining function it has a special meaning
            // in System.Private.CoreLib of indicatating it takes a StackMark which cause 
            // the caller to also be not inlined; so we can't mark it directly.
            // So is the effect of marking this function as non-inlining in a regular situation.
            return GetResourceStringInner(resource);
        }

        // This function will convert an ExceptionResource enum value to the resource string.
        // Second function in chain so as to not propergate the non-inlining to outside caller
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceStringInner(ExceptionResource resource)
        {
            Debug.Assert(Enum.IsDefined(typeof(ExceptionResource), resource),
                "The enum value is not defined, please check the ExceptionResource Enum.");

            return resource.ToString();
        }
    }

    //
    // The convention for this enum is using the argument name as the enum name
    // 
    internal enum ExceptionArgument
    {
        s,
        chars,
        bytes,
        charIndex,
        charCount,
        byteIndex,
        byteCount
    }

    //
    // The convention for this enum is using the resource name as the enum name
    // 
    internal enum ExceptionResource
    {
        ArgumentNull_String,
        ArgumentNull_Array,
        ArgumentOutOfRange_NeedNonNegNum,
        ArgumentOutOfRange_IndexCountBuffer,
        ArgumentOutOfRange_Index
    }
}
