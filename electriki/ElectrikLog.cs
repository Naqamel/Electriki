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
using System.IO;

namespace electriki
{
    public class ElectrikLog
    {
        private String m_LogPath;

        public ElectrikLog ()
        {
            BuildLogFilePath ();
        }

        public void WriteStringLine (string s)
        {
            File.AppendAllText (m_LogPath, s);
            File.AppendAllText (m_LogPath, "\r\n");
        }

        public void FlushAndReset ()
        {
            BuildLogFilePath ();
        }

        private void BuildLogFilePath ()
        {
            string currentDir = Directory.GetCurrentDirectory();

            DateTime dt = DateTime.Now;
            m_LogPath = string.Format ("{0}\\eye{1}-{2}.log", currentDir, dt.ToString ("yyyyMMdd"), dt.ToString ("HH"));
        }
    }
}
