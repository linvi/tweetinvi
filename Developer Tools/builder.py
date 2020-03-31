import os
import re
import glob
from shutil import copyfile, rmtree

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
    replace('./TweetinviAPI/TweetinviAPI.nuspec', nuspecVersionRegex, nuspecNewVersion)
    replace('./TweetinviAPI-Symbols/TweetinviAPI-Symbols.nuspec', nuspecVersionRegex, nuspecNewVersion)
    print('updated nuspec versions to ' + version)

    csprojVersionRegex = re.compile(r'<VersionPrefix>[0-9\.]*</VersionPrefix>')
    csprojNewVersion = '<VersionPrefix>' + version + '</VersionPrefix>'

    for projectPath in tweetinviProjects:
        filePath = '../src/' + projectPath + '/' + projectPath + '.csproj'
        print('updating ' + projectPath + ' version to ' + version)
        replace(filePath, csprojVersionRegex, csprojNewVersion)


def clean_build():
    releasePath = '../src/Tweetinvi/bin/'
    if os.path.exists(releasePath):
        rmtree(releasePath)


def compile():
    print('compiling...')
    os.system('dotnet build -c release ../src/Tweetinvi')
    os.system('dotnet build -c release ../src/Tweetinvi.AspNet')


def clean_nuget_folder():
    print('cleaning up nuget build')

    files = glob.glob('./TweetinviAPI/*.nupkg')

    remove_files_in('./TweetinviAPI/lib/netstandard1.4')
    remove_files_in('./TweetinviAPI/lib/netstandard2.0')
    remove_files_in('./TweetinviAPI-Symbols/lib/netstandard1.4')
    remove_files_in('./TweetinviAPI-Symbols/lib/netstandard2.0')


def build_nuget_package():
    print('building nuget package')
    tweetinviBuildFiles = os.listdir('../src/Tweetinvi/bin/Release/netstandard2.0')
    tweetinviDllFiles = list(filter(re.compile(r'.*\.dll').search, tweetinviBuildFiles))
    tweetinviXmlFiles = list(filter(re.compile(r'.*\.xml').search, tweetinviBuildFiles))

    for dll in tweetinviDllFiles:
        copyfile('../src/Tweetinvi/bin/Release/netstandard1.4/' + dll, './TweetinviAPI/lib/netstandard1.4/' + dll)
        copyfile('../src/Tweetinvi/bin/Release/netstandard2.0/' + dll, './TweetinviAPI/lib/netstandard2.0/' + dll)

    for xml in tweetinviXmlFiles:
        copyfile('../src/Tweetinvi/bin/Release/netstandard1.4/' + xml, './TweetinviAPI/lib/netstandard1.4/' + xml)
        copyfile('../src/Tweetinvi/bin/Release/netstandard2.0/' + xml, './TweetinviAPI/lib/netstandard2.0/' + xml)

    os.system('cd TweetinviAPI && ..\\nuget.exe pack')


def build_nuget_symbol():
    print('building symbols package')

    tweetinviBuildFiles = os.listdir('../src/Tweetinvi/bin/Release/netstandard2.0')
    symbolsFilter = re.compile(r'.*\.pdb')
    tweetinviSymbolFiles = list(filter(symbolsFilter.search, tweetinviBuildFiles))

    for pdb in tweetinviSymbolFiles:
        copyfile('../src/Tweetinvi/bin/Release/netstandard1.4/' + pdb,
                 './TweetinviAPI-Symbols/lib/netstandard1.4/' + pdb)
        copyfile('../src/Tweetinvi/bin/Release/netstandard2.0/' + pdb,
                 './TweetinviAPI-Symbols/lib/netstandard2.0/' + pdb)

    os.system('cd TweetinviAPI-Symbols && ..\\nuget.exe pack')
    os.rename('./TweetinviAPI-Symbols/TweetinviAPI.' + version + '.nupkg', './TweetinviAPI-Symbols/TweetinviAPI.' + version + '.snupkg')


update_version()
clean_build()
compile()
clean_nuget_folder()

build_nuget_package()
build_nuget_symbol()
