using SystemInterface.Microsoft.Win32;

namespace pdfforge.PDFCreator.Utilities.Registry;

public interface IRegistryUtility
{
    /// <summary>
    ///     Renames a subkey of the passed in registry key since
    ///     the Framework totally forgot to include such a handy feature.
    /// </summary>
    /// <param name="parentKey">
    ///     The RegistryKey that contains the subkey
    ///     you want to rename (must be writeable)
    /// </param>
    /// <param name="subKeyName">
    ///     The name of the subkey that you want to rename
    /// </param>
    /// <param name="newSubKeyName">The new name of the RegistryKey</param>
    /// <returns>True if succeeds</returns>
    bool RenameSubKey(IRegistryKey parentKey, string subKeyName, string newSubKeyName);

    /// <summary>
    ///     Copy a registry key.  The parentKey must be writeable.
    /// </summary>
    /// <param name="parentKey"></param>
    /// <param name="keyNameToCopy"></param>
    /// <param name="newKeyName"></param>
    /// <returns></returns>
    bool CopyKey(IRegistryKey parentKey, string keyNameToCopy, string newKeyName);
}
