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


using System.ServiceProcess;

namespace electriki
{
    internal static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ElectricEye()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
