#!/bin/bash

# Build script for Windows deployment (cross-compiled from Linux)
# This script builds the TodoListApp for Windows x64

set -e

echo "🪟 Building TodoListApp for Windows..."

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean TodoListApp/TodoListApp.csproj -c Release

# Restore dependencies
echo "📦 Restoring dependencies..."
dotnet restore TodoListApp/TodoListApp.csproj

# Build and publish for Windows
echo "🔨 Building and publishing for Windows x64..."
dotnet publish TodoListApp/TodoListApp.csproj \
    -c Release \
    -r win-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:PublishAot=false \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -o ./publish/win-x64

echo ""
echo "✅ Build complete!"
echo "📁 Output location: ./publish/win-x64"
echo "📦 Executable: ./publish/win-x64/TodoListApp.exe"
echo ""
echo "📋 To distribute:"
echo "  1. Copy the entire 'publish/win-x64' folder to a Windows machine"
echo "  2. Run TodoListApp.exe"
echo ""
echo "💡 Note: The executable is self-contained and includes .NET runtime"
echo "   No .NET installation required on the target Windows system!"
