using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

[StructLayout(LayoutKind.Sequential)]
//Struct nhận thông tin hệ thống
public struct SYSTEM_INFO

{

    public uint dwOemId;
    public uint dwPageSize;
    public uint lpMinimumApplicationAddress;
    public uint lpMaximumApplicationAddress;
    public uint dwActiveProcessorMask;
    public uint dwNumberOfProcessors;
    public uint dwProcessorType;
    public uint dwAllocationGranularity;
    public uint dwProcessorLevel;
    public uint dwProcessorRevision;
}

//struct nhận trạng thái bộ nhớ
[StructLayout(LayoutKind.Sequential)]
public struct MEMORYSTATUS
{
    public uint dwLength;
    public uint dwMemoryLoad;
    public uint dwTotalPhys;
    public uint dwAvailPhys;
    public uint dwTotalPageFile;
    public uint dwAvailPageFile;
    public uint dwTotalVirtual;
    public uint dwAvailVirtual;
}

namespace BaiTapLon
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            #region hàm
            try
            {
                SYSTEM_INFO pSI = new SYSTEM_INFO();
                GetSystemInfo(ref pSI);
                string CPUType = "unknown";
                switch (pSI.dwProcessorType)
                {
                    
                    case PROCESSOR_INTEL_386:
                        {
                            CPUType = "Intel 386";
                            break;
                        }

                    case PROCESSOR_INTEL_486:
                        {
                            CPUType = "Intel 486";
                            break;
                        }

                    case PROCESSOR_INTEL_PENTIUM:
                        {
                            CPUType = "Intel Pentium";
                            break;
                        }

                    case PROCESSOR_MIPS_R4000:
                        {
                            CPUType = "MIPS R4000";
                            break;
                        }

                    case PROCESSOR_ALPHA_21064:
                        {
                            CPUType = "DEC Alpha 21064";
                            break;
                        }                       
                }

                //hiển thị thông tin
                listBox1.Items.Insert(0, "Active Processor Mask :		" + pSI.dwActiveProcessorMask.ToString());
                listBox1.Items.Insert(1, "Allocation Granularity :		" + pSI.dwAllocationGranularity.ToString());
                listBox1.Items.Insert(2, "Number Of Processors :		" + pSI.dwNumberOfProcessors.ToString());
                listBox1.Items.Insert(3, "OEM ID :				" + pSI.dwOemId.ToString());
                listBox1.Items.Insert(4, "Page Size :			" + pSI.dwPageSize.ToString());
                // Processor Level (Req filtering to get level)
                listBox1.Items.Insert(5, "Processor Level Value :		" + pSI.dwProcessorLevel.ToString());
                listBox1.Items.Insert(6, "Processor Revision :		" + pSI.dwProcessorRevision.ToString());
                listBox1.Items.Insert(7, "CPU type :			" + CPUType);
                listBox1.Items.Insert(8, "Maximum Application Address :	" + pSI.lpMaximumApplicationAddress.ToString());
                listBox1.Items.Insert(9, "Minimum Application Address :	" + pSI.lpMinimumApplicationAddress.ToString());

                /**************	lấy thông tin từ trạng thái bộ nhớ ****************/

                MEMORYSTATUS memSt = new MEMORYSTATUS();
                GlobalMemoryStatus(ref memSt);

                listBox1.Items.Insert(10, "Available Page File :		" + (memSt.dwAvailPageFile / 1024).ToString());
                listBox1.Items.Insert(11, "Available Physical Memory :		" + (memSt.dwAvailPhys / 1024).ToString());
                listBox1.Items.Insert(12, "Available Virtual Memory :		" + (memSt.dwAvailVirtual / 1024).ToString());
                listBox1.Items.Insert(13, "Size of structur :			" + memSt.dwLength.ToString());
                listBox1.Items.Insert(14, "Memory In Use :			" + memSt.dwMemoryLoad.ToString());
                listBox1.Items.Insert(15, "Total Page Size :			" + (memSt.dwTotalPageFile / 1024).ToString());
                listBox1.Items.Insert(16, "Total Physical Memory :		" + (memSt.dwTotalPhys / 1024).ToString());
                listBox1.Items.Insert(17, "Total Virtual Memory :		" + (memSt.dwTotalVirtual / 1024).ToString());
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
            {
                ListViewItem SessionID = new ListViewItem(theprocess.SessionId.ToString());
                ListViewItem.ListViewSubItem ProcessID = new ListViewItem.ListViewSubItem(SessionID, theprocess.Id.ToString());
                ListViewItem.ListViewSubItem ProcessName = new ListViewItem.ListViewSubItem(SessionID, theprocess.ProcessName.ToString());
                SessionID.SubItems.Add(ProcessID);
                SessionID.SubItems.Add(ProcessName);
                listView1.Items.Add(SessionID);
            }
            #endregion
        }
        //hằng số cho cho các bộ sử lý
        public const int PROCESSOR_INTEL_386 = 386;
        public const int PROCESSOR_INTEL_486 = 486;
        public const int PROCESSOR_INTEL_PENTIUM = 586;
        public const int PROCESSOR_MIPS_R4000 = 4000;
        public const int PROCESSOR_ALPHA_21064 = 21064;

        //lấy thông tin hệ thống
        [DllImport("kernel32")]
        static extern void GetSystemInfo(ref SYSTEM_INFO pSI);

        //lấy trạng thái bộ nhớ
        [DllImport("kernel32")]        
        static extern void GlobalMemoryStatus(ref MEMORYSTATUS buf);

        private void button1_Click(object sender, EventArgs e)
        {
            if(File.Exists("./Thông tin.txt"))
            {
                File.Delete("./Thông tin.txt");
            }
            //tại file txt
            using (StreamWriter ghi = File.CreateText("Thông Tin.txt"))
            {
                foreach(string i in listBox1.Items)
                {
                    ghi.WriteLine(i);
                }
            }
        }
    }
}

