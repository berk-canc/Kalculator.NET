//////////////////////////////////////////////////////////////////
//                                                              //
//  Copyright (c) 2016 Berk C. Celebisoy. All Rights Reserved.  //
//                                                              //
//////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalculator
{
    class Buffer
    {
        private static Buffer m_instance;
        private string m_buffer;


        public static Buffer GetInstance()
        {
            if (m_instance == null)
            {
                m_instance = new Buffer();
            }

            return m_instance;
        }


        private Buffer()
        {
            m_buffer = "";
        }


        public void Log(string value)
        {
            if (value == null)
            {
                throw new ArgumentException("Bad value.");
            }

            m_buffer += value;
        }


        public void Log(double value)
        {
            m_buffer += Convert.ToString(value);
        }


        public void LogLine(string value)
        {
            if (value == null)
            {
                throw new ArgumentException("Bad value.");
            }

            m_buffer += (value + "\n");
        }


        public void LogLine(double value)
        {
            m_buffer += (Convert.ToString(value) + "\n");
        }


        public void Clear()
        {
            m_buffer = "";
        }


        public string GetBuffer()
        {
            return m_buffer;
        }
    }
}
