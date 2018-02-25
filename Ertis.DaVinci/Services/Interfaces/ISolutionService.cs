using Ertis.DaVinci.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Services.Interfaces
{
    public interface ISolutionService
    {
        string SolutionPath { get; }

        void LoadSolution(string path);

        Solution CurrentSolution { get; }

        void CreateSolution(string projectName, string folderPath);

        event EventHandler<Solution> SolutionLoaded;

        void SaveSolution(Solution solution);

        void RefreshWebSite();

        string TempFolderPath { get; }
    }
}
