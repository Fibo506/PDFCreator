namespace pdfforge.PDFCreator.Utilities.UserGuide;

/// <summary>
///     The IUserGuideLauncher provides an easy mechanism of referencing and
///     launching sections of a user guide. Each topic is identified by an
///     enum value that is annotated with a <see cref="HelpTopicAttribute" />.
/// </summary>
public interface IUserGuideLauncher
{
    /// <summary>
    ///     Launch the user guide with the given topic.
    /// </summary>
    /// <param name="topic">An enum value that is the symbolic reference to a help topic.</param>
    void ShowHelpTopic(object topic);

    /// <summary>
    ///     The the file or edition of the user guide that will be used. The result and interpretation
    ///     depends on the implementation and can be a filename, URL or similar.
    /// </summary>
    /// <param name="path">Path to the user guide to use.</param>
    /// <returns>True, if the user guide could be updated</returns>
    void SetUserGuide(string path, string iso2);

    void SetLanguage(string iso2);
    string GetLanguage();
}
