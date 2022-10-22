using BrightIdeasSoftware;
using System.Collections.Generic;
using System.Drawing;

namespace EXrat_Controller
{
    public class victim
    {
        public static ObjectListView OLV = new ObjectListView();

        public victim(string username, string iP, string screenWidth, string screenHeight, string isAdmin)
        {
            Device = "";
            Username = username;
            IP = iP;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            IsAdmin = isAdmin;
        }
        public string Device
        {
            get => device;
            set => device = value;
        }
        private string device;

        public string Username
        {
            get => username;
            set => username = value;
        }
        private string username;

        public string IP
        {
            get => iP;
            set => iP = value;
        }
        private string iP;

        public string ScreenWidth
        {
            get => screenWidth;
            set => screenWidth = value;
        }
        private string screenWidth;
        public string ScreenHeight
        {
            get => screenHeight;
            set => screenHeight = value;
        }
        private string screenHeight;
        public string IsAdmin
        {
            get => isAdmin;
            set => isAdmin = value;
        }
        private string isAdmin;

        internal static List<victim> ToList(victim victim)
        {
            List<victim> victimsLIST = new List<victim>
            {
                victim
            };
            return victimsLIST;
        }
        internal static void Add(victim victim)
        {
            OLV.AddObjects(ToList(victim));
        }
        public static void InitOLV(ObjectListView objectListView, OLVColumn Device, OLVColumn username, Image DeviceImage)
        {
            OLV = objectListView;
            objectListView.ShowGroups = false;
            objectListView.BackColor = Color.FromArgb(205, 231, 245);

            HeaderFormatStyle olvHdr = new HeaderFormatStyle();
            olvHdr.SetFont(new Font("arial", 10, FontStyle.Bold));

            olvHdr.SetBackColor(Color.FromArgb(255, 255, 155));

            objectListView.HeaderFormatStyle = olvHdr;

            Device.ImageGetter += delegate (object rowObject)
            {
                return DeviceImage;
            };
        }


    }
}
