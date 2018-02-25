using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Models
{
    public class View
    {
        #region Fields

        private Type viewType;
        private string title;
        private double startWidth = 750;
        private double startHeight = 500;

        #endregion

        #region Properties

        public Type ViewType
        {
            get
            {
                return viewType;
            }

            private set
            {
                this.viewType = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                this.title = value;
            }
        }

        public double StartWidth
        {
            get
            {
                return startWidth;
            }

            set
            {
                this.startWidth = value;
            }
        }

        public double StartHeight
        {
            get
            {
                return startHeight;
            }

            set
            {
                this.startHeight = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public View(string caption, Type viewType)
        {
            this.Title = caption;
            this.ViewType = viewType;
        }

        #endregion
    }
}
