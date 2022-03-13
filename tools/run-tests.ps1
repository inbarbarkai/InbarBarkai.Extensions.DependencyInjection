$ErrorActionPreference = "Stop"

function Find-Report-Generator() {
    $nugetPath = [System.IO.Path]::Combine($env:USERPROFILE, ".nuget", "packages")
    if(Test-Path $nugetPath) {
        $nugetPath = Join-Path $nugetPath "ReportGenerator"
        if(Test-Path $nugetPath) {
            $reportGenerators = Get-ChildItem $nugetPath -Directory | Sort-Object { [Version]$_.Name } -Descending 
            if($reportGenerators.Count -gt 0) {
                $reportGenerator = $reportGenerators[0].FullName
                $frameworks = @("net6.0", "net5.0", "netcoreapp3.1")
                foreach($framework in $frameworks) {
                    $path = [System.IO.Path]::Combine($reportGenerator, "tools", $framework, "ReportGenerator.exe")
                    if(Test-Path $path) {
                        return $path
                    }
                }
            }
        }
    }
    return $null
}

function Download-Report-Generator() {
    '<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>netstandard2.0</TargetFramework></PropertyGroup></Project>' | Out-File "temp.csproj"
    dotnet add "temp.csproj" package ReportGenerator 
    Remove-Item "temp.csproj"
}

function Download-Or-Find-Report-Generator() {
    $reportGenerator = Find-Report-Generator
    if($null -eq $reportGenerator) {
        Download-Report-Generator
    }
    $reportGenerator = Find-Report-Generator
    return $reportGenerator
}

function Main() {
    $reportGenerator = Download-Or-Find-Report-Generator
    if($null -eq $reportGenerator) {
        "Could not find the report generator" | Write-Warning
    }
    if(Test-Path "TestResults") {
        Remove-Item "TestResults" -Force -Recurse
    }
    if(Test-Path "TestReport") {
        Remove-Item "TestReport" -Force -Recurse
    }
    dotnet test -c Release --logger trx --collect:"XPlat Code Coverage" --settings ../coverlet.runsettings -r "TestResults" ../InbarBarkai.Extensions.DependencyInjection.sln 
    if($null -ne $reportGenerator) {
        $command = '"' + $reportGenerator + '" -reports:TestResults/*/coverage*.xml -targetdir:./TestReport' 
        $command | Out-Host
        $command | cmd
        Start-Process -FilePath "TestReport/index.html"
    }
}

Main