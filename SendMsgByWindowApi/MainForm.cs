using DataStruct;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMsgByWindowApi
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            var random = new Random();

            while (true)
            {
                IntPtr ptrCopyData = IntPtr.Zero;
                var copyData = new COPYDATASTRUCT();
                try
                {
                    var hwnd = FindWindow(null, "ReceiveMsgForm");
                    if (hwnd != IntPtr.Zero)
                    {
                        var num = random.Next(1, 100);
                        // 发送结构数据
                        var sendData = new SendData();
                        sendData.Id = num;
                        sendData.Name = "hello";
                        var sendDataSize = Marshal.SizeOf(sendData);

                        if (num % 2 == 0)
                        {
                            string numStr = num.ToString();
                            // 偶数
                            copyData.dwData = new IntPtr(0);
                            copyData.cbData = numStr.Length + 1;
                            copyData.lpData = Marshal.StringToHGlobalAnsi(numStr);
                        }
                        else
                        {
                            // 奇数
                            copyData.dwData = new IntPtr(1);
                            #region 结构体-string
                            //using var ms = new MemoryStream();
                            //ms.Position = 0;
                            //ms.Seek(0, SeekOrigin.Begin);
                            //var binaryFormat = new BinaryFormatter();
                            //binaryFormat.Serialize(ms, sendData);
                            //var sendBytes = ms.ToArray();
                            //var sendStr = System.Convert.ToBase64String(ms.ToArray());
                            //copyData.cbData = sendStr.Length;
                            //copyData.lpData = Marshal.StringToHGlobalAnsi(sendStr);
                            #endregion

                            #region 结构体-byte数组
                            using var ms = new MemoryStream();
                            var formatter = new BinaryFormatter();
                            formatter.Serialize(ms, sendData);
                            var length = (int)ms.Length;
                            copyData.cbData = length;
                            copyData.lpData = Marshal.AllocHGlobal(length);
                            Marshal.Copy(ms.ToArray(), 0, copyData.lpData, length);
                            #endregion

                        }

                        ptrCopyData = Marshal.AllocHGlobal(Marshal.SizeOf(copyData));
                        Marshal.StructureToPtr(copyData, ptrCopyData, false);

                        SendMessage(hwnd, WM_COPYDATA, IntPtr.Zero, ptrCopyData);
                        System.Diagnostics.Debug.WriteLine($"已发送num:{num}");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (ptrCopyData != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ptrCopyData);
                    }
                    if (copyData.lpData != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(copyData.lpData);
                    }
                }
                await Task.Delay(100);
                //System.Threading.Thread.Sleep(1000);
            }
        }


        static uint WM_COPYDATA = 0x004A;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
