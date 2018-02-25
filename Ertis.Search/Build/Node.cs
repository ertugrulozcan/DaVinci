using Ertis.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Search.Build
{
    public class Node
    {
        #region Fields

        private char nodeKey;
        private List<Node> children;
        private readonly HashSet<object> objects;

        #endregion

        #region Properties

        public char NodeKey
        {
            get
            {
                return nodeKey;
            }

            private set
            {
                nodeKey = value;
            }
        }

        public List<Node> Children
        {
            get
            {
                return children;
            }

            private set
            {
                children = value;
            }
        }

        public HashSet<object> Objects
        {
            get
            {
                return objects;
            }
        }

        #endregion

        #region Constructors

        private Node(char key)
        {
            this.NodeKey = key;
            this.Children = new List<Node>();
            this.objects = new HashSet<object>();
        }

        private static Node Create(char key)
        {
            return new Node(key);
        }

        public static Node CreateWithSource(char key, List<KeyValuePair<string, object>> source)
        {
            var node = Create(key);

            if (source.Count > 0)
                node.SetChildSources(source);

            return node;
        }

        #endregion

        #region Methods

        private void SetChildSources(List<KeyValuePair<string, object>> source)
        {
            var visitedItems = new Dictionary<string, int>();

            foreach (var vp in source)
            {
                if (!Objects.Contains(vp.Value))
                    Objects.Add(vp.Value);

                if (string.IsNullOrEmpty(vp.Key) || string.IsNullOrWhiteSpace(vp.Key) || visitedItems.ContainsKey(vp.Key))
                {
                    continue;
                }
                else
                {
                    char key = CharTool.ToLowerFast(vp.Key[0]);
                    Node node = Create(key);
                    // Düğüm oluştur.
                    var subNodeSource = this.CreateSubNodeSource(source, key, visitedItems);

                    if (subNodeSource.Count > 0)
                        node.SetChildSources(subNodeSource);

                    this.Children.Add(node);
                }
            }
        }

        private List<KeyValuePair<string, object>> CreateSubNodeSource(List<KeyValuePair<string, object>> mainSource, char key, Dictionary<string, int> visitedItems)
        {
            List<KeyValuePair<string, object>> subNodeSource = new List<KeyValuePair<string, object>>();

            foreach (var vp in mainSource)
            {
                if (!string.IsNullOrEmpty(vp.Key))
                {
                    if (CharTool.ToLowerFast(vp.Key[0]) == key)
                    {
                        var item = new KeyValuePair<string, object>(vp.Key.Substring(1, vp.Key.Length - 1), vp.Value);
                        subNodeSource.Add(item);
                        if (!visitedItems.ContainsKey(vp.Key))
                            visitedItems.Add(vp.Key, 1);
                    }
                }
            }

            return subNodeSource;
        }

        public Node GetChildNode(char c)
        {
            var invar = CharTool.ToLowerFast(c);
            var child = this.Children.FirstOrDefault(x => CharTool.ToLowerFast(x.NodeKey) == invar);
            return child;
        }

        public void AddNewItem(string path, object item)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                return;

            char keyChar = CharTool.ToLowerFast(path[0]);
            if (!this.Children.Any(x => x.NodeKey == keyChar))
            {
                // Düğüm oluştur.
                if (!this.Objects.Contains(item))
                    this.Objects.Add(item);

                var child = CreateWithSource(keyChar, new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>(path.Substring(1, path.Length - 1), item) });
                this.Children.Add(child);
            }
            else
            {
                // Düğüm zaten oluşturulmuş.
                Node child = this.GetChildNode(keyChar);

                if (!child.Objects.Contains(item))
                    child.Objects.Add(item);

                child.AddNewItem(path.Substring(1, path.Length - 1), item);
            }
        }

        public void RemoveItem(string path, object item)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                return;

            char keyChar = path[0];
            Node child = this.GetChildNode(keyChar);
            if (child != null)
            {
                child.RemoveItem(path.Substring(1, path.Length - 1), item);

                if (child.Objects.Count == 0)
                    this.Children.Remove(child);
            }

            if (this.Objects.Contains(item))
                this.Objects.Remove(item);
        }

        public override string ToString()
        {
            return "[" + this.NodeKey + "]";
        }

        #endregion

        #region Dispose

        public void Destroy()
        {
            foreach (var child in this.Children)
                child.Destroy();

            this.Objects.Clear();
            this.Children.Clear();
        }

        #endregion

        #region Equals Implementation

        public override bool Equals(object obj)
        {
            if (!(obj is Node))
                return false;

            char c1 = CharTool.ToLowerFast((obj as Node).NodeKey);
            char c2 = CharTool.ToLowerFast(this.NodeKey);
            return c1 == c2;
        }

        public override int GetHashCode()
        {
            return (int)this.NodeKey;
        }

        #endregion
    }
}
