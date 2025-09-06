using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using StaticViewLocator;
using TodoListApp.ViewModels;

namespace TodoListApp;

[StaticViewLocator]
public partial class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }

        var type = data.GetType();

        if (s_views.TryGetValue(type, out var func))
        {
            return func.Invoke();
        }

        return new TextBlock { Text = data.ToString() ?? string.Empty };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}