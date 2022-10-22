using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EXrat_Controller
{
    public class victim
    {

        public victim()
        {

        }

        public victim(string device, string username, string iP, int ScreenWidth, int ScreenHeight, bool isAdmin)
        {
            Device = device;
            Username = username;
            IP = iP;
        }
        public string Device
        {
            get { return device; }
            set { device = value; }
        }
        private string device;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string username;

        public string IP
        {
            get { return iP; }
            set { iP = value; }
        }
        private string iP;
        static internal List<victim> GET()
        {
            List<victim> songs = new List<victim>();
            
            songs.Add(new victim("", "T", "123.12", 123, 123, true));
            return songs;
        }



    }
}
