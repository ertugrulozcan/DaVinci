using System;
using Ertis.Core.Data;

namespace Ertis.Core.Server
{
    public static class Errors
    {
        public static Error UNKNOWN_ERROR = Error.New(4000, "Unknown error");
        public static Error USERCARD_INVALID = Error.New(4001, "User informations incorrectly or not valid!");
        public static Error EMAIL_ALREADY_EXIST = Error.New(4002, "Email address is already exist!");

        public static Error STAFF_CREATE_ERROR = Error.New(5002, "Staff could not be created!");
        public static Error STAFF_UPDATE_ERROR = Error.New(5003, "Staff could not be updated!");
        public static Error STAFF_DELETE_ERROR = Error.New(5003, "Staff could not be deleted!");

        public static Error DEPARTMENT_CREATE_ERROR = Error.New(5102, "Department could not be created!");
        public static Error DEPARTMENT_UPDATE_ERROR = Error.New(5103, "Department could not be updated!");
        public static Error DEPARTMENT_DELETE_ERROR = Error.New(5103, "Department could not be deleted!");

        public static Error SECTION_CREATE_ERROR = Error.New(5202, "Section could not be created!");
        public static Error SECTION_UPDATE_ERROR = Error.New(5203, "Section could not be updated!");
        public static Error SECTION_DELETE_ERROR = Error.New(5203, "Section could not be deleted!");

        public static Error TEAM_CREATE_ERROR = Error.New(5302, "Team could not be created!");
        public static Error TEAM_UPDATE_ERROR = Error.New(5303, "Team could not be updated!");
        public static Error TEAM_DELETE_ERROR = Error.New(5303, "Team could not be deleted!");

        public static Error POSITION_CREATE_ERROR = Error.New(5402, "Position could not be created!");
        public static Error POSITION_UPDATE_ERROR = Error.New(5403, "Position could not be updated!");
        public static Error POSITION_DELETE_ERROR = Error.New(5403, "Position could not be deleted!");

        public static Error CreateUncategorizedError(string errorMessage)
        {
            return Error.New(0, errorMessage);
        }
    }
}
