using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuYu.Components
{
    /// <summary>
    /// Delegate InitializePushTracking
    /// </summary>
    /// <param name="utctimestamp"></param>
    /// <returns></returns>
    public delegate IEnumerable<string> InitializePushTracking(DateTime utctimestamp);

    /// <summary>
    /// Delegate UpdatePushResult
    /// </summary>
    /// <param name="pushResult"></param>
    public delegate void UpdatePushResult(List<Result> pushResult);

    /// <summary>
    /// Delegate ReportException
    /// </summary>
    /// <param name="exception">The exception.</param>
    public delegate void ReportException(Exception exception);
}
