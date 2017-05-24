$env:Path = "C:\\Python34;C:\\Python34\\Scripts;" + $env:Path
pip install codecov
nuget install opencover -OutputDirectory .\tools