@echo off
set "source=%cd%\CanIUse"
set "destination=%LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\"

echo Moving directory...
move "%source%" "%destination%"

echo Move completed.
pause