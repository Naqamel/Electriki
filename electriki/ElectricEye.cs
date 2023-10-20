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
using System.Timers;

namespace electriki
{
    public partial class ElectricEye : ServiceBase
    {
        private double m_wakeup;
        private Timer IntervalTimer;
        private TearlessRetina m_Retina;
        private Timer DayTimer;
        private bool baseline;
        private ElectrikLog m_Logger;
        public int counter;

        public ElectricEye ()
        {
            InitializeComponent ();
            m_Retina = new TearlessRetina ();
            m_wakeup = 5000; // 5 seconds
            baseline = true;
            m_Logger = new ElectrikLog ();
            counter = 0;
        }

        protected override void OnStart (string[] args)
        {
            // Establish a baseline of open IP addresses. 
            m_Retina.GetPidsAndIPs (true, m_Logger);
           
            IntervalTimer = new Timer ();
            IntervalTimer.Interval = m_wakeup;
            IntervalTimer.Elapsed += new ElapsedEventHandler (this.OnTimer);
            IntervalTimer.Start ();

            DayTimer = new Timer ();
            DayTimer.Interval = 24 * 60 * 60 * 1000; // 24 hours
            DayTimer.Elapsed += new ElapsedEventHandler (this.OnDayTimer);
            DayTimer.Start ();
            string log = string.Format ("Electriki running\n");
            m_Logger.WriteStringLine (log);
        }

        protected override void OnStop ()
        {
            IntervalTimer.Stop ();
        }

        public void OnTimer (object sender, ElapsedEventArgs args)
        {
            m_Retina.GetPidsAndIPs (baseline, m_Logger);
            baseline = false;
            counter++;
            if (counter > 60)
            {
                m_Logger.WriteStringLine ("---------- no new incoming connections ------------");
                counter = 0;
            }
        }

        public void OnDayTimer (object sender, ElapsedEventArgs args)
        {
            baseline = true;
            m_Logger.FlushAndReset ();
        }
    }
}
