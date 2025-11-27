#!/bin/bash

# Build script for Linux deployment (standalone executable)
# This script builds the TodoListApp for Linux x64

set -e

echo "🚀 Building TodoListApp for Linux..."

# Clean previous builds
echo "🧹 Cleaning previous builds..."
dotnet clean TodoListApp/TodoListApp.csproj -c Release

# Restore dependencies
echo "📦 Restoring dependencies..."
dotnet restore TodoListApp/TodoListApp.csproj

# Build and publish
echo "🔨 Building and publishing..."
dotnet publish TodoListApp/TodoListApp.csproj \
    -c Release \
    -r linux-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -o ./publish/linux-x64

echo ""
echo "✅ Build complete!"
echo "📁 Output location: ./publish/linux-x64"
echo ""
echo "To run the application:"
echo "  cd publish/linux-x64"
echo "  chmod +x TodoListApp"
echo "  ./TodoListApp"
echo ""
echo "💡 Tip: To create an AppImage, run: ./build-appimage.sh"
