@SET config=Release

REM dotnet restore -v q

REM dotnet build -c %config% -v q

REM dotnet test .\src\Vertica.Utilities.Tests\Vertica.Utilities.Tests.csproj -c %config% -v q

dotnet pack .\src\Vertica.Utilities\Vertica.Utilities.csproj -c %config% --no-build -v m

@ECHO.
@ECHO -----------------------------------------------------------------------------
@ECHO ^|  SUPER-GREEN. Package available in .\src\Vertica.Utilities\bin\%config%\   ^|
@ECHO -----------------------------------------------------------------------------
@ECHO.
