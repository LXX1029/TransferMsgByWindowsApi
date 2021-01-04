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
using System.Windows.Forms;

namespace ReceiveMsgByWindowApi
{
    public partial class ReceiveMsgForm : Form
    {
        public ReceiveMsgForm()
        {
            InitializeComponent();
        }
        protected override void WndProc(ref Message m)
        {

            try
            {
                if (m.Msg == WM_COPYDATA)
                {

                    var copyData = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));
                    int dataType = (int)copyData.dwData;
                    if (dataType == 0)
                    {
                        var data = Marshal.PtrToStringAnsi(copyData.lpData);
                        System.Diagnostics.Debug.WriteLine($"====偶数：{data}");
                    }
                    else
                    {
                        #region Struct
                        #region String
                        //string receiveData = Marshal.PtrToStringAnsi(copyData.lpData, 200);
                        //var dataBytes = System.Convert.FromBase64String(receiveData);
                        //var ms = new MemoryStream(dataBytes);
                        //ms.Position = 0;
                        //ms.Seek(0, SeekOrigin.Begin);
                        //var formatter = new BinaryFormatter();
                        //SendData data = (SendData)formatter.Deserialize(ms);
                        #endregion

                        #region 字节数组
                        var dataBytes = new byte[copyData.cbData];
                        Marshal.Copy(copyData.lpData, dataBytes, 0, copyData.cbData);
                        using var ms = new MemoryStream(dataBytes);
                        var formatter = new BinaryFormatter();
                        var data = (SendData)formatter.Deserialize(ms);
                        #endregion
                        System.Diagnostics.Debug.WriteLine($"====奇数：{data.Id}");
                        //System.Diagnostics.Debug.WriteLine($"Bytes length:{dataBytes.Length} Id:{data.Id}-Name:{data.Name}");
                        #endregion
                    }
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            catch (Exception ex)
            {

            }
        }
        static uint WM_COPYDATA = 0x004A;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string path = AppContext.BaseDirectory + @"‪143188.jpg";
                var bytes = File.ReadAllBytes(@"F:\DW\壁纸\143188.jpg");
                using var ms = new MemoryStream(bytes);
                var image = Image.FromStream(ms);
                this.pictureBox1.Image = image;
            }
            catch (Exception ex)
            {

            }

        }
    }
}
