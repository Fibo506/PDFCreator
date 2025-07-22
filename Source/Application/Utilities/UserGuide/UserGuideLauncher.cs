using System.IO;
using pdfforge.PDFCreator.Utilities.Messages;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Utilities.UserGuide;

/// <summary>
///     The UserGuideLauncher provides an easy mechanism of referencing and
///     launching sections of a user guide. Each topic is identified by an
///     enum value that is annotated with a <see cref="HelpTopicAttribute" />.
/// </summary>
public class UserGuideLauncher : IUserGuideLauncher
{
    private readonly IShowHelpHelper _showHelpHelper;
    private readonly IDirectory _directoryWrapper;
    private string _userGuideDirectory;
    private string _language;

    public UserGuideLauncher(IShowHelpHelper showHelpHelper, IDirectory directoryWrapper)
    {
        _showHelpHelper = showHelpHelper;
        _directoryWrapper = directoryWrapper;
    }

    /// <summary>
    ///     Launch the user guide with the given topic.
    /// </summary>
    /// <param name="topic">An enum value that is the symbolic reference to a help topic.</param>
    public void ShowHelpTopic(object topic)
    {
        if (_userGuideDirectory == null)
            return;

        var topicText = GetTopic(topic);

        if (topicText == null)
        {
            return;
        }

        _showHelpHelper.ShowHelp(_userGuideDirectory, $"{topicText}.html", GetLanguage, SetLanguage);
    }

    public void SetUserGuide(string path, string language)
    {
        if (!_directoryWrapper.Exists(path))
            throw new IOException($"The directory '{path}' does not exist");

        _language = language;

        _userGuideDirectory = $"{path}";
    }

    public void SetLanguage(string iso2)
    {
        _language = iso2;
    }

    public string GetLanguage()
    {
        return _language;
    }

    /// <summary>
    ///     Determine the string value of a help topic, which identifies the section within the user guide.
    /// </summary>
    /// <param name="value">An enum value that is the symbolic reference to a help topic.</param>
    /// <returns>The string representation of the help topic, i.e. the path within the file.</returns>
    private string GetTopic(object value)
    {
        return StringValueAttribute.GetValue(value);
    }
}
