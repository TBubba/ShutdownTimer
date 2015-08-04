@ECHO OFF

REM NOTE: This batch is parameters only (for parameters not passed, default values will be used)

REM This batch should be run when you want to create a release build
REM This will copy the current build and post it to the output folder (under said version name)
REM
REM Arguments:
REM 	%1: Build Directory (where the software was compiled)
REM 	%2: Target Directory (where the release build (output) should be)

REM Check whether or not the first parameter exists
IF [%1] == [] ( 
  SET ReleaseDir=%CD%\ShutdownTimer\bin\Release\
) ELSE ( 
  SET ReleaseDir=%1
)

REM Check whether or not the second parameter exists
IF [%2] == [] (   
  SET Target=%CD%\output\unknown\
) ELSE ( 
  SET Target=%2
)

REM Export x86 (32 bit)
ECHO %ReleaseDir%x86\ %Target%x86\
CALL ExportBuild %ReleaseDir%x86\ %Target%x86\

REM Export x64 (64 bit)
CALL ExportBuild %ReleaseDir%x64\ %Target%x64\

PAUSE