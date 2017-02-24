using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using C0reExternalBase_v2.Structs;

namespace C0reExternalBase_v2
{
    class Memory
    {
        public class ManageMemory
        {
            public static Process m_Process;
            public static IntPtr m_pProcessHandle;
            public static int m_iNumberOfBytesRead = 0;

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
                m_pProcessHandle = OpenProcess(PROCESS_WM_READ, false, m_Process.Id); // Sets Our ProcessHandle
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

            #region Transformation
            public static float[] ConvertToFloatArray(byte[] bytes)
            {
                if (bytes.Length % 4 != 0)
                    throw new ArgumentException();

                float[] floats = new float[bytes.Length / 4];
                for (int i = 0; i < floats.Length; i++)
                {
                    floats[i] = BitConverter.ToSingle(bytes, i * 4);
                }

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
            #endregion

            #region DllImports

            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll")]
            public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

            #endregion

            #region Constants

            const int PROCESS_WM_READ = 0x0010;

            #endregion
        }
    }
}
