#!/bin/bash

echo "🔍 AppImage Diagnostics"
echo "======================="
echo ""

# Check if AppImage exists
echo "1. Checking if AppImage exists..."
if [ -f "StudyBuddy-1.0.0-x86_64.AppImage" ]; then
    echo "   ✅ AppImage found"
    ls -lh StudyBuddy-1.0.0-x86_64.AppImage
    echo ""
    echo "   File type:"
    file StudyBuddy-1.0.0-x86_64.AppImage
else
    echo "   ❌ AppImage NOT found"
    echo "   Run ./build-appimage.sh first"
    exit 1
fi

echo ""
echo "2. Checking AppDir structure..."
if [ -d "TodoListApp.AppDir" ]; then
    echo "   ✅ AppDir exists"
    
    # Check AppRun
    if [ -f "TodoListApp.AppDir/AppRun" ]; then
        echo "   ✅ AppRun exists"
        if [ -x "TodoListApp.AppDir/AppRun" ]; then
            echo "   ✅ AppRun is executable"
        else
            echo "   ❌ AppRun is NOT executable"
        fi
    else
        echo "   ❌ AppRun NOT found"
    fi
    
    # Check executable
    if [ -f "TodoListApp.AppDir/usr/bin/TodoListApp" ]; then
        echo "   ✅ TodoListApp executable exists"
        if [ -x "TodoListApp.AppDir/usr/bin/TodoListApp" ]; then
            echo "   ✅ TodoListApp is executable"
        else
            echo "   ❌ TodoListApp is NOT executable"
        fi
    else
        echo "   ❌ TodoListApp executable NOT found"
    fi
    
    # Check libraries
    echo ""
    echo "   Shared libraries:"
    ls -1 TodoListApp.AppDir/usr/lib/*.so 2>/dev/null | wc -l | xargs echo "   Found libraries:"
    
else
    echo "   ❌ AppDir NOT found"
fi

echo ""
echo "3. Testing AppRun directly..."
if [ -f "TodoListApp.AppDir/AppRun" ]; then
    echo "   Running: ./TodoListApp.AppDir/AppRun"
    ./TodoListApp.AppDir/AppRun &
    APPRUN_PID=$!
    sleep 2
    if ps -p $APPRUN_PID > /dev/null; then
        echo "   ✅ AppRun started successfully (PID: $APPRUN_PID)"
        kill $APPRUN_PID 2>/dev/null
    else
        echo "   ❌ AppRun failed to start or crashed"
    fi
else
    echo "   ⚠️  Cannot test - AppRun not found"
fi

echo ""
echo "4. Testing executable directly..."
if [ -f "TodoListApp.AppDir/usr/bin/TodoListApp" ]; then
    echo "   Running: ./TodoListApp.AppDir/usr/bin/TodoListApp"
    cd TodoListApp.AppDir/usr/bin
    export LD_LIBRARY_PATH="../lib:${LD_LIBRARY_PATH}"
    ./TodoListApp &
    EXEC_PID=$!
    cd ../../..
    sleep 2
    if ps -p $EXEC_PID > /dev/null; then
        echo "   ✅ Executable started successfully (PID: $EXEC_PID)"
        kill $EXEC_PID 2>/dev/null
    else
        echo "   ❌ Executable failed to start or crashed"
        echo ""
        echo "   Checking for missing libraries:"
        ldd TodoListApp.AppDir/usr/bin/TodoListApp | grep "not found"
    fi
else
    echo "   ⚠️  Cannot test - executable not found"
fi

echo ""
echo "5. Checking system dependencies..."
echo "   Required libraries for Avalonia:"
LIBS=("libX11" "libICE" "libSM" "libfontconfig")
for lib in "${LIBS[@]}"; do
    if ldconfig -p | grep -q "$lib"; then
        echo "   ✅ $lib found"
    else
        echo "   ⚠️  $lib might be missing"
    fi
done

echo ""
echo "======================="
echo "Diagnostics complete!"
