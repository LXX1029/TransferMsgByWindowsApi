using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataStruct
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SendData
    {
        public int Id;
        public string Name;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        /// <summary>
        /// User defined data to be passed to the receiving application.
        /// </summary>
        public IntPtr dwData;

        /// <summary>
        /// The size, in bytes, of the data pointed to by the lpData member.
        /// </summary>
        public int cbData;
        /// <summary>
        /// The data to be passed to the receiving application. This member can be IntPtr.Zero.
        /// </summary>
        public IntPtr lpData;
    }
}
