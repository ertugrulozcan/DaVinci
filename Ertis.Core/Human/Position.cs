using System;
using System.Collections.Generic;

namespace Ertis.Core.Human
{
    public class Position
    {
        #region Fields

        private int id;
        private string name;
        private string description;

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
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        #endregion

        #region Constructors

        public Position(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region Methods

        public static List<Position> ToListFromJson(string jsonStr)
        {
            List<Position> positionList = new List<Position>();

            var json = Newtonsoft.Json.Linq.JArray.Parse(jsonStr);
            foreach (var c in json.Children())
            {
                positionList.Add(CreateFromJson(c.ToString()));
            }

            return positionList;
        }

        public static Position CreateFromJson(string json)
        {
            var positionDefinition = new
            {
                id = 0,
                name = "",
                description = ""
            };

            var anonymousPosition = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, positionDefinition);

            return new Position(anonymousPosition.id, anonymousPosition.name) { Description = anonymousPosition.description };
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Position))
                return false;

            return this.Id == (obj as Position).Id;
        }

        #endregion
    }
}
