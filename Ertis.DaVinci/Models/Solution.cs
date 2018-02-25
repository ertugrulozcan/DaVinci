using Ertis.Infrastructure.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Models
{
    public class Solution : INotifyPropertyChanged
    {
        #region Fields

        private int id;
        private string name;
        private ObservableRangeCollection<Project> projectList;

        #endregion

        #region Properties

        public int Id
        {
            get
            {
                return id;
            }

            private set
            {
                this.id = value;
                this.RaisePropertyChanged("Id");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            private set
            {
                this.name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public ObservableRangeCollection<Project> ProjectList
        {
            get
            {
                return projectList;
            }

            private set
            {
                this.projectList = value;
                this.RaisePropertyChanged("ProjectList");
            }
        }

        [JsonIgnore]
        public string SolutionPath { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        private Solution()
        {
            this.ProjectList = new ObservableRangeCollection<Project>();
        }

        public Solution(int id, string name) : this()
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
