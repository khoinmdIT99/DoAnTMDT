ECHO OFF
cd DatabaseTools
CLS
:CONTEXT
ECHO ...............................................
ECHO PRESS 1 OR 2 to select your context, or 3 to EXIT.
ECHO ...............................................
ECHO.
ECHO 1 - ApplicationDBContext
ECHO 2 - ShopDBContext
ECHO 3 - EXIT
ECHO.
SET /P M=Type 1, 2 or 3 then press ENTER:
IF %M%==1 SET context="ApplicationDBContext"
IF %M%==2 SET context="ShopDBContext"
IF %M%==3 GOTO EOF
:MENU
ECHO.
ECHO ...............................................
ECHO PRESS 1, 2 OR 3 to select your task, or 4 to EXIT.
ECHO ...............................................
ECHO.
ECHO 1 - Create Migreation
ECHO 2 - Update Database
ECHO 3 - Remove Migreation
ECHO 4 - Drop Database
ECHO 5 - Change DB Context
ECHO 6 - EXIT
ECHO.
SET /P M=Type 1, 2, 3, or 4 then press ENTER:
IF %M%==1 GOTO CREATE
IF %M%==2 GOTO UPDATE
IF %M%==3 GOTO REMOVE
IF %M%==4 GOTO DROP
IF %M%==5 GOTO CONTEXT
IF %M%==6 GOTO EOF
:CREATE
ECHO ------------------Create Migreation------------------
SET NAME=""
SET /P NAME=Input Migrate name then press ENTER:
dotnet ef migrations add %NAME% --context %context%
GOTO MENU
:UPDATE
ECHO ------------------Update Database------------------
dotnet ef database update --context %context%
GOTO MENU
:REMOVE
ECHO ------------------Remove Migreation------------------
dotnet ef migrations remove --context %context%
GOTO MENU
:DROP
ECHO ------------------Drop Database------------------
dotnet ef database drop
GOTO MENU
