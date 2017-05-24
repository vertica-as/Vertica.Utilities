$wc = New-Object 'System.Net.WebClient'
$test_results = Resolve-Path .\src\Vertica.Utilities.Tests\TestResults\test-results.xml
$wc.UploadFile("https://ci.appveyor.com/api/testresults/mstest/$($env:APPVEYOR_JOB_ID)", $test_results)