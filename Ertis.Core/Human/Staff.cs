using System;
using System.Collections.Generic;
using Ertis.Core.Organization;
using Ertis.Core.Profile;
using Ertis.Core.Serialization;

namespace Ertis.Core.Human
{
    public class Staff : ICloneable, IDisposable
    {
        #region Fields

        private int id;
        private int credentialsId;
        private Credentials userCredentials;
        private int userId;
        private User ertisUser;
        private int positionId;
        private Position position;
        private int departmentId;
        private Department department;
        private int sectionId;
        private Section section;
        private int teamId;
        private Team team;

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

        public Credentials UserCredentials
        {
            get
            {
                return userCredentials;
            }

            set
            {
                userCredentials = value;
            }
        }

        public int CredentialsId
        {
            get
            {
                if (this.UserCredentials != null)
                    return this.UserCredentials.Id;
                
                return credentialsId;
            }

            internal set
            {
                credentialsId = value;
            }
        }

        public User ErtisUser
        {
            get
            {
                return ertisUser;
            }

            set
            {
                ertisUser = value;
            }
        }

        public int UserId
        {
            get
            {
                if (this.ErtisUser != null)
                    return this.ErtisUser.Id;

                return userId;
            }

            internal set
            {
                userId = value;
            }
        }

        public Position Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public int PositionId
        {
            get
            {
                if (this.Position != null)
                    return this.Position.Id;

                return positionId;
            }

            internal set
            {
                positionId = value;
            }
        }

        public Department Department
        {
            get
            {
                return department;
            }

            set
            {
                department = value;
            }
        }

        public int DepartmentId
        {
            get
            {
                if (this.Department != null)
                    return this.Department.Id;

                return departmentId;
            }

            internal set
            {
                departmentId = value;
            }
        }

        public Section Section
        {
            get
            {
                return section;
            }

            set
            {
                section = value;
            }
        }

        public int SectionId
        {
            get
            {
                if (this.Section != null)
                    return this.Section.Id;

                return sectionId;
            }

            internal set
            {
                sectionId = value;
            }
        }

        public Team Team
        {
            get
            {
                return team;
            }

            set
            {
                team = value;
            }
        }

        public int TeamId
        {
            get
            {
                if (this.Team != null)
                    return this.Team.Id;

                return teamId;
            }

            internal set
            {
                teamId = value;
            }
        }

        public string Name
        {
            get
            {
                return this.UserCredentials.Name;
            }
        }

        public string Surname
        {
            get
            {
                return this.UserCredentials.Surname;
            }
        }

        public string FullName
        {
            get
            {
                return this.Name + " " + this.Surname;
            }
        }

        public DateTime? BirthDate
        {
            get
            {
                return this.UserCredentials.BirthDate;
            }
        }

        public string EmailAddress
        {
            get
            {
                return this.UserCredentials.EmailAddress;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return this.UserCredentials.PhoneNumber;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Use only serialization
        /// </summary>
        private Staff()
        {

        }

        public Staff(int id, int credentialsId, int userId, int positionId, int departmentId, int sectionId, int teamId)
        {
            this.Id = id;
            this.UserId = userId;
            this.CredentialsId = credentialsId;
            this.PositionId = positionId;
            this.DepartmentId = departmentId;
            this.SectionId = sectionId;
            this.TeamId = teamId;
        }

        public Staff(int id, User user, Position position)
        {
            this.Id = id;
            this.ErtisUser = user;
            this.Position = position;
        }

        public Staff(int id, string name, string surname, DateTime? birthDate, string emailAddress, string phoneNumber, Position position)
        {
            this.Id = id;
            this.UserCredentials = new Credentials(0, name, surname, birthDate, emailAddress, phoneNumber);
            this.Position = position;
        }

        public Staff(
            int id,
            int userId,
            string name,
            string surname,
            DateTime? birthDate,
            string emailAddress,
            string phoneNumber,
            int positionId,
            int departmentId,
            int sectionId,
            int teamId)
        {
            this.Id = id;
            this.UserId = userId;
            this.UserCredentials = new Credentials(0, name, surname, birthDate, emailAddress, phoneNumber);
            this.PositionId = positionId;
            this.DepartmentId = departmentId;
            this.SectionId = sectionId;
            this.TeamId = teamId;
        }

        #endregion

        #region Methods
        
        public override string ToString()
        {
            return this.FullName;
        }

        public object Clone()
        {
            return new Staff(this.Id, this.ErtisUser, this.Position)
            {
                CredentialsId = this.CredentialsId,
                DepartmentId = this.DepartmentId,
                PositionId = this.PositionId,
                SectionId = this.SectionId,
                TeamId = this.TeamId,
                UserId = this.UserId,
                UserCredentials = this.UserCredentials.Clone() as Credentials,
                Department = this.Department,
                ErtisUser = this.ErtisUser,
                Position = this.Position,
                Section = this.Section,
                Team = this.Team
            };
        }

        public void Dispose()
        {
            this.UserCredentials = null;
            ErtisUser = null;
        }
        
        ~Staff()
        {
            this.Dispose();
        }
        
        #endregion
    }
}
