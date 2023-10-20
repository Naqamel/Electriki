/*
This file is part of Electriki, Copyright 2023 by Naqamel.

Electriki is free software: you can redistribute it and/or modify it under the terms 
of the GNU General Public License as published by the Free Software Foundation, 
either version 3 of the License, or (at your option) any later version.

Electriki is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with Electriki. 
If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace electriki
{
    public static class IPHlpAPI
    {
        public const int AFINET = 2;

        [DllImport ("iphlpapi.dll", SetLastError = true)]
        public static extern uint GetExtendedTcpTable (IntPtr tcpTable, ref int tcpTableLength, bool sort, int ipVersion, TcpTableType tcpTableType, int reserved);

        [DllImport ("iphlpapi.dll", SetLastError = true)]
        public static extern int GetExtendedUdpTable (byte[] pUdpTable, out int dwOutBufLen, bool sort, 
            int ipVersion, UDP_TABLE_CLASS tblClass, int reserved);

        public enum UDP_TABLE_CLASS
        {
            UDP_TABLE_BASIC,
            UDP_TABLE_OWNER_PID,
            UDP_TABLE_OWNER_MODULE
        }
        [StructLayout (LayoutKind.Sequential)]
        public struct MIB_UDP6ROW_OWNER_PID
        {
            [MarshalAs (UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] localAddr;
            public uint localScopeId;
            [MarshalAs (UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] localPort;

            public long LocalScopeId
            {
                get { return localScopeId; }
            }

            public IPAddress LocalAddress
            {
                get { return new IPAddress (localAddr, LocalScopeId); }
            }

            public ushort LocalPort
            {
                get { return BitConverter.ToUInt16 (localPort.Take (2).Reverse ().ToArray (), 0); }
            }
        }
        [StructLayout (LayoutKind.Sequential)]
        public struct MIB_UDP6TABLE_OWNER_PID
        {
            public uint dwNumEntries;
            [MarshalAs (UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1)]
            public MIB_UDP6ROW_OWNER_PID[] table;
        }
        [StructLayout (LayoutKind.Sequential)]
        public struct MIB_UDPTABLE_OWNER_PID
        {
            public uint dwNumEntries;
            [MarshalAs (UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1)]
            public MIB_UDPROW_OWNER_PID[] table;
        }
        [StructLayout (LayoutKind.Sequential)]
        public struct MIB_UDPROW_OWNER_PID
        {
            public uint localAddr;
            [MarshalAs (UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] localPort;
            public uint owningPid;

            public uint ProcessId
            {
                get { return owningPid; }
            }

            public IPAddress LocalAddress
            {
                get { return new IPAddress (localAddr); }
            }

            public ushort LocalPort
            {
                get
                {
                    return BitConverter.ToUInt16 (new byte[2] { localPort[1], localPort[0] }, 0);
                }
            }
        }

        public enum TcpTableType
        {
            BasicListener,
            BasicConnections,
            BasicAll,
            OwnerPidListener,
            OwnerPidConnections,
            OwnerPidAll,
            OwnerModuleListener,
            OwnerModuleConnections,
            OwnerModuleAll,
        }

        [StructLayout (LayoutKind.Sequential)]
        public struct TcpTable
        {
            public uint length;
            public TcpRow row;
        }

        [StructLayout (LayoutKind.Sequential)]
        public struct TcpRow
        {
            public TcpState state;
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public uint remoteAddr;
            public byte remotePort1;
            public byte remotePort2;
            public byte remotePort3;
            public byte remotePort4;
            public int owningPid;
        }
    }
}
