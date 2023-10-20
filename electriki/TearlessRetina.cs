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
using System.Diagnostics;

namespace electriki
{
    public class TearlessRetina
    {
        protected HashSet<String> m_KnownIPAddresses;
        public HashSet<String> KnownIPAddresses
        {
            get { return m_KnownIPAddresses; }
        }

        public TearlessRetina ()
        {
            m_KnownIPAddresses= new HashSet<string> ();
        }
        public void GetPidsAndIPs (bool baseline, ElectrikLog logger)
        {
            foreach (TcpRow tcpRow in IPHlpAPIWrapper.GetExtendedTcpTable (true))
            {
                Process process = Process.GetProcessById (tcpRow.ProcessId);
                string remoteIP = tcpRow.RemoteEndPoint.ToString ();
                if (baseline)
                {
                    m_KnownIPAddresses.Clear ();
                }
                if (m_KnownIPAddresses.Count == 0)
                {
                    // then just add it. take no further action
                    m_KnownIPAddresses.Add (remoteIP);
                }
                else
                {
                    // is this a new connection? 
                    if (m_KnownIPAddresses.Contains (remoteIP))
                    {
                        // then we know this one, ignore it as we've already logged it
                    }
                    else
                    {
                        // log it
                        if (DoWeCareAboutThisProcess (process.ProcessName.ToLower ()))
                        {
                            string log = string.Format ("[{0}] Incoming connection detected: Process Name: {1}, Remote IP Address: {2}",
                                DateTime.Now.ToString ("yyyMMdd HH:mm"), process.ProcessName, remoteIP);
                            logger.WriteStringLine (log);
                        }
                        // and add it
                        m_KnownIPAddresses.Add (remoteIP);
                    }
                }
            }
        }

        protected bool DoWeCareAboutThisProcess (string name)
        {
            // black list these, potentially dangerous
            if (name.Contains ("anydesk")){ return true; }
            if (name.Contains ("teamview")) { return true; }
            if (name.Contains ("ultraview")) { return true; }
            if (name.Contains ("supremo")) { return true; }
            if (name.Contains ("screenconn")) { return true; }
            if (name.Contains ("awesun")) { return true; }

            // whitelist these, most likely bengign
            if (name.Contains ("chrome")) { return false;}
            if (name.Contains ("svchost")) { return false; }
            if (name.Contains ("firefox")) { return false; }
            if (name.Contains ("msedge")) { return false; }
            if (name.Contains ("iexplore")) { return false; }
            if (name.Contains ("perfwatson")) { return false; }
            if (name.Contains ("devenv")) { return false; }
            if (name.Contains ("idle")) { return false; }

            // can't tell what it is, go ahead and return true
            return true;
        }
    }
}


