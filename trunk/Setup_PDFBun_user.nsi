; example1.nsi
;
; This script is perhaps one of the simplest NSIs you can make. All of the
; optional settings are left to their default settings. The installer simply 
; prompts the user asking them where to install, and drops a copy of example1.nsi
; there. 

;--------------------------------

!define APP "PDFBun"
!define COM "HIRAOKA HYPERS TOOLS, Inc."
!define VER "0.4"
!define APV "0_4"

!define PROGID "HHT.PDFBun"

; bin\DEBUG

; The name of the installer
Name "${APP} ${VER}"

; The file to write
OutFile "Setup_${APP}_${APV}_user.exe"

; The default installation directory
InstallDir "$APPDATA\${APP}"

; Request application privileges for Windows Vista
RequestExecutionLevel user

!define DOTNET_VERSION "2.0"

!include "DotNET.nsh"
!include "LogicLib.nsh"

;--------------------------------

; Pages

Page license
Page directory
Page instfiles

LicenseData GNUGPL2.rtf

;--------------------------------

!ifdef SHCNE_ASSOCCHANGED
!undef SHCNE_ASSOCCHANGED
!endif
!define SHCNE_ASSOCCHANGED 0x08000000
!ifdef SHCNF_FLUSH
!undef SHCNF_FLUSH
!endif
!define SHCNF_FLUSH        0x1000

!macro UPDATEFILEASSOC
; Using the system.dll plugin to call the SHChangeNotify Win32 API function so we
; can update the shell.
  System::Call "shell32::SHChangeNotify(i,i,i,i) (${SHCNE_ASSOCCHANGED}, ${SHCNF_FLUSH}, 0, 0)"
!macroend

;--------------------------------

; The stuff to install
Section "" ;No components page, name is not important

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR

  !insertmacro CheckDotNET ${DOTNET_VERSION}

  ; Put file there
  File "1.ico"
  File /r /x "*.vshost.exe" "bin\DEBUG\*.*"
  
  WriteRegStr HKCU "Software\Classes\.pdf\OpenWithProgIDs" "${PROGID}" ""
  WriteRegStr HKCU "Software\Classes\.tif\OpenWithProgIDs" "${PROGID}" ""
  WriteRegStr HKCU "Software\Classes\.tiff\OpenWithProgIDs" "${PROGID}" ""

  WriteRegStr HKCU "Software\Classes\${PROGID}" "" "PDF分割"
  WriteRegStr HKCU "Software\Classes\${PROGID}\DefaultIcon" "" "$INSTDIR\1.ico"
  WriteRegStr HKCU "Software\Classes\${PROGID}\shell\open\command" "" '"$INSTDIR\PDFBun.exe" "%1"'

  DetailPrint "ファイル関連付けを更新中"
  !insertmacro UPDATEFILEASSOC

SectionEnd ; end the section
