using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using pdfforge.PDFCreator.Core.StartupInterface;

namespace pdfforge.PDFCreator.Core.ComImplementation;

public class ComStartupConditionChecker
{
    private readonly IList<IStartupCondition> _startupConditions;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public ComStartupConditionChecker(IList<IStartupCondition> startupConditions)
    {
        _startupConditions = startupConditions.Where(s => !s.CanRequestUserInteraction).ToList();
    }

    public Tuple<bool, string> CheckStartupConditions()
    {
        _logger.Trace($"Checking {_startupConditions.Count} startup conditions");

        foreach (var startupCondition in _startupConditions)
        {
            var result = startupCondition.Check();

            _logger.Trace($"{startupCondition.GetType().Name}: {result.IsSuccessful}");

            if (!result.IsSuccessful)
            {
                var message = $"Startup condition '{startupCondition.GetType().Name}' failed";

                if (result.ShowMessage)
                    message += ": " + result.Message;

                _logger.Error(message);

                return new Tuple<bool, string>(false, message);
            }
        }

        return new Tuple<bool, string>(true, "");
    }
}
