using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoListApp.Models;

public partial class TodoItem : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private bool _isCompleted;

    [ObservableProperty]
    private DateTime _createdDate = DateTime.Now;

    public TodoItem() { }

    public TodoItem(string title, string description = "")
    {
        Title = title;
        Description = description;
        CreatedDate = DateTime.Now;
    }
}