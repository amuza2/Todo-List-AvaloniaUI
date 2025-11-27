#!/bin/bash

set -e  # Exit on error

VERSION="1.0.0"
ARCH="x86_64"
APP_NAME="TodoListApp"
APPDIR="${APP_NAME}.AppDir"

echo "🚀 Building ${APP_NAME} AppImage v${VERSION}..."

# Check for appimagetool
if ! command -v appimagetool &> /dev/null; then
    echo "⚠️  appimagetool not found. Downloading..."
    wget -q https://github.com/AppImage/AppImageKit/releases/download/continuous/appimagetool-${ARCH}.AppImage -O appimagetool
    chmod +x appimagetool
    APPIMAGETOOL="./appimagetool"
else
    APPIMAGETOOL="appimagetool"
fi

# Clean previous builds
echo "🧹 Cleaning previous builds..."
rm -rf ./publish/linux-x64
rm -rf ./${APPDIR}/usr

# Publish the application
echo "📦 Publishing application..."
dotnet publish ${APP_NAME}/${APP_NAME}.csproj \
  -c Release \
  -r linux-x64 \
  --self-contained \
  -p:PublishSingleFile=true \
  -p:PublishTrimmed=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -o ./publish/linux-x64

# Create AppDir structure
echo "📁 Creating AppImage directory structure..."
mkdir -p ${APPDIR}/usr/bin
mkdir -p ${APPDIR}/usr/lib
mkdir -p ${APPDIR}/usr/share/applications
mkdir -p ${APPDIR}/usr/share/icons/hicolor/256x256/apps

# Copy application files
echo "📋 Copying application files..."
cp ./publish/linux-x64/${APP_NAME} ${APPDIR}/usr/bin/

# Copy shared libraries
echo "📚 Copying shared libraries..."
for lib in ./publish/linux-x64/*.so; do
    if [ -f "$lib" ]; then
        cp "$lib" ${APPDIR}/usr/lib/
        echo "  ✅ Copied $(basename $lib)"
    fi
done

# Copy icon (using the tasklist icon as app icon)
echo "🎨 Copying icon..."
if [ -f "${APP_NAME}/Assets/icons8-tasklist-48.png" ]; then
    cp ${APP_NAME}/Assets/icons8-tasklist-48.png ${APPDIR}/todolistapp.png
    cp ${APP_NAME}/Assets/icons8-tasklist-48.png ${APPDIR}/usr/share/icons/hicolor/256x256/apps/todolistapp.png
    echo "  ✅ Icon copied"
fi

# Create .DirIcon symlink
echo "🔗 Creating .DirIcon symlink..."
cd ${APPDIR}
ln -sf todolistapp.png .DirIcon
cd ..

# Copy desktop file
echo "📄 Copying desktop file..."
cp ${APPDIR}/${APP_NAME}.desktop ${APPDIR}/usr/share/applications/

# Make AppRun executable
echo "🔧 Making AppRun executable..."
chmod +x ${APPDIR}/AppRun

# Build AppImage
echo "🔨 Building AppImage..."
ARCH=${ARCH} ${APPIMAGETOOL} ${APPDIR} StudyBuddy-${VERSION}-${ARCH}.AppImage

echo ""
echo "✅ AppImage created successfully!"
echo "📦 File: StudyBuddy-${VERSION}-${ARCH}.AppImage"
echo ""
echo "To run:"
echo "  chmod +x StudyBuddy-${VERSION}-${ARCH}.AppImage"
echo "  ./StudyBuddy-${VERSION}-${ARCH}.AppImage"
