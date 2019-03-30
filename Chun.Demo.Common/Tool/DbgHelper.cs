using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace Chun.Demo.Common
{


    public class DbgHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private struct EXCEPTION_POINTERS
        {
            public IntPtr pExceptionRecord;           // pointer to an EXCEPTION_RECORD struct
            public IntPtr pContextRecord;             // pointer to a CONTEXT struct
        }
     
        /// <summary>
        /// 
        /// </summary>
        private struct EXCEPTION_RECORD
        {
            public uint ExceptionCode;
            public uint ExceptionFlags;
            public IntPtr pExceptionRecord;             //' Pointer to an EXCEPTION_RECORD structure
            public uint ExceptionAddress;
            public uint NumberParameters;             //
            public unsafe fixed int ExceptionInformation[15];
        }
    
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionPointer"></param>
        /// <returns></returns>
        public delegate int UnhandledExceptionFilter(IntPtr exceptionPointer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern Int32 SetUnhandledExceptionFilter(UnhandledExceptionFilter lpTopLevelExceptionFilter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="exceptionCode"></param>
        /// <param name="exceptionAddress"></param>
        public static void GetExceptionInfo(IntPtr exception, out uint exceptionCode, out uint exceptionAddress)
        {
            var info = (EXCEPTION_POINTERS)Marshal.PtrToStructure(exception, typeof(EXCEPTION_POINTERS));
            var record = (EXCEPTION_RECORD)Marshal.PtrToStructure(info.pExceptionRecord, typeof(EXCEPTION_RECORD));

            exceptionCode = record.ExceptionCode;
            exceptionAddress = record.ExceptionAddress;
        }
    }
}