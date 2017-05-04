using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace C0reExternalBase_v2
{
    class Memory
    {
        public class ManageMemory
        {
            public static Process m_Process;
            public static IntPtr m_pProcessHandle;

            public static int m_iNumberOfBytesRead = 0;
            public static int m_iNumberOfBytesWritten = 0;

            public static void Initialize(string ProcessName)
            {

                // Check if csgo.exe is running
                if (Process.GetProcessesByName(ProcessName).Length > 0)
                    m_Process = Process.GetProcessesByName(ProcessName)[0];
                else
                {
                    MessageBox.Show("Couldn't find Counter-Strike. Please start it first!", "Process not found!", MessageBoxButtons.OK);
                    Environment.Exit(1);
                }
                m_pProcessHandle = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, m_Process.Id); // Sets Our ProcessHandle
            }

            public static int GetModuleAdress(string ModuleName)
            {
                try
                {
                    foreach (ProcessModule ProcMod in m_Process.Modules)
                    {
                        if (!ModuleName.Contains(".dll"))
                            ModuleName = ModuleName.Insert(ModuleName.Length, ".dll");

                        if (ModuleName == ProcMod.ModuleName)
                        {
                            return (int)ProcMod.BaseAddress;
                        }
                    }
                }
                catch { }
                return -1;
            }

            public static T ReadMemory<T>(int Adress) where T : struct
            {
                int ByteSize = Marshal.SizeOf(typeof(T)); // Get ByteSize Of DataType
                byte[] buffer = new byte[ByteSize]; // Create A Buffer With Size Of ByteSize
                ReadProcessMemory((int)m_pProcessHandle, Adress, buffer, buffer.Length, ref m_iNumberOfBytesRead); // Read Value From Memory

                return ByteArrayToStructure<T>(buffer); // Transform the ByteArray to The Desired DataType
            }

            public static float[] ReadMatrix<T>(int Adress, int MatrixSize) where T : struct
            {
                int ByteSize = Marshal.SizeOf(typeof(T));
                byte[] buffer = new byte[ByteSize * MatrixSize]; // Create A Buffer With Size Of ByteSize * MatrixSize
                ReadProcessMemory((int)m_pProcessHandle, Adress, buffer, buffer.Length, ref m_iNumberOfBytesRead);

                return ConvertToFloatArray(buffer); // Transform the ByteArray to A Float Array (PseudoMatrix ;P)
            }

            public static void WriteMemory<T>(int Adress, object Value)
            {
                byte[] buffer = StructureToByteArray(Value); // Transform Data To ByteArray 

                WriteProcessMemory((int)m_pProcessHandle, Adress, buffer, buffer.Length, out m_iNumberOfBytesWritten);
            }

            public static void WriteMemory<T>(int Adress, char[] Value)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(Value);

                WriteProcessMemory((int)m_pProcessHandle, Adress, buffer, buffer.Length, out m_iNumberOfBytesWritten);
            }

            #region Transformation
            public static float[] ConvertToFloatArray(byte[] bytes)
            {
                if (bytes.Length % 4 != 0)
                    throw new ArgumentException();

                float[] floats = new float[bytes.Length / 4];

                for (int i = 0; i < floats.Length; i++)
                    floats[i] = BitConverter.ToSingle(bytes, i * 4);

                return floats;
            }

            private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
            {
                var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                try
                {
                    return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                }
                finally
                {
                    handle.Free();
                }
            }

            private static byte[] StructureToByteArray(object obj)
            {
                int len = Marshal.SizeOf(obj);

                byte[] arr = new byte[len];

                IntPtr ptr = Marshal.AllocHGlobal(len);

                Marshal.StructureToPtr(obj, ptr, true);
                Marshal.Copy(ptr, arr, 0, len);
                Marshal.FreeHGlobal(ptr);

                return arr;
            }
            #endregion

            #region DllImports

            [DllImport("kernel32.dll")]
            private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll")]
            private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref int lpNumberOfBytesRead);

            [DllImport("kernel32.dll")]
            private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);
            #endregion

            #region Constants

            const int PROCESS_VM_OPERATION = 0x0008;
            const int PROCESS_VM_READ = 0x0010;
            const int PROCESS_VM_WRITE = 0x0020;

            #endregion
        }
    }
}
