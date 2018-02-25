using Ertis.DaVinci.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ertis.DaVinci.Commands
{
    public class OpenProjectCommand : DelegateCommand
    {
        private static readonly Action ExecuteAction = delegate
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Browse Ertis Files";            
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = "ei";
            openFileDialog1.Filter = "Ertis files (*.ei)|*.ei|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var solutionPath = openFileDialog1.FileName;
                var solutionService = ServiceLocator.Current.GetInstance<ISolutionService>();
                solutionService.LoadSolution(solutionPath);
            }
        };

        public OpenProjectCommand() : base(ExecuteAction)
        {

        }
    }
}
