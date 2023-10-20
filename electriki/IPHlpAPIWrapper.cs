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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace electriki
{
    public static class IPHlpAPIWrapper
    {
        public static List<TcpRow> GetExtendedTcpTable (bool sorted)
        {
            List<TcpRow> tcpRows = new List<TcpRow> ();

            IntPtr tcpTable = IntPtr.Zero;
            int tcpTableLength = 0;

            if (IPHlpAPI.GetExtendedTcpTable (tcpTable, ref tcpTableLength, sorted, IPHlpAPI.AFINET, IPHlpAPI.TcpTableType.OwnerPidAll, 0) != 0)
            {
                try
                {
                    tcpTable = Marshal.AllocHGlobal (tcpTableLength);
                    if (IPHlpAPI.GetExtendedTcpTable (tcpTable, ref tcpTableLength, true, IPHlpAPI.AFINET, IPHlpAPI.TcpTableType.OwnerPidAll, 0) == 0)
                    {
                        IPHlpAPI.TcpTable table = (IPHlpAPI.TcpTable)Marshal.PtrToStructure (tcpTable, typeof (IPHlpAPI.TcpTable));
                        IntPtr rowPtr = (IntPtr)((long)tcpTable + Marshal.SizeOf (table.length));
                        for (int i = 0; i < table.length; ++i)
                        {
                            tcpRows.Add (new TcpRow ((IPHlpAPI.TcpRow)Marshal.PtrToStructure (rowPtr, typeof (IPHlpAPI.TcpRow))));
                            rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf (typeof (IPHlpAPI.TcpRow)));
                        }
                    }
                }
                finally
                {
                    if (tcpTable != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal (tcpTable);
                    }
                }
            }
            return tcpRows;
        }
    }
}
