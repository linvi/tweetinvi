#! /usr/local/bin/python3

import os
import re
import glob
import argparse
import sys

from shutil import copyfile, rmtree
from pathlib import Path

parser = argparse.ArgumentParser()
parser.add_argument('--version', nargs=1, default='5.0.4')
parser.add_argument('--pre', nargs=1, default='')
parser.add_argument('--build-version', action='store_true')
parser.add_argument('--nuget-push', action='store_true')
args = parser.parse_args()

version = args.version
nugetVersion = version + args.pre

if args.build_version:
    print(nugetVersion)
    sys.exit(0)


if args.nuget_push:
    print('This is going to publish 3 packages on nuget.org as version' + nugetVersion + '. Please confirm by typing "continue"')
    answer = input()
    if answer == 'continue':
        print('publishing on nuget...')
        os.system('cd TweetinviAPI && nuget push TweetinviAPI.' + nugetVersion + '.nupkg -Source nuget.org')
        os.system('cd TweetinviAPI-Symbols && nuget push TweetinviAPI.' + nugetVersion + '.snupkg -Source nuget.org')
        os.system('cd TweetinviAspNet && nuget push TweetinviAPI.AspNetPlugin.' + nugetVersion + '.nupkg -Source nuget.org')
    else:
        print('nuget push aborted')
    sys.exit(0)


srcFolders = os.listdir('../src/')
tweetinviProjects = list(filter(lambda folder: "Tweetinvi" in folder, srcFolders))


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


def createPath(path):
    try:
        os.makedirs(path)
    except FileExistsError:
        print("Directory ", path,  " already exists")


def update_version():
    nuspecVersionRegex = re.compile(r'<version>.*</version>')
    nuspecNewVersion = '<version>' + nugetVersion + '</version>'
    replace('./TweetinviAPI/TweetinviAPI.nuspec', nuspecVersionRegex, nuspecNewVersion)
    replace('./TweetinviAPI-Symbols/TweetinviAPI-Symbols.nuspec', nuspecVersionRegex, nuspecNewVersion)
    replace('./TweetinviAspNet/TweetinviAPI.AspNetPlugin.nuspec', nuspecVersionRegex, nuspecNewVersion)


    nugetDependency = '<dependency id="TweetinviAPI" version="' + nugetVersion + '" />'
    aspnetPluginTweetinviDependencyVersionRegex = re.compile(r'<dependency id="TweetinviAPI" version=".*" \/>')
    replace('./TweetinviAspNet/TweetinviAPI.AspNetPlugin.nuspec', aspnetPluginTweetinviDependencyVersionRegex, nugetDependency)

    userAgentRegex = re.compile(r'"User-Agent",\s"Tweetinvi/.+"')
    replace('../src/Tweetinvi.WebLogic/TwitterClientHandler.cs', userAgentRegex, '"User-Agent", "Tweetinvi/' + nugetVersion + '"')
    print('updated nuspec versions to ' + version)

    csprojVersionRegex = re.compile(r'<VersionPrefix>.*</VersionPrefix>')
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

    createPath('./TweetinviAPI/lib/netstandard1.4')
    createPath('./TweetinviAPI/lib/netstandard2.0')

    files = glob.glob('./TweetinviAPI-Symbols/*.snupkg')
    for f in files:
        os.remove(f)

    remove_files_in('./TweetinviAPI-Symbols/lib/netstandard1.4')
    remove_files_in('./TweetinviAPI-Symbols/lib/netstandard2.0')

    createPath('./TweetinviAPI-Symbols/lib/netstandard1.4')
    createPath('./TweetinviAPI-Symbols/lib/netstandard2.0')

    files = glob.glob('./TweetinviAspNet/*.nupkg')
    for f in files:
        os.remove(f)

    createPath('./TweetinviAspNet/lib/netstandard2.0')
    remove_files_in('./TweetinviAspNet/lib/netcoreapp2.1')
    remove_files_in('./TweetinviAspNet/lib/netcoreapp3.1')

    createPath('./TweetinviAspNet/lib/netstandard2.0')
    createPath('./TweetinviAspNet/lib/netcoreapp2.1')
    createPath('./TweetinviAspNet/lib/netcoreapp3.1')


def build_tweetinvi_nuget_package():
    print('Building nuget package...')
    tweetinviBuildFiles = os.listdir('../src/Tweetinvi/bin/release/netstandard2.0')
    tweetinviDllFiles = list(filter(re.compile(r'.*\.dll').search, tweetinviBuildFiles))
    tweetinviXmlFiles = list(filter(re.compile(r'.*\.xml').search, tweetinviBuildFiles))

    for dll in tweetinviDllFiles:
        copyfile('../src/Tweetinvi/bin/release/netstandard1.4/' + dll, './TweetinviAPI/lib/netstandard1.4/' + dll)
        copyfile('../src/Tweetinvi/bin/release/netstandard2.0/' + dll, './TweetinviAPI/lib/netstandard2.0/' + dll)

    for xml in tweetinviXmlFiles:
        copyfile('../src/Tweetinvi/bin/release/netstandard1.4/' + xml, './TweetinviAPI/lib/netstandard1.4/' + xml)
        copyfile('../src/Tweetinvi/bin/release/netstandard2.0/' + xml, './TweetinviAPI/lib/netstandard2.0/' + xml)

    os.system('cd TweetinviAPI && nuget pack')


def build_tweetinvi_nuget_symbols():
    print('building symbols package')

    tweetinviBuildFiles = os.listdir('../src/Tweetinvi/bin/release/netstandard2.0')
    symbolsFilter = re.compile(r'.*\.pdb')
    tweetinviSymbolFiles = list(filter(symbolsFilter.search, tweetinviBuildFiles))

    for pdb in tweetinviSymbolFiles:
        copyfile('../src/Tweetinvi/bin/release/netstandard1.4/' + pdb, './TweetinviAPI-Symbols/lib/netstandard1.4/' + pdb)
        copyfile('../src/Tweetinvi/bin/release/netstandard2.0/' + pdb, './TweetinviAPI-Symbols/lib/netstandard2.0/' + pdb)

    os.system('cd TweetinviAPI-Symbols && nuget pack')
    os.rename('./TweetinviAPI-Symbols/TweetinviAPI.' + nugetVersion + '.nupkg', './TweetinviAPI-Symbols/TweetinviAPI.' + nugetVersion + '.snupkg')


def build_aspNet_nuget_package():
    def copy_aspnet_file(filepath):
        copyfile('../src/Tweetinvi.AspNet/bin/release/' + filepath, './TweetinviAspNet/lib/' + filepath)

    Path("./TweetinviAspNet/lib/netstandard2.0").mkdir(parents=True, exist_ok=True)
    Path("./TweetinviAspNet/lib/netcoreapp2.1").mkdir(parents=True, exist_ok=True)
    Path("./TweetinviAspNet/lib/netcoreapp3.1").mkdir(parents=True, exist_ok=True)

    copy_aspnet_file('netstandard2.0/Tweetinvi.AspNet.dll')
    copy_aspnet_file('netstandard2.0/Tweetinvi.AspNet.xml')

    copy_aspnet_file('netcoreapp2.1/Tweetinvi.AspNet.dll')
    copy_aspnet_file('netcoreapp2.1/Tweetinvi.AspNet.xml')

    copy_aspnet_file('netcoreapp3.1/Tweetinvi.AspNet.dll')
    copy_aspnet_file('netcoreapp3.1/Tweetinvi.AspNet.xml')

    os.system('cd TweetinviAspNet && nuget pack')


update_version()
clean_build()
compile_tweetinvi()
clean_nuget_folder()

build_tweetinvi_nuget_package()
build_tweetinvi_nuget_symbols()
build_aspNet_nuget_package()