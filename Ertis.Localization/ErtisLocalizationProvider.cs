using Ertis.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;
using XAMLMarkupExtensions.Base;

namespace Ertis.Localization
{
    public class ErtisLocalizationProvider : CSVLocalizationProviderBase, ILocalizationProvider
    {
        #region Dependency Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> DefaultDictionary to set the fallback resource dictionary.
        /// </summary>
        public static readonly DependencyProperty DefaultDictionaryProperty =
                DependencyProperty.RegisterAttached(
                "DefaultDictionary",
                typeof(string),
                typeof(ErtisLocalizationProvider),
                new PropertyMetadata(null, AttachedPropertyChanged));

        #endregion

        #region Dependency Property Callback
        
        /// <summary>
        /// Indicates, that one of the attached properties changed.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="args">The event argument.</param>
        private static void AttachedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Instance.OnProviderChanged(obj);
        }
        
        #endregion

        #region Dependency Property Management
        
        /// <summary>
        /// Getter of <see cref="DependencyProperty"/> default dictionary.
        /// </summary>
        /// <param name="obj">The dependency object to get the default dictionary from.</param>
        /// <returns>The default dictionary.</returns>
        public static string GetDefaultDictionary(DependencyObject obj)
        {
            return (obj != null) ? (string)obj.GetValue(DefaultDictionaryProperty) : null;
        }

        /// <summary>
        /// Setter of <see cref="DependencyProperty"/> default dictionary.
        /// </summary>
        /// <param name="obj">The dependency object to set the default dictionary to.</param>
        /// <param name="value">The dictionary.</param>
        public static void SetDefaultDictionary(DependencyObject obj, string value)
        {
            obj.SetValue(DefaultDictionaryProperty, value);
        }
        
        #endregion


        private string fileName = "";
        private readonly string fileExtension = ".loc";

        // creates a Missing Localizations file if set to true
        private readonly bool doesLogMissingLocalizations = false;

        /// <summary>
        /// The name of the file without an extension.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;

                    UpdateAvailableCultures();
                }
            }
        }

        public void UpdateAvailableCultures()
        {
            this.AvailableCultures.Clear();

            var appPath = Path.Combine(GetWorkingDirectory(), "Localization");
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            foreach (var c in cultures)
            {
                var csv = Path.Combine(appPath, this.FileName + "." + c.Name + this.fileExtension);
                if (File.Exists(csv))
                    this.AvailableCultures.Add(c);
            }

            OnProviderChanged(null);
        }

        #region Singleton Variables, Properties & Constructor

        /// <summary>
        /// The instance of the singleton.
        /// </summary>
        private static ErtisLocalizationProvider instance;

        /// <summary>
        /// Lock object for the creation of the singleton instance.
        /// </summary>
        private static readonly object InstanceLock = new object();

        /// <summary>
        /// Gets the <see cref="ErtisLocalizationProvider"/> singleton.
        /// </summary>
        public static ErtisLocalizationProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (InstanceLock)
                    {
                        if (instance == null)
                            instance = new ErtisLocalizationProvider();
                    }
                }

                // return the existing/new instance
                return instance;
            }
        }

        /// <summary>
        /// The singleton constructor.
        /// </summary>
        private ErtisLocalizationProvider()
        {
            //AvailableCultures = new ObservableCollection<CultureInfo>();
            //AvailableCultures.Add(CultureInfo.InvariantCulture);
        }

        private bool hasHeader = false;
        /// <summary>
        /// A flag indicating, if it has a header row.
        /// </summary>
        public bool HasHeader
        {
            get { return hasHeader; }
            set
            {
                hasHeader = value;
                //OnProviderChanged(null); 
            }
        }

        #endregion

        /// <summary>
        /// Get the working directory, depending on the design mode or runtime.
        /// </summary>
        /// <returns>The working directory.</returns>
        private string GetWorkingDirectory()
        {
            //if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            //{
            //    var dte = (EnvDTE.DTE)Marshal.GetActiveObject("VisualStudio.DTE.10.0");
            //    var sb = dte.Solution.SolutionBuild;
            //    string msg = "";

            //    foreach (String item in (Array)sb.StartupProjects)
            //    {
            //        msg += item;
            //    }

            //    EnvDTE.Project startupProj = dte.Solution.Item(msg);

            //    return (Path.GetDirectoryName(startupProj.FullName));
            //}
            //else
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Parses a key ([[Assembly:]Dict:]Key and return the parts of it.
        /// </summary>
        /// <param name="inKey">The key to parse.</param>
        /// <param name="outAssembly">The found or default assembly.</param>
        /// <param name="outDict">The found or default dictionary.</param>
        /// <param name="outKey">The found or default key.</param>
        public static void ParseKey(string inKey, out string outAssembly, out string outDict, out string outKey)
        {
            // Reset everything to null.
            outAssembly = null;
            outDict = null;
            outKey = null;

            if (!string.IsNullOrEmpty(inKey))
            {
                string[] split = inKey.Trim().Split(":".ToCharArray());

                // assembly:dict:key
                if (split.Length == 3)
                {
                    outAssembly = !string.IsNullOrEmpty(split[0]) ? split[0] : null;
                    outDict = !string.IsNullOrEmpty(split[1]) ? split[1] : null;
                    outKey = split[2];
                }

                // dict:key
                if (split.Length == 2)
                {
                    outDict = !string.IsNullOrEmpty(split[0]) ? split[0] : null;
                    outKey = split[1];
                }

                // key
                if (split.Length == 1)
                {
                    outKey = split[0];
                }
            }
        }

        public FullyQualifiedResourceKeyBase GetFullyQualifiedResourceKey(string key, DependencyObject target)
        {
            String assembly, dictionary;
            ParseKey(key, out assembly, out dictionary, out key);

            if (target == null)
                return new FQAssemblyDictionaryKey(key, assembly, dictionary);

            if (String.IsNullOrEmpty(assembly))
                assembly = GetAssembly(target);

            if (String.IsNullOrEmpty(dictionary))
                dictionary = GetDictionary(target);

            return new FQAssemblyDictionaryKey(key, assembly, dictionary);
        }

        /// <summary>
        /// Parent is passed to remove all child translation bindings
        /// </summary>
        /// <param name="obj">Parent window</param>
        /*
        public void RemoveParentNotifier(DependencyObject obj)
        {
            var toRemove = new List<DependencyObject>();
            var keyList = parentNotifiers.Keys;

            foreach (var key in keyList)
            {
                var typeOf = obj.GetType();
                MethodInfo method = typeof(VisualHelper).GetMethod("TryFindParent");
                MethodInfo generic = method.MakeGenericMethod(typeOf);
                var retParent = generic.Invoke(null, new object[] { key });

                if (retParent != null && retParent == obj)
                    toRemove.Add(key);
            }

            foreach (var val in toRemove)
            {
                if (parentNotifiers.ContainsKey(val))
                {
                    parentNotifiers[val].Dispose();
                    parentNotifiers.Remove(val);
                }
            }
        }
        */

        #region Variables

        /// <summary>
        /// A dictionary for notification classes for changes of the individual target Parent changes.
        /// </summary>
        private ParentNotifiers parentNotifiers = new ParentNotifiers();
        
        #endregion

        /// <summary>
        /// An action that will be called when a parent of one of the observed target objects changed.
        /// </summary>
        /// <param name="obj">The target <see cref="DependencyObject"/>.</param>
        private void ParentChangedAction(DependencyObject obj)
        {
            OnProviderChanged(obj);
        }

        /// <summary>
        /// Get the dictionary from the context, if possible.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <returns>The dictionary name, if available.</returns>
        protected override string GetDictionary(DependencyObject target)
        {
            if (target == null)
                return null;

            return target.GetValueOrRegisterParentNotifier<string>(CSVEmbeddedLocalizationProvider.DefaultDictionaryProperty, ParentChangedAction, parentNotifiers);
        }

        /// <summary>
        /// Get the assembly from the context, if possible.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <returns>The assembly name, if available.</returns>
        protected override string GetAssembly(DependencyObject target)
        {
            if (target == null)
                return null;

            return target.GetValueOrRegisterParentNotifier<string>(CSVEmbeddedLocalizationProvider.DefaultAssemblyProperty, ParentChangedAction, parentNotifiers);
        }
        
        /// <summary>
        /// Get the localized object.
        /// </summary>
        /// <param name="key">The key to the value.</param>
        /// <param name="target">The target object.</param>
        /// <param name="culture">The culture to use.</param>
        /// <returns>The value corresponding to the source/dictionary/key path for the given culture (otherwise NULL).</returns>
        public object GetLocalizedObject(string key, DependencyObject target, CultureInfo culture)
        {
            string csvPath = "";

            try
            {
                // Try to get the culture specific file.
                csvPath = GetCsvPath(target, key, culture);
            }
            catch (InvalidOperationException invOpEx)
            {
                return null;
            }

            // Open the file.
            string ret = GetExtendedLocalizedValue(csvPath, key);

            // Nothing found -> Raise the error message.
            if (ret == null)
                OnProviderError(target, key, "The key does not exist in " + csvPath + ".");

            return ret;
        }

        Dictionary<string, string> LocFileDict = new Dictionary<string, string>();
        private string GetCsvPath(DependencyObject target, string key, CultureInfo culture)
        {
            // Try to get the culture specific file.
            var appPath = Path.Combine(GetWorkingDirectory(), "Localization");
            var csvPath = "";
            while (culture != CultureInfo.InvariantCulture)
            {
                string twoLetterCode = culture.TwoLetterISOLanguageName;
                csvPath = Path.Combine(appPath, this.FileName + (String.IsNullOrEmpty(culture.Name) ? "" : "." + twoLetterCode) + fileExtension);

                culture = culture.Parent;

                if (LocFileDict.ContainsKey(twoLetterCode))
                {
                    return csvPath;
                }
                if (File.Exists(csvPath))
                {
                    LocFileDict.Add(twoLetterCode, csvPath);
                    ReadAllKeysAndAddToDictionary(csvPath);
                    return csvPath;
                }
            }

            if (!File.Exists(csvPath))
            {
                // Take the invariant culture.
                csvPath = Path.Combine(appPath, this.FileName + fileExtension);

                if (!File.Exists(csvPath))
                {
                    string err = "A file for the provided culture " + culture.EnglishName + " does not exist at " + csvPath + ".";
                    OnProviderError(target, key, err);
                    throw new InvalidOperationException(err);
                }
            }

            return csvPath;
        }

        Dictionary<string, Dictionary<string, string>> LocalizedDict = new Dictionary<string, Dictionary<string, string>>();

        private void ReadAllKeysAndAddToDictionary(string csvPath)
        {
            if (!LocalizedDict.ContainsKey(csvPath))
                LocalizedDict.Add(csvPath, new Dictionary<string, string>());

            // Open the file.
            using (var reader = new StreamReader(csvPath, Encoding.UTF8))
            {
                // Skip the header if needed.
                if (this.HasHeader && !reader.EndOfStream)
                    reader.ReadLine();

                // Read each line and split it.
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var parts = line.Split(";".ToCharArray());

                    if (parts.Length < 2)
                        continue;

                    if (!LocalizedDict[csvPath].ContainsKey(parts[0]))
                        LocalizedDict[csvPath].Add(parts[0], parts[1]);
                }
            }

        }

        public HashSet<string> IgnoredLocalizations { get; set; }

        private string GetExtendedLocalizedValue(string csvPath, string key)
        {
            string ret = null;

            var splitted = new List<string>(key.Split('^'));

            int beforeIgnoreCount = splitted.Count;
            // remove ignored localizations from the list
            for (int i = 0; i < splitted.Count; i++)
            {
                if (IgnoredLocalizations.Contains(splitted[i]))
                {
                    splitted.RemoveAt(i);
                    i--;
                }
            }

            // we ignored everthing here so I return nothing to you
            if (splitted.Count == 0 && beforeIgnoreCount != 0)
            {
                return "";
            }

            for (int i = 0; i < splitted.Count; i++)
            {
                if (i != 0)
                    ret += " ";

                if (splitted[i].StartsWith("`"))
                    ret += splitted[i].Substring(1);
                else
                {
                    string retPart = GetLocalizedValue(csvPath, splitted[i]);
                    if (retPart != null)
                    {
                        ret += retPart;
                    }
                    else
                    {
                        ret += "Key:" + splitted[i];

                        if (doesLogMissingLocalizations)
                            LogMissingLocalizations(splitted[i]);

                        OnProviderError(null, splitted[i], "The key does not exist in " + csvPath + ".");
                    }
                }
            }

            return ret;
        }

        HashSet<string> missingLocs = new HashSet<string>();
        private void LogMissingLocalizations(string key)
        {
            if (!missingLocs.Contains(key))
            {
                missingLocs.Add(key);
                System.Diagnostics.Debug.WriteLine("MissingLocalizationKey : " + key);

                /*
                System.IO.StreamWriter file = new System.IO.StreamWriter("missingLocs.mtx.loc", true);
                file.WriteLine(key);
                file.Close();
                */
            }
        }

        private string GetLocalizedValue(string csvPath, string key)
        {
            if (LocalizedDict.ContainsKey(csvPath))
            {
                if (LocalizedDict[csvPath].ContainsKey(key))
                    return LocalizedDict[csvPath][key];
            }
            else
                LocalizedDict.Add(csvPath, new Dictionary<string, string>());

            string ret = null;
            // Open the file.
            using (var reader = new StreamReader(csvPath, Encoding.UTF8))
            {
                // Skip the header if needed.
                if (this.HasHeader && !reader.EndOfStream)
                    reader.ReadLine();

                // Read each line and split it.
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var parts = line.Split(";".ToCharArray());

                    if (parts.Length < 2)
                        continue;

                    // Check the key (1st column).
                    if (parts[0] != key)
                        continue;

                    // Get the value (2nd column).
                    ret = parts[1];
                    break;
                }
            }

            if (!LocalizedDict[csvPath].ContainsKey(key))
                LocalizedDict[csvPath].Add(key, ret);

            return ret;
        }

        /// <summary>
        /// Adds new translations to each file
        /// </summary>
        /// <param name="transList"></param>
        public void AddLocalizations(List<AppTranslation> transList)
        {
            var transDict = new Dictionary<string, string>();
            foreach (var translation in transList)
            {
                var cultureName = translation.Language;
                var culture = System.Globalization.CultureInfo.GetCultureInfo(cultureName);

                string csvPath = GetCsvPath(null, "", culture);


                if (GetExtendedLocalizedValue(csvPath, translation.StrKey).Contains("Key:"))
                {
                    if (!transDict.ContainsKey(cultureName))
                        transDict.Add(cultureName, "");

                    transDict[cultureName] += Environment.NewLine + translation.StrKey + ";" + translation.StrValue;
                }
            }

            foreach (var cultureName in transDict.Keys)
            {
                var culture = System.Globalization.CultureInfo.GetCultureInfo(cultureName);

                string csvPath = GetCsvPath(null, "", culture);

                //write symbols to csv files
                System.IO.StreamWriter file = new System.IO.StreamWriter(csvPath, true);
                file.WriteLine(transDict[cultureName]);
                file.Close();
            }
        }

        /// <summary>
        /// An observable list of available cultures.
        /// </summary>
        private ObservableCollection<CultureInfo> availableCultures = null;
        public ObservableCollection<CultureInfo> AvailableCultures
        {
            get
            {
                if (availableCultures == null)
                    availableCultures = new ObservableCollection<CultureInfo>();

                return availableCultures;
            }
        }
    }
}
