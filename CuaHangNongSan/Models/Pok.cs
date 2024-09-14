using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangNongSan.Models
{
    public class Pok
    {
        private int _id;
        private string _name;
        private ArrayList _types;
        private string _image;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public ArrayList Types
        {
            get { return _types; }
            set { _types = value; }
        }

        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }




    }
}