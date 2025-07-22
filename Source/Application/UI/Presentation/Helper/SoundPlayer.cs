using System.Media;

namespace pdfforge.PDFCreator.UI.Presentation.Helper;

public interface ISoundPlayer
{
    void Play(SystemSound sound);
}

public class SoundPlayer : ISoundPlayer
{
    public void Play(SystemSound sound)
    {
        sound.Play();
    }
}

public class DesignTimeSoundPlayer : ISoundPlayer
{
    public void Play(SystemSound sound)
    {
        //Play no f***ing sound!
    }
}
