﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.ModalWindow.Contracts
{
    public interface IMessageDialog : IDialog
    {
        string Message { get; set; }
    }
}
