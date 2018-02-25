using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Client.Models
{
    public class ResponseResult
    {
        #region Fields

        private bool isSuccess;
        private string message;
        private object data;

        #endregion

        #region Properties

        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }

            protected set
            {
                this.isSuccess = value;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }

            protected set
            {
                this.message = value;
            }
        }

        public object Data
        {
            get
            {
                return data;
            }

            set
            {
                this.data = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor 1
        /// </summary>
        /// <param name="success"></param>
        public ResponseResult(bool success)
        {
            this.IsSuccess = success;
        }

        /// <summary>
        /// Constructor 1
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public ResponseResult(bool success, string message)
        {
            this.IsSuccess = success;
            this.Message = message;
        }

        /// <summary>
        /// Constructor 3
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public ResponseResult(bool success, string message, object data)
        {
            this.IsSuccess = success;
            this.Message = message;
            this.Data = data;
        }

        #endregion
    }

    
    public class ResponseResult<T> : ResponseResult
    {
        #region Fields
        
        private T data;

        #endregion

        #region Properties
        
        public new T Data
        {
            get
            {
                return data;
            }

            private set
            {
                this.data = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public ResponseResult(bool success, string message, T data) : base(success, message)
        {
            this.Data = data;
        }

        #endregion
    }
}
