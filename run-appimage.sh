#!/bin/bash

# Helper script to run the AppImage with diagnostics

APPIMAGE="StudyBuddy-1.0.0-x86_64.AppImage"

echo "🚀 Attempting to run $APPIMAGE"
echo ""

# Check if AppImage exists
if [ ! -f "$APPIMAGE" ]; then
    echo "❌ Error: $APPIMAGE not found"
    echo ""
    echo "Build it first with:"
    echo "  ./build-appimage.sh"
    exit 1
fi

# Check if executable
if [ ! -x "$APPIMAGE" ]; then
    echo "⚠️  AppImage not executable, fixing..."
    chmod +x "$APPIMAGE"
fi

# Check for FUSE
if ! which fusermount &> /dev/null; then
    echo "⚠️  FUSE2 not found!"
    echo ""
    echo "On Arch/EndeavourOS, install with:"
    echo "  sudo pacman -S fuse2"
    echo ""
    echo "Extracting AppImage instead..."
    echo ""
    ./"$APPIMAGE" --appimage-extract
    echo ""
    echo "Running extracted version..."
    cd squashfs-root
    ./AppRun "$@"
    exit $?
fi

# Run the AppImage
echo "✅ Running AppImage..."
echo ""
./"$APPIMAGE" "$@" 2>&1

# Capture exit code
EXIT_CODE=$?

if [ $EXIT_CODE -ne 0 ]; then
    echo ""
    echo "❌ AppImage exited with code: $EXIT_CODE"
    echo ""
    echo "Troubleshooting steps:"
    echo "1. Check if required libraries are installed:"
    echo "   sudo apt install libx11-6 libice6 libsm6 libfontconfig1"
    echo ""
    echo "2. Try extracting and running manually:"
    echo "   ./$APPIMAGE --appimage-extract"
    echo "   cd squashfs-root"
    echo "   ./AppRun"
    echo ""
    echo "3. Check the diagnostic script:"
    echo "   ./diagnose-appimage.sh"
fi

exit $EXIT_CODE
