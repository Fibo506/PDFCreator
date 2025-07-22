using System;
using System.ComponentModel;

namespace pdfforge.PDFCreator.UI.ComWrapper;

public class ComDependencyBuilder
{
    private readonly dynamic _comDependencyBuilder;

    internal ComDependencyBuilder(dynamic comDependencyBuilder)
    {
        _comDependencyBuilder = comDependencyBuilder;
    }
    public Action<Container> ModifyRegistrations
    {
        get { return _comDependencyBuilder.ModifyRegistrations; }
    }

    public ComDependencies ComDependencies()
    {
        return new ComDependencies(_comDependencyBuilder.ComDependencies());
    }

    public void ResetDependencies()
    {
        _comDependencyBuilder.ResetDependencies();
    }

    public ComDependencies BuildComDependencies()
    {
        return new ComDependencies(_comDependencyBuilder.BuildComDependencies());
    }

}
