using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBST.Bibliography.Controllers {
    internal interface IController<TResult> : IDisposable {
        TResult Execute();
    }
}
