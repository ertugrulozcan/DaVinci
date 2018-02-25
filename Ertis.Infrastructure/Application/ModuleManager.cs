using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Application
{
    public class ModuleManager
    {
        #region Static Current

        private static ModuleManager self;
        public static ModuleManager Current
        {
            get
            {
                if (self == null)
                    self = new ModuleManager();

                return self;
            }
        }

        #endregion

        #region Fields

        private ObservableCollection<EModule> moduleList;

        #endregion

        #region Properties

        public ObservableCollection<EModule> ModuleList
        {
            get
            {
                return this.moduleList;
            }
            private set
            {
                if (this.moduleList != null)
                    this.moduleList.CollectionChanged -= Modules_CollectionChanged;

                this.moduleList = value;

                this.moduleList.CollectionChanged += Modules_CollectionChanged;
            }
        }

        #endregion

        #region Events

        public event EventHandler ModuleRegistered;
        public event EventHandler ModuleUnregistered;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        private ModuleManager()
        {
            this.ModuleList = new ObservableCollection<EModule>();
        }

        #endregion

        #region Methods

        private void ExecuteHandler(EventHandler handler)
        {
            if (handler != null)
                handler(this, new EventArgs());
        }

        #endregion

        #region Event Handlers

        private void Modules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    this.ExecuteHandler(this.ModuleRegistered);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    this.ExecuteHandler(this.ModuleUnregistered);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
