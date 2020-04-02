import os
import re
import glob
from shutil import copyfile, rmtree
from pathlib import Path

version = '5.0.0'
tweetinviProjects = os.listdir('../src/')


def replace(filepath, regex, with_content):
    with open(filepath, 'r') as file:
        filedata = file.read()
    filedata = re.sub(regex, with_content, filedata)
    with open(filepath, 'w') as file:
        file.write(filedata)


def remove_files_in(path):
    files = glob.glob(path + '/*')
    for f in files:
        os.remove(f)


def update_version():
    nuspecVersionRegex = re.compile(r'<version>[0-9\.]*</version>')
    nuspecNewVersion = '<version>' + version + '</version>'
    replace('./TweetinviAPI/TweetinviAPI.nuspec',
            nuspecVersionRegex, nuspecNewVersion)
    replace('./TweetinviAPI-Symbols/TweetinviAPI-Symbols.nuspec',
            nuspecVersionRegex, nuspecNewVersion)
    print('updated nuspec versions to ' + version)

    csprojVersionRegex = re.compile(r'<VersionPrefix>[0-9\.]*</VersionPrefix>')
    csprojNewVersion = '<VersionPrefix>' + version + '</VersionPrefix>'

    for projectPath in tweetinviProjects:
        filePath = '../src/' + projectPath + '/' + projectPath + '.csproj'
        print('updating ' + projectPath + ' version to ' + version)
        replace(filePath, csprojVersionRegex, csprojNewVersion)


def clean_build():
    print('Cleaning build folders')
    releasePath = '../src/Tweetinvi/bin/'
    if os.path.exists(releasePath):
        rmtree(releasePath)

    releasePath = '../src/Tweetinvi.AspNet/bin/'
    if os.path.exists(releasePath):
        rmtree(releasePath)


def compile_tweetinvi():
    print('Compiling Tweetinvi...')
    os.system('dotnet build -c release ../src/Tweetinvi')
    os.system('dotnet build -c release ../src/Tweetinvi.AspNet')


def clean_nuget_folder():
    print('Cleaning nuget build...')

    cachePackagePath = 'C:/Users/linvi/.nuget/packages/tweetinviapi/' + version
    if os.path.exists(cachePackagePath):
        rmtree(cachePackagePath)

    cachePackagePath = 'C:/Users/linvi/.nuget/packages/tweetinviapi.aspnetplugin/' + version
    if os.path.exists(cachePackagePath):
        rmtree(cachePackagePath)

    files = glob.glob('./TweetinviAPI/*.nupkg')
    for f in files:
        os.remove(f)

    remove_files_in('./TweetinviAPI/lib/netstandard1.4')
    remove_files_in('./TweetinviAPI/lib/netstandard2.0')

    files = glob.glob('./TweetinviAPI-Symbols/*.snupkg')
    for f in files:
        os.remove(f)

    remove_files_in('./TweetinviAPI-Symbols/lib/netstandard1.4')
    remove_files_in('./TweetinviAPI-Symbols/lib/netstandard2.0')

    files = glob.glob('./TweetinviAspNet/*.nupkg')
    for f in files:
        os.remove(f)

    remove_files_in('./TweetinviAspNet/lib/netstandard2.0')
    remove_files_in('./TweetinviAspNet/lib/netcoreapp2.1')
    remove_files_in('./TweetinviAspNet/lib/netcoreapp3.1')


def build_tweetinvi_nuget_package():
    print('Building nuget package...')
    tweetinviBuildFiles = os.listdir(
        '../src/Tweetinvi/bin/Release/netstandard2.0')
    tweetinviDllFiles = list(
        filter(re.compile(r'.*\.dll').search, tweetinviBuildFiles))
    tweetinviXmlFiles = list(
        filter(re.compile(r'.*\.xml').search, tweetinviBuildFiles))

    for dll in tweetinviDllFiles:
        copyfile('../src/Tweetinvi/bin/Release/netstandard1.4/' +
                 dll, './TweetinviAPI/lib/netstandard1.4/' + dll)
        copyfile('../src/Tweetinvi/bin/Release/netstandard2.0/' +
                 dll, './TweetinviAPI/lib/netstandard2.0/' + dll)

    for xml in tweetinviXmlFiles:
        copyfile('../src/Tweetinvi/bin/Release/netstandard1.4/' +
                 xml, './TweetinviAPI/lib/netstandard1.4/' + xml)
        copyfile('../src/Tweetinvi/bin/Release/netstandard2.0/' +
                 xml, './TweetinviAPI/lib/netstandard2.0/' + xml)

    os.system('cd TweetinviAPI && ..\\nuget.exe pack')


def build_tweetinvi_nuget_symbols():
    print('building symbols package')

    tweetinviBuildFiles = os.listdir(
        '../src/Tweetinvi/bin/Release/netstandard2.0')
    symbolsFilter = re.compile(r'.*\.pdb')
    tweetinviSymbolFiles = list(
        filter(symbolsFilter.search, tweetinviBuildFiles))

    for pdb in tweetinviSymbolFiles:
        copyfile('../src/Tweetinvi/bin/Release/netstandard1.4/' + pdb,
                 './TweetinviAPI-Symbols/lib/netstandard1.4/' + pdb)
        copyfile('../src/Tweetinvi/bin/Release/netstandard2.0/' + pdb,
                 './TweetinviAPI-Symbols/lib/netstandard2.0/' + pdb)

    os.system('cd TweetinviAPI-Symbols && ..\\nuget.exe pack')
    os.rename('./TweetinviAPI-Symbols/TweetinviAPI.' + version + '.nupkg',
              './TweetinviAPI-Symbols/TweetinviAPI.' + version + '.snupkg')


def build_aspNet_nuget_package():
    def copy_aspnet_file(filepath):
        copyfile('../src/Tweetinvi.AspNet/bin/Release/' +
                 filepath, './TweetinviAspNet/lib/' + filepath)

    Path("./TweetinviAspNet/lib/netstandard2.0").mkdir(parents=True, exist_ok=True)
    Path("./TweetinviAspNet/lib/netcoreapp2.1").mkdir(parents=True, exist_ok=True)
    Path("./TweetinviAspNet/lib/netcoreapp3.1").mkdir(parents=True, exist_ok=True)

    copy_aspnet_file('netstandard2.0/Tweetinvi.AspNet.dll')
    copy_aspnet_file('netstandard2.0/Tweetinvi.AspNet.xml')

    copy_aspnet_file('netcoreapp2.1/Tweetinvi.AspNet.dll')
    copy_aspnet_file('netcoreapp2.1/Tweetinvi.AspNet.xml')

    copy_aspnet_file('netcoreapp3.1/Tweetinvi.AspNet.dll')
    copy_aspnet_file('netcoreapp3.1/Tweetinvi.AspNet.xml')

    os.system('cd TweetinviAspNet && ..\\nuget.exe pack')


update_version()
clean_build()
compile_tweetinvi()
clean_nuget_folder()

build_tweetinvi_nuget_package()
build_tweetinvi_nuget_symbols()
build_aspNet_nuget_package()
