using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Shared.ModalWindow.Contracts
{
    public interface IWaitDialog : IMessageDialog
    {
        Action WorkerReady { get; set; }

        bool CloseWhenWorkerFinished { get; set; }
        string ReadyMessage { get; set; }

        void Show(Action workerMethod);
        void InvokeUICall(Action uiWorker);
    }
}
