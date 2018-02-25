using System.Windows.Threading;
using NHunspell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using Ertis.Shared.Search;
using Ertis.Shared.Helpers;
using Ertis.Infrastructure.Helpers;

namespace Ertis.Search.Build
{
    public class SearchEngine : DependencyObject
    {
        #region Fields

        private SearchTree tree;
        private HashSet<object> dataSource;
        private System.Collections.IEnumerable originalDataSource;
        private List<SpellFix> spellFixList = new List<SpellFix>();
        private bool useMemberKey;
        private string memberkey;
        private bool isBusy = true;

        #endregion

        #region Properties

        public SearchTree Tree
        {
            get
            {
                return tree;
            }

            private set
            {
                tree = value;
            }
        }

        public HashSet<object> DataSource
        {
            get
            {
                return dataSource;
            }

            private set
            {
                dataSource = value;
            }
        }

        public Queue<ISearchable> AddingQueue { get; private set; }
        public Queue<ISearchable> RemovingQueue { get; private set; }

        public Hunspell SpellChecker { get; private set; }

        public List<SpellFix> SpellFixList
        {
            get
            {
                return spellFixList;
            }

            private set
            {
                spellFixList = value;
            }
        }

        public bool IsSpellCheckerActive { get; set; }

        public IEnumerable OriginalDataSource
        {
            get
            {
                return originalDataSource;
            }

            private set
            {
                if (this.originalDataSource != null && this.originalDataSource is System.Collections.Specialized.INotifyCollectionChanged)
                {
                    var collection = this.originalDataSource as System.Collections.Specialized.INotifyCollectionChanged;
                    collection.CollectionChanged -= Collection_CollectionChanged;
                }

                if (value is System.Collections.Specialized.INotifyCollectionChanged)
                {
                    var collection = value as System.Collections.Specialized.INotifyCollectionChanged;
                    collection.CollectionChanged += Collection_CollectionChanged;
                }

                originalDataSource = value;
            }
        }

        public string Memberkey
        {
            get
            {
                return memberkey;
            }

            private set
            {
                memberkey = value;
            }
        }

        public string Info
        {
            get
            {
                if (this.DataSource == null)
                    return "Data source is empty";

                return string.Format("{0} item", this.DataSource.Count);
            }
        }


        /*
        private static readonly DependencyPropertyKey IsReadyPropertyKey = DependencyProperty.RegisterReadOnly("IsReady", typeof(bool), typeof(SearchEngine), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public static readonly DependencyProperty IsReadyProperty = IsReadyPropertyKey.DependencyProperty;

        public bool IsReady
        {
            get
            {
                return (bool)GetValue(IsReadyProperty);
            }
            protected set
            {
                SetValue(IsReadyPropertyKey, value);
            }
        }
        */

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            private set
            {
                this.isBusy = value;

                if (this.IsBusy)
                {
                    if (this.OnBusy != null)
                        this.OnBusy(this, new EventArgs());
                }
                else
                {
                    if (this.OnReady != null)
                        this.OnReady(this, new EventArgs());

                    this.PeekQueues();
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler OnBusy;
        public event EventHandler OnReady;

        #endregion

        #region Constructors

        /// <summary>
        /// Private Constructor
        /// </summary>
        private SearchEngine()
        {
            this.SpellFixList = new List<SpellFix>();
            this.AddingQueue = new Queue<ISearchable>();
            this.RemovingQueue = new Queue<ISearchable>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchEngine(IEnumerable<ISearchable> dataSource) : this()
        {
            this.Construct(dataSource, null);
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        public SearchEngine(IEnumerable<ISearchable> dataSource, string memberKey) : this()
        {
            this.Construct(dataSource, memberKey);
        }

        /// <summary>
        /// Constructor 3
        /// </summary>
        public SearchEngine(IEnumerable<ISearchable> dataSource, bool isSpellCheckerActive) : this()
        {
            this.IsSpellCheckerActive = isSpellCheckerActive;
            this.ConstructAsync(dataSource, null);
        }

        /// <summary>
        /// Constructor 4
        /// </summary>
        public SearchEngine(IEnumerable<ISearchable> dataSource, string memberKey, bool isSpellCheckerActive) : this()
        {
            this.IsSpellCheckerActive = isSpellCheckerActive;
            this.Construct(dataSource, memberKey);
        }

        /// <summary>
        /// Kullanırken dikkatli ol !!!
        /// SearchEngine construct edilirken object referansına ihtiyaç duyulabilecek durumlarda kullanılmak üzere eklendi.
        /// Engine henüz üretilmeden Add/Remove DataSource'una ekleme yapılmak istenirse, kuyruğa alınması için Engine referansının verilmesi gerekli.
        /// Static Constructor çağırıldıktan hemen sonra Construct edilmeli !!! Yoksa DataSource ve SearchTree set edilmez.
        /// </summary>
        /// <returns></returns>
        public static SearchEngine Create(IEnumerable<ISearchable> dataSource, bool isSpellCheckerActive)
        {
            SearchEngine engine = new SearchEngine();
            engine.OriginalDataSource = dataSource;
            engine.IsSpellCheckerActive = isSpellCheckerActive;

            return engine;
        }

        public void Construct()
        {
            this.Construct(this.OriginalDataSource as IEnumerable<ISearchable>, null);
        }

        static int c = 0;
        private void Construct(IEnumerable<ISearchable> dataSource, string memberKey)
        {
            c++;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            UIHelper.UiInvoke(DispatcherPriority.Background, false, delegate
            {
                this.IsBusy = true;
                this.OriginalDataSource = dataSource;
            });

            var tempDataSource = new HashSet<object>();
            Hunspell spellChecker = null;
            var dictionary = new List<KeyValuePair<string, object>>();

            if (dataSource != null)
            {
                var list = dataSource.ToList();

                if (string.IsNullOrEmpty(memberKey))
                {
                    this.useMemberKey = false;
                    this.Memberkey = null;

                    foreach (var data in list)
                    {
                        tempDataSource.Add(data);
                        dictionary.Add(new KeyValuePair<string, object>(data.SearchKey, data));
                    }
                }
                else
                {
                    this.useMemberKey = true;
                    this.Memberkey = memberKey;

                    foreach (var data in list)
                    {
                        tempDataSource.Add(data);

                        var prop = GetPropValue(data, memberKey);
                        if (prop != null)
                            dictionary.Add(new KeyValuePair<string, object>(prop.ToString(), data));
                        else
                            dictionary.Add(new KeyValuePair<string, object>(data.SearchKey, data));
                    }
                }

                stopwatch.Stop();
                Debug.WriteLine("SEARCH {1} \t CONSTRUCTDICT\t Time elapsed: {0}", stopwatch.ElapsedMilliseconds, c);
                stopwatch.Restart();

                this.SplitWords(dictionary);

                stopwatch.Stop();
                Debug.WriteLine("SEARCH {1} \t SPLITWORDS\t Time elapsed: {0}", stopwatch.ElapsedMilliseconds, c);
                stopwatch.Restart();

                try
                {
                    if (this.IsSpellCheckerActive)
                    {
                        var wordList = dictionary.Select(x => x.Key).ToList();
                        if (wordList.Count == 0)
                            wordList.Add("load, price wheather otc smp, base peak, offpeak import");

                        spellChecker = this.CreateSpellChecker(wordList);

                        if (spellChecker == null)
                            this.IsSpellCheckerActive = false;
                    }
                }
                catch
                {
                    this.IsSpellCheckerActive = false;
                }
                stopwatch.Stop();
                Debug.WriteLine("SEARCH {1} \t SPELLCHECKER\t Time elapsed: {0}", stopwatch.ElapsedMilliseconds, c);
                stopwatch.Restart();
            }

            var tempTree = new SearchTree(dictionary);

            stopwatch.Stop();
            Debug.WriteLine("SEARCH {1} \t TREE\t Time elapsed: {0}", stopwatch.ElapsedMilliseconds, c);

            UIHelper.UiInvoke(DispatcherPriority.Background, false, delegate
            {
                this.SpellChecker = spellChecker;

                this.DataSource = tempDataSource;
                this.Tree = tempTree;
                this.IsBusy = false;
            });
        }

        public async void ConstructAsync(IEnumerable<ISearchable> dataSource, string memberKey)
        {
            await Task.Run(() =>
            {
                this.Construct(dataSource, memberKey);
            });
        }

        public void Regenerate(IEnumerable<ISearchable> dataSource, string memberKey, bool isSpellCheckerActive)
        {
            this.Dispose();

            this.IsSpellCheckerActive = isSpellCheckerActive;
            this.Construct(dataSource, memberKey);
        }

        public void Regenerate(IEnumerable<ISearchable> dataSource, bool isSpellCheckerActive)
        {
            this.Dispose();

            this.IsSpellCheckerActive = isSpellCheckerActive;
            this.Construct(dataSource, null);
        }

        #endregion

        #region Methods

        public HashSet<object> Search(string searchKey, ResultCompiling resultCompiling, CombineMode combineMode = CombineMode.Intersection)
        {
            /*
            if (!this.IsReady)
                return new HashSet<object>();
            */

            this.SpellFixList.Clear();

            if (string.IsNullOrEmpty(searchKey.Trim(' ')))
                return this.DataSource;

            if (this.Tree != null)
            {
                if (resultCompiling == ResultCompiling.Basic)
                {
                    Node node = this.Tree.FindNode(searchKey);

                    if (node != null)
                        return node.Objects;
                    else
                    {
                        var unaccentedText = String.Join("", searchKey.Normalize(NormalizationForm.FormD).Where(c => CharTool.ToLowerFast(c) != c));
                        var node2 = this.Tree.FindNode(unaccentedText);

                        if (node2 != null)
                            return node2.Objects;
                        else
                            return this.GetSpellCheckerSuggestions(searchKey);
                    }
                }
                else
                {
                    List<string> searchKeys = searchKey.Split(' ').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToList();
                    if (searchKeys.Count == 1)
                    {
                        var trimmedSearchKey = searchKey.Trim();
                        Node node = this.Tree.FindNode(trimmedSearchKey);

                        if (node != null)
                            return node.Objects;
                        else
                        {
                            var unaccentedText = ConvertTurkishCharsToEnglish(trimmedSearchKey);
                            var node2 = this.Tree.FindNode(unaccentedText);

                            if (node2 != null)
                                return node2.Objects;
                            else
                                return this.GetSpellCheckerSuggestions(trimmedSearchKey);
                        }
                    }

                    List<Node> nodeList = new List<Node>();
                    foreach (string key in searchKeys)
                    {
                        Node found = this.Tree.FindNode(key);
                        if (found != null)
                            nodeList.Add(found);
                    }

                    if (nodeList.Count > 0)
                    {
                        return this.CombineResults(nodeList, combineMode);
                    }
                    else
                        return this.GetSpellCheckerSuggestions(searchKeys.First());
                }
            }
            else
                return new HashSet<object>();
        }

        private HashSet<object> CombineResults(List<Node> nodeList, CombineMode combineMode)
        {
            HashSet<object> combinedResults = new HashSet<object>();

            switch (combineMode)
            {
                case CombineMode.Combination:
                    {
                        foreach (Node node in nodeList)
                        {
                            foreach (object resultObject in node.Objects)
                            {
                                if (!combinedResults.Contains(resultObject))
                                {
                                    combinedResults.Add(resultObject);
                                }
                            }
                        }
                    }
                    break;
                case CombineMode.Intersection:
                    {
                        foreach (Node node in nodeList)
                        {
                            foreach (object resultObject in node.Objects)
                            {
                                if (!combinedResults.Contains(resultObject) && nodeList.All(x => x.Objects.Contains(resultObject)))
                                {
                                    combinedResults.Add(resultObject);
                                }
                            }
                        }
                    }
                    break;
                case CombineMode.Difference:
                    {
                        var combinationList = this.CombineResults(nodeList, CombineMode.Combination);
                        var intersectionList = this.CombineResults(nodeList, CombineMode.Intersection);
                        foreach (var item in combinationList)
                        {
                            if (!intersectionList.Contains(item))
                            {
                                combinedResults.Add(item);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            return combinedResults;
        }

        /// <summary>
        /// Basic; Girilen search key tek parça bir string olarak ele alınır. Tam olarak girilen değere dayalı sonuçlar gösterilir.
        /// Combined; Girilen search key kelime kelime parçalanır. Her girdi için ayrı sonuç hesaplanır ve bu sonuçlar seçili CombineMode'a göre birleştirilir.
        /// </summary>
        public enum ResultCompiling
        {
            Basic,
            Combined
        }

        public enum CombineMode
        {
            Combination,
            Intersection,
            Difference
        }

        private HashSet<object> GetSpellCheckerSuggestions(string searchKey)
        {
            if (this.IsSpellCheckerActive && searchKey.Length > 1)
            {
                bool hasSuggestion = this.DidYouMean(searchKey);
                if (hasSuggestion)
                {
                    var dym = new HashSet<object>();
                    foreach (var spellfix in this.SpellFixList)
                        foreach (var suggestion in spellfix.SuggestionList)
                            dym.Add(spellfix);
                    return dym;
                }
                else
                    return new HashSet<object>();
            }
            else
                return new HashSet<object>();
        }

        private void SplitWords(List<KeyValuePair<string, object>> dictionary)
        {
            var dictionaryList = dictionary.ToList();

            foreach (KeyValuePair<string, object> vp in dictionaryList)
            {
                List<string> words = vp.Key.Split(' ').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToList();
                if (words.Count > 1)
                {
                    /*
                    for (int i = 1; i < words.Length; i++)
                        newValuePairs.Add(new KeyValuePair<string, object>(words[i], vp.Value));
                    */

                    for (int i = 1; i < words.Count; i++)
                    {
                        //string merged = string.Empty;
                        //for (int j = i; j < words.Count; j++)
                        //    merged += words[j] + " ";
                        //newValuePairs.Add(new KeyValuePair<string, object>(merged, vp.Value));
                        var kvPair = new KeyValuePair<string, object>(words[i], vp.Value);
                        dictionary.Add(kvPair);
                    }
                }
                else if (words.Count == 1)
                {
                    var kvPair = new KeyValuePair<string, object>(words[0], vp.Value);
                    dictionary.Add(kvPair);
                }
            }
        }

        private IList<string> SplitWords(IList<string> dictionary)
        {
            List<string> splittedList = new List<string>();

            foreach (string sentence in dictionary)
            {
                List<string> words = sentence.Trim().Split(' ').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToList();
                if (words.Count > 1)
                {
                    for (int i = 1; i < words.Count; i++)
                    {
                        if (!splittedList.Any(x => x == words[i]))
                            splittedList.Add(words[i]);
                    }
                }
                else if (words.Count == 1)
                    if (!splittedList.Any(x => x == words[0]))
                        splittedList.Add(words[0]);
            }

            return splittedList;
        }

        public static object GetPropValue(object src, string propName)
        {
            var prop = src.GetType().GetProperty(propName);
            if (prop == null)
                return null;
            else
                return prop.GetValue(src, null);
        }

        private void PeekQueues()
        {
            if (this.RemovingQueue != null && this.RemovingQueue.Count > 0)
            {
                this.RemoveItems(this.RemovingQueue.ToArray());
                this.RemovingQueue.Clear();
            }

            if (this.AddingQueue != null && this.AddingQueue.Count > 0)
            {
                this.AddItems(this.AddingQueue.ToArray());
                this.AddingQueue.Clear();
            }
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var addedItems = e.NewItems;
                if (addedItems != null && addedItems.Count > 0)
                {
                    if (this.IsBusy)
                    {
                        foreach (var item in addedItems)
                        {
                            if (item is ISearchable)
                                this.AddingQueue.Enqueue(item as ISearchable);
                        }
                    }
                    else
                    {
                        this.AddItems(addedItems);
                    }
                }
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var removedItems = e.OldItems;
                if (removedItems != null && removedItems.Count > 0)
                {
                    if (this.IsBusy)
                    {
                        foreach (var item in removedItems)
                        {
                            if (item is ISearchable)
                                this.RemovingQueue.Enqueue(item as ISearchable);
                        }
                    }
                    else
                    {
                        this.RemoveItems(removedItems);
                    }
                }
            }
        }

        private void AddItems(IList items)
        {
            List<KeyValuePair<string, object>> dictionary = new List<KeyValuePair<string, object>>();

            foreach (var item in items)
            {
                if (!this.DataSource.Contains(item))
                    this.DataSource.Add(item);

                dictionary.Add(this.ToKeyValuePair(item as ISearchable));
            }

            this.SplitWords(dictionary);

            if (this.IsSpellCheckerActive)
            {
                var spellDict = dictionary.Select(x => x.Key).ToList();
                foreach (var key in spellDict)
                {
                    this.SpellChecker.Add(key);
                }

            }

            this.Tree.AddItems(dictionary);
        }

        private void RemoveItems(IList items)
        {
            var dictionary = new List<KeyValuePair<string, object>>();

            foreach (var item in items)
            {
                if (this.DataSource.Contains(item))
                    this.DataSource.Remove(item);

                dictionary.Add(this.ToKeyValuePair(item as ISearchable));
            }

            this.SplitWords(dictionary);

            if (this.IsSpellCheckerActive)
            {
                var spellDict = dictionary.Select(x => x.Key);
                foreach (var key in spellDict)
                    this.SpellChecker.Remove(key);
            }

            this.Tree.RemoveItems(dictionary);
        }

        private KeyValuePair<string, object> ToKeyValuePair(ISearchable item)
        {
            if (this.useMemberKey)
            {
                var prop = GetPropValue(item, this.Memberkey);
                if (prop != null)
                {
                    return new KeyValuePair<string, object>(prop.ToString(), item);
                }
                else
                {
                    return new KeyValuePair<string, object>((item as ISearchable).SearchKey, item);
                }
            }
            else
            {
                return new KeyValuePair<string, object>((item as ISearchable).SearchKey, item);
            }
        }

        #endregion

        #region SpellChecker

        private Hunspell CreateSpellChecker(IList<string> dictionary)
        {
            Hunspell hunspell = null;

            dictionary = this.SplitWords(dictionary);
            string mergedDictionary = string.Join(Environment.NewLine, dictionary);
            mergedDictionary = string.Format("{0}{1}{2}", dictionary.Count, Environment.NewLine, mergedDictionary);

            try
            {
                hunspell = new Hunspell(new byte[1], GetBytes(mergedDictionary));

                /*
                // Premium ;)
                string dictionaryPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + @"/Resources/Dictionaries/tr-TR/";
                hunspell = new Hunspell(dictionaryPath + "tr-TR.aff", dictionaryPath + "tr-TR.dic");
                */
            }
            catch (Exception ex)
            {
                //hunspell = new Hunspell(new byte[1], GetBytes(mergedDictionary));
            }
            finally
            {
                if (hunspell != null)
                {
                    foreach (var word in dictionary)
                    {
                        hunspell.Add(word);
                    }
                }
            }

            return hunspell;
        }

        private bool DidYouMean(string word)
        {
            if (this.SpellChecker != null && !SpellChecker.Spell(word))
            {
                var suggestions = SpellChecker.Suggest(word);
                if (suggestions.Count > 0)
                {
                    this.SpellFixList.Add(new SpellFix(word, suggestions));
                    return true;
                }
                else
                    return false;
            }
            else
            {
                this.SpellFixList = new List<SpellFix>();
                return false;
            }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public class SpellFix
        {
            public string Original { get; set; }
            public List<string> SuggestionList { get; set; }

            public SpellFix(string original, List<string> suggestionList)
            {
                this.Original = original;
                this.SuggestionList = suggestionList;
            }

            public SearchResultBadge CategoryBadge
            {
                get
                {
                    return SearchResultBadge.Create(SearchResultBadge.Badges.DidYouMean);
                }
            }

            public override string ToString()
            {
                if (this.SuggestionList.Count > 0)
                    return string.Format(SpellFixConfig.DidYouMeanMessage_, this.SuggestionList[0]);
                else
                    return string.Empty;
            }
        }

        private static string ConvertTurkishCharsToEnglish(string key)
        {
            String[] news = { "Ğ", "ğ", "Ü", "ü", "Ş", "ş", "İ", "ı", "Ö", "ö", "Ç", "ç" };
            String[] olds = { "g", "g", "u", "u", "s", "s", "i", "i", "ö", "o", "ç", "c" };

            for (int i = 0; i < olds.Length; i++)
            {
                key = key.Replace(olds[i], news[i]);
            }
            return key;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            this.OriginalDataSource = null;

            if (this.Tree != null)
                this.Tree.Destroy();

            if (this.DataSource != null)
                this.DataSource.Clear();

            this.SpellFixList.Clear();
            this.AddingQueue.Clear();
            this.RemovingQueue.Clear();
        }

        #endregion
    }

    public static class SpellFixConfig
    {
        public static string DidYouMeanMessage_ = "'{0}' mi demek istediniz?";

        public static readonly DependencyProperty DidYouMeanMessageProperty = DependencyProperty.RegisterAttached("DidYouMeanMessage", typeof(string), typeof(SpellFixConfig),
            new FrameworkPropertyMetadata(DidYouMeanMessage_,
                FrameworkPropertyMetadataOptions.AffectsRender,
                DidYouMeanMessageChanged));

        public static void SetDidYouMeanMessage(DependencyObject element, string value)
        {
            element.SetValue(DidYouMeanMessageProperty, value);
        }

        public static string GetDidYouMeanMessage(DependencyObject element)
        {
            return (string)element.GetValue(DidYouMeanMessageProperty);
        }

        private static void DidYouMeanMessageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DidYouMeanMessage_ = args.NewValue.ToString();
        }
    }
}
