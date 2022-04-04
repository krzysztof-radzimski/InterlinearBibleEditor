using System;

namespace WBST.Bibliography.Controllers {
    internal interface IController<TResult> : IDisposable {
        TResult Execute();
    }
}
