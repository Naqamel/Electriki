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

using System.Net;
using System.Net.NetworkInformation;

namespace electriki
{
    public class TcpRow
    {
        
        private IPEndPoint localIP;
        private IPEndPoint remoteIP;
        private TcpState state;
        private int processId;
        public TcpRow (IPHlpAPI.TcpRow tcpRow)
        {
            this.state = tcpRow.state;
            this.processId = tcpRow.owningPid;

            int localPort = (tcpRow.localPort1 << 8) + (tcpRow.localPort2) + (tcpRow.localPort3 << 24) + (tcpRow.localPort4 << 16);
            long localAddress = tcpRow.localAddr;
            this.localIP = new IPEndPoint (localAddress, localPort);

            int remotePort = (tcpRow.remotePort1 << 8) + (tcpRow.remotePort2) + (tcpRow.remotePort3 << 24) + (tcpRow.remotePort4 << 16);
            long remoteAddress = tcpRow.remoteAddr;
            this.remoteIP = new IPEndPoint (remoteAddress, remotePort);
        }
              
        public IPEndPoint LocalEndPoint
        {
            get { return this.localIP; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return this.remoteIP; }
        }

        public TcpState State
        {
            get { return this.state; }
        }

        public int ProcessId
        {
            get { return this.processId; }
        }
    }
}


