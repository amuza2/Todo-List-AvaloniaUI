using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TodoListApp.Models;

namespace TodoListApp.Services;

[JsonSerializable(typeof(List<TodoItem>))]
[JsonSerializable(typeof(TodoItem))]
[JsonSerializable(typeof(TaskCategory))]
[JsonSerializable(typeof(TaskPriority))]
[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Converters = [typeof(JsonStringEnumConverter<TaskCategory>), typeof(JsonStringEnumConverter<TaskPriority>)])]
public partial class TodoJsonContext : JsonSerializerContext
{
}