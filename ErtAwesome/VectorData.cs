using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ErtAwesome
{
    public class VectorData
    {
        private string name;
        private string data;
        private Brush fill;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.name = value;
            }
        }

        public string Data
        {
            get
            {
                return data;
            }

            set
            {
                this.data = value;
            }
        }

        public Brush Fill
        {
            get
            {
                return fill;
            }

            set
            {
                this.fill = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public VectorData(string name, string data)
        {
            this.Name = name;
            this.Data = data;
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="brush"></param>
        public VectorData(string name, string data, Brush brush)
        {
            this.Name = name;
            this.Data = data;
            this.Fill = brush;
        }
    }
}
