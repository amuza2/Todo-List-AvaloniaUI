#!/bin/bash

# Build script for all platforms
# Builds TodoListApp for Linux and Windows

set -e

echo "🚀 Building TodoListApp for all platforms..."
echo ""

# Build for Linux
echo "═══════════════════════════════════════"
echo "Building for Linux x64..."
echo "═══════════════════════════════════════"
./build-linux.sh

echo ""
echo ""

# Build for Windows
echo "═══════════════════════════════════════"
echo "Building for Windows x64..."
echo "═══════════════════════════════════════"
./build-windows.sh

echo ""
echo ""
echo "═══════════════════════════════════════"
echo "✅ All builds complete!"
echo "═══════════════════════════════════════"
echo ""
echo "📦 Build outputs:"
echo "  Linux:   ./publish/linux-x64/TodoListApp"
echo "  Windows: ./publish/win-x64/TodoListApp.exe"
echo ""
echo "💡 To create Linux AppImage:"
echo "  ./build-appimage.sh"
